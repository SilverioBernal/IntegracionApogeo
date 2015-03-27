using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net.Mail;
using System.Text.RegularExpressions;
using SAPbobsCOM;
using System.Data.Common;
using Orkidea.ApogeoWinservice.Entities.Excepciones;
using Orkidea.ApogeoWinservice.Entities.Seguridad;


namespace Orkidea.ApogeoWinservice.Business
{
    /// <summary>
    /// Utilizades generales de la aplicacion
    /// </summary>
    public static class Util
    {
        #region Métodos
        /// <summary>
        /// Metodo encargado de hacer la autenticacion de la apliacion externa con el servicio
        /// </summary>
        /// <param name="unaConexionWS">Contiene los datos de conexion a SAP Business One </param>
        /// <returns>Bool que indica la validez de la conexion </returns>
        public static bool ValidarDatosAccesoServicio(ConexionWS unaConexionWS)
        {
            if (!ConfigurationManager.AppSettings["usuario"].Equals(unaConexionWS.Usuario))
                return false;
            if (!ConfigurationManager.AppSettings["contrasena"].Equals(unaConexionWS.Contrasena))
                return false;
            return true;
        }

        /// <summary>
        /// Metodo para procesar las excepciones de tipo BALException
        /// </summary>
        /// <param name="ex">Excepcion ocurrida</param>
        public static void ProcesarBusinessException(BusinessException ex)
        {
            ex.Data.Add("1", "16");
            ex.Data.Add("2", "NA");
            ex.Data.Add("3", ex.Mensaje);
            throw ex;
        }

        /// <summary>
        /// Metodo para procesar las excepciones de tipo SAPException
        /// </summary>
        /// <param name="ex">Excepcion ocurrida</param>
        /// <param name="unProceso">Proceso que lanza la excepcion</param>
        public static void ProcesarSapException(SAPException ex, string unProceso)
        {
            Exception outEx;
            switch (Math.Abs(ex.NumeroError))
            {
                case 103:
                    ExceptionPolicy.HandleException(ex, "Politica_ConexionBdSAP", out outEx);
                    outEx.Data.Add("1", "13");
                    outEx.Data.Add("2", "103");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 104:
                    ExceptionPolicy.HandleException(ex, "Politica_ConexionLicenciaSAP", out outEx);
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "104");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 105:
                    ExceptionPolicy.HandleException(ex, "Politica_ObserverSAP", out outEx);
                    outEx.Data.Add("1", "15");
                    outEx.Data.Add("2", "105");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 106:
                    ExceptionPolicy.HandleException(ex, "Politica_SinConexionSAP", out outEx);
                    outEx.Data.Add("1", "16");
                    outEx.Data.Add("2", "106");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 107:
                    ExceptionPolicy.HandleException(ex, "Politica_AutenticacionSAP", out outEx);
                    outEx.Data.Add("1", "17");
                    outEx.Data.Add("2", "107");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 109:
                    ExceptionPolicy.HandleException(ex, "Politica_CopiandoDllSAP", out outEx);
                    outEx.Data.Add("1", "18");
                    outEx.Data.Add("2", "109");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 110:
                    ExceptionPolicy.HandleException(ex, "Politica_AbriendoObserverSAP", out outEx);
                    outEx.Data.Add("1", "19");
                    outEx.Data.Add("2", "110");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 111:
                    ExceptionPolicy.HandleException(ex, "Politica_SBOCommonSAP", out outEx);
                    outEx.Data.Add("1", "20");
                    outEx.Data.Add("2", "111");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 112:
                    ExceptionPolicy.HandleException(ex, "Politica_DllCabSAP", out outEx);
                    outEx.Data.Add("1", "21");
                    outEx.Data.Add("2", "112");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 113:
                    ExceptionPolicy.HandleException(ex, "Politica_DirectorioTemporalSAP", out outEx);
                    outEx.Data.Add("1", "22");
                    outEx.Data.Add("2", "113");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                default:
                    ExceptionPolicy.HandleException(ex, "Politica_ExcepcionSAP", out outEx);
                    outEx.Data.Add("1", "10");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Número Error: " + ex.NumeroError + " Descripción: " + ex.Descripcion);
                    break;
            }
            throw outEx;
        }
        #endregion
    }
}

