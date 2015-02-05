using System;
using System.Configuration; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using SAPbobsCOM;
using IntegracionApogeo.Business.Entities;
//using System.Configuration;
using IntegracionApogeo.Business.Entities.Seguridad;
using IntegracionApogeo.Business.Entities.Excepciones;
 
//using DataConexionSAP.

namespace IntegracionApogeo.data
{
    class Utilidades
    {
        #region Atributos
        /// <summary>
        /// Atributos de conexión a la base de datos
        /// </summary>
        private Database baseDatos;
        /// <summary>
        /// Lector
        /// </summary>
        private IDataReader reader;
        /// <summary>
        /// Enumeración para el tipo de operación
        /// </summary>
        public enum tipoOperacion
        {

            EntradaCompras,
            OrdenProduccion,
            Transferencia,
            ProductoTerminado,
            RegistroConsumos,
            EntregaVentas
        }
        /// <summary>
        /// Tipo de Operación.
        /// </summary>
        public tipoOperacion TipoOperacion { set; get; }
        #endregion


        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Utilidades()
        {
            this.baseDatos = DatabaseFactory.CreateDatabase("SAP");
        }
        #endregion

        public void ConsultarActualizaciones(tipoOperacion tipoOperacion, string userWS, int numeroDocumento)
        {
            StringBuilder miSentencia = new StringBuilder("SELECT ISNULL(MAX(CONVERT(int,Code)),0) FROM [@WSSAP_LOG]");
            DbCommand miComando = this.baseDatos.GetSqlStringCommand(miSentencia.ToString());
            if (ConfigurationManager.AppSettings["RegistroLog"].Equals("SI"))
            {
                int numerolog = (Int32)this.baseDatos.ExecuteScalar(miComando);
                UserTable tablaLog = (UserTable)ConexionSAP.Conexion.compania.UserTables.Item("WSSAP_Log");
                tablaLog.Code = (numerolog + 1).ToString();
                tablaLog.Name = (numerolog + 1).ToString();
                tablaLog.UserFields.Fields.Item("U_Fecha").Value = DateTime.Now;
                tablaLog.UserFields.Fields.Item("U_Hora").Value = DateTime.Now;
                tablaLog.UserFields.Fields.Item("U_UsuarioWSSAP").Value = "";
                tablaLog.UserFields.Fields.Item("U_TipoOperacion").Value = tipoOperacion.ToString();
                tablaLog.UserFields.Fields.Item("U_NumeroDocumento").Value = numeroDocumento.ToString();
                if (tablaLog.Add() != 0)
                {
                    throw new SAPException(ConexionSAP.Conexion.compania.GetLastErrorCode(), ConexionSAP.Conexion.compania.GetLastErrorDescription());
                }
            }
        }
    }
}
