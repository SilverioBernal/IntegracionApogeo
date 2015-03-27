using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAPbobsCOM;
using System.Configuration;
using Orkidea.ApogeoWinservice.Entities.Excepciones;
using Orkidea.ApogeoWinservice.Entities.Seguridad;


namespace Orkidea.ApogeoWinservice.SAP
{
    /// <summary>
    /// Clase para la conexión con SAP
    /// </summary>
    public class DataConexionSAP
    {
        static readonly object padlock = new object();
        static readonly object padlock2 = new object();

        #region Métodos
        /// <summary>
        /// Contiene la conexion a SAP Business One
        /// </summary>
        public ConexionSAP conexion;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataConexionSAP()
        {
            conexion = ConexionSAP.Conexion;
        }

        /// <summary>
        /// Inicia una transacción en SAP Business One
        /// </summary>
        /// <returns>Estado de la operación</returns>
        public bool IniciarTransaccion()
        {
            lock (padlock)
            {
                while (conexion.compania.InTransaction) { }
                conexion.compania.StartTransaction();
            }
            return true;
        }

        /// <summary>
        /// Liberar el objeto COM de acuerdo a buenas prácticas
        /// </summary>
        /// <returns>Bool, Indica el exito de la tarea de liberar la compañia</returns>
        public bool LiberarCompania()
        {
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(conexion.compania);
            conexion = null;
            ConexionSAP.Conexion = null;
            return true;
        }

        /// <summary>
        /// Permite Desconectar de SAP Business One despues de realizar las operaciones
        /// </summary>
        /// <returns>Bool, Indica el exito de la tarea de desconectar</returns>
        public bool Desconectar()
        {
            conexion.compania.Disconnect();
            return true;
        }

        /// <summary>
        /// Permite conectar con SAP Business One
        /// </summary>
        /// <returns>Estado de la operación</returns>
        public bool Conectar()
        {
            long nResult = 0;
            lock (padlock2)
            {
                if (!conexion.compania.Connected)
                    nResult = conexion.compania.Connect();
            }
            if (nResult != 0)
            {
                throw new SAPException(conexion.compania.GetLastErrorCode(), conexion.compania.GetLastErrorDescription());
            }
            return true;
        }

        /// <summary>
        /// Conecta a SAP Business One segun las credenciales enviadas
        /// </summary>
        /// <param name="unaDataBase">Compañia de SAP a la que se va conectar</param>
        /// <param name="unUserSAP">Usuario de autenticación con SAP</param>
        /// <param name="unPasswordSAP">Contraseña de autenticación con SAP</param>
        /// <returns>True|Conexión exitosa
        ///         False|Fallo en la conexión</returns>
        public bool Conectar(string unaDataBase, string unUserSAP, string unPasswordSAP)
        {
            long nResult = 0;
            conexion.compania.CompanyDB = unaDataBase;
            conexion.compania.UserName = unUserSAP;
            conexion.compania.Password = unPasswordSAP;

            if (!conexion.compania.Connected)
                nResult = conexion.compania.Connect();
            if (nResult != 0)
            {
                throw new SAPException(conexion.compania.GetLastErrorCode(), conexion.compania.GetLastErrorDescription());
            }
            return true;
        }

        /// <summary>
        /// Termina una transacción en SAP, exitosa o fallida
        /// </summary>
        /// <param name="opcionTransaccion">Opción de la transacción</param>
        /// <returns>Estado de la operación</returns>
        public bool TerminarTransaccion(BoWfTransOpt opcionTransaccion)
        {
            if (conexion.compania.Connected)
                if (conexion.compania.InTransaction)
                    conexion.compania.EndTransaction(opcionTransaccion);
            return true;
        }
        #endregion
    }
}
