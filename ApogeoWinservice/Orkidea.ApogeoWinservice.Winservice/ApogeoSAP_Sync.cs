using Orkidea.ApogeoWinservice.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.ApogeoWinservice.Winservice
{
    public partial class ApogeoSAP_Sync : ServiceBase
    {
        private System.Timers.Timer TmrTemporizador;
        //private System.ComponentModel.IContainer components;
        //private System.Diagnostics.EventLog lgRegistroDeEventos;

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


        public ApogeoSAP_Sync()
        {
            InitializeComponent();
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


            lgRegistroDeEventos.WriteEntry("Apogeo SAP Sync service start on " + DateTime.Now);

            TmrTemporizador = new System.Timers.Timer();
            TmrTemporizador.Interval = double.Parse(ConfigurationManager.AppSettings["RecicladoMS"]);//60000 = 60 seconds            
            TmrTemporizador.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);

            TmrTemporizador.Start();
        }

        protected override void OnStop()
        {
            lgRegistroDeEventos.WriteEntry("Apogeo SAP Sync service stop on " + DateTime.Now);
            TmrTemporizador.Enabled = false;
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            BusinessFacade bizFacade = new BusinessFacade();

            lgRegistroDeEventos.WriteEntry("Apogeo SAP Sync process has been start on " + DateTime.Now);

            bizFacade.SincronizarAsientosSocios();

            lgRegistroDeEventos.WriteEntry(string.Format("Apogeo SAP Sync process has been finish successfully on {0} please see the log file", DateTime.Now));
        }

        
    }
}
