using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionApogeo.WinService
{
    public partial class IntegracionApogeoWinSrv : ServiceBase
    {
        private System.Timers.Timer TmrTemporizador;

        public IntegracionApogeoWinSrv()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
            
            TmrTemporizador.Enabled = false;

        }
        public void Monitorear() {
            TmrTemporizador = new System.Timers.Timer();
            TmrTemporizador.Interval = double.Parse(System.Configuration.ConfigurationSettings.AppSettings["RecicladoMS"]);//60000; // 60 seconds
            

            //TmrTemporizador.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            TmrTemporizador.Elapsed += new System.Timers.ElapsedEventHandler(DesencadenarEventoCiclico);

            TmrTemporizador.Start();
        }

        private static void DesencadenarEventoCiclico(object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

        }

       
    }
}
