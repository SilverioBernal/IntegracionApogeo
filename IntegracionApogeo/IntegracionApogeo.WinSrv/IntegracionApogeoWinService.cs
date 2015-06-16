using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using IntegracionApogeo.Business;
using IntegracionApogeo.Business.Entities;
using IntegracionApogeo.Business.Entities.Seguridad;
using SAPbobsCOM;
using System.Runtime.InteropServices;




namespace IntegracionApogeo.WinSrv
{
    public partial class IntegracionApogeoWinService : ServiceBase
    {
        private System.Timers.Timer TmrTemporizador;
        private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog lgRegistroDeEventos;

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);


        public IntegracionApogeoWinService()
        {
            InitializeComponent();
            lgRegistroDeEventos = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("IntegraciónApogeo"))
            {
                System.Diagnostics.EventLog.CreateEventSource("IntegraciónApogeo", "TxLog");
            }
            lgRegistroDeEventos.Source = "IntegraciónApogeo";
            lgRegistroDeEventos.Log = "TxLog";
        }

        protected override void OnStart(string[] args)
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            lgRegistroDeEventos.WriteEntry("Inicialización IntegraciónApogeo WinSrv" + DateTime.Now);
            Monitorear();
        }

        protected override void OnStop()
        {
            lgRegistroDeEventos.WriteEntry("Finalización  IntegraciónApogeo WinSrv" + DateTime.Now);
            TmrTemporizador.Enabled = false;
        }

        public void Monitorear()
        {
            TmrTemporizador = new System.Timers.Timer();
            TmrTemporizador.Interval = double.Parse(System.Configuration.ConfigurationSettings.AppSettings["RecicladoMS"]);//60000; // 60 seconds


            //TmrTemporizador.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            TmrTemporizador.Elapsed += new System.Timers.ElapsedEventHandler(DesencadenarEventoCiclico);

            TmrTemporizador.Start();
        }


        private static void DesencadenarEventoCiclico(object source, System.Timers.ElapsedEventArgs e)
        {
            //SAP_CABEZA_ASIENTO oCabezaAsiento = new SAP_CABEZA_ASIENTO();
            BusinessExternalDB oExtCabezaAsiento = new BusinessExternalDB();
            BusinessFachade fachada;
            ConexionWS conexion = new ConexionWS();
            Business.Entities.Asientos.Asiento miAsientoContable = new Business.Entities.Asientos.Asiento();


            List<SAP_CABEZA_ASIENTO> lsCabezaAsiento = oExtCabezaAsiento.GetSAP_CABEZA_ASIENTOList("N");
            foreach (SAP_CABEZA_ASIENTO itemCabezaAsiento in lsCabezaAsiento)
            {
                ///apCabezaAsiento.AutoVAT
                //JournalEntries miAsientoContable;
                //IntegracionApogeo.Business.Entities.Asientos miAsientoContable;


                ///repetido miAsientoContable.TransCode = itemCabezaAsiento.TransCode;

                if (itemCabezaAsiento.Memo.ToString().Length > 50)
                    miAsientoContable.Memo = itemCabezaAsiento.Memo.ToString().Substring(0, 49);
                else
                    miAsientoContable.Memo = itemCabezaAsiento.Memo.ToString();



                miAsientoContable.Memo = itemCabezaAsiento.Memo == null ? "" : itemCabezaAsiento.Memo;

                miAsientoContable.Ref1 = itemCabezaAsiento.Ref1 == null ? "" : itemCabezaAsiento.Ref1;
                miAsientoContable.Ref2 = itemCabezaAsiento.Ref2 == null ? "" : itemCabezaAsiento.Ref2;
                miAsientoContable.TransCode = itemCabezaAsiento.TransCode == null ? "" : itemCabezaAsiento.TransCode;
                miAsientoContable.Project = itemCabezaAsiento.Project == null ? "" : itemCabezaAsiento.Project;


                if (!string.IsNullOrEmpty(itemCabezaAsiento.StampTax))
                    miAsientoContable.StampTax = itemCabezaAsiento.StampTax;

                
                List<SAP_DETALLE_ASIENTO> lsDetalleAsiento = oExtCabezaAsiento.GetSAP_DETALLE_ASIENTOById(Convert.ToInt32(itemCabezaAsiento.TransId));
                // Adicion de detalle de asiento 



                foreach (SAP_DETALLE_ASIENTO linea in lsDetalleAsiento)
                {

                    Business.Entities.Asientos.AsientoDetalle elinea = new Business.Entities.Asientos.AsientoDetalle();




                    elinea.Account = linea.Account;
                    elinea.Debit = Convert.ToDouble(linea.Debit);
                    elinea.Credit = Convert.ToDouble(linea.Credit);
                    elinea.Ref1 = linea.Ref1 == null ? "" : linea.Ref1;
                    elinea.Ref2 = linea.Ref2 == null ? "" : linea.Ref2;
                    elinea.Ref3Line = linea.Ref3Line == null ? "" : linea.Ref3Line;

                    if ((Convert.ToDateTime(linea.TaxDate).Year != 1))
                        miAsientoContable.TaxDate = Convert.ToDateTime(linea.TaxDate);

                    //if (linea.DuoDate != null || linea.DuoDate.Year != 1)
                    //  elinea.DueDate = linea.DuoDate;

                    elinea.LineMemo = linea.LineMemo == null ? "" : linea.LineMemo;

                    if (linea.RefDate != null)
                    {
                        if (Convert.ToDateTime(linea.RefDate).Year != 1)
                            elinea.RefDate = linea.RefDate == null ? System.DateTime.Now : Convert.ToDateTime(linea.RefDate);
                    }


                    elinea.LineMemo = linea.LineMemo == null ? "" : linea.LineMemo;
                    elinea.Project = linea.Project == null ? "" : linea.Project;

                    if (linea.TaxDate != null)
                    {
                        if (Convert.ToDateTime(linea.TaxDate).Year != 1)
                            elinea.TaxDate = linea.TaxDate == null ? System.DateTime.Now : Convert.ToDateTime(linea.TaxDate);
                    }
                    elinea.ProfitCode = linea.ProfitCode == null ? "" : linea.ProfitCode;
                    elinea.OcrCode2 = linea.OcrCode2 == null ? "" : linea.OcrCode2;
                    elinea.OcrCode3 = linea.OcrCode3 == null ? "" : linea.OcrCode3;

                    //UDF Values
                    elinea.U_InfoCo01 = linea.U_InfoCo01 == null ? "" : linea.U_InfoCo01;

                    miAsientoContable.lineas.Add(elinea);
                }

                try
                {
                    int iResultadoDelCargue;

                    fachada = new BusinessFachade(BusinessFachade.BusinessClass.BusinessAsiento);

                    iResultadoDelCargue = fachada.CrearAsiento(miAsientoContable, conexion);

                    if (iResultadoDelCargue > 0) {
                        itemCabezaAsiento.ESTADO = "P";
                        oExtCabezaAsiento.SaveSAP_CABEZA_ASIENTO(itemCabezaAsiento);
                    }
                       
                }
                catch (Exception ex)
                {

                }



            }







            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

        }
    }
}
