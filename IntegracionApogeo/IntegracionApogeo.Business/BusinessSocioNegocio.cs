using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegracionApogeo.Business.Entities.SociosNegocio;
using IntegracionApogeo.Business.Entities;
using IntegracionApogeo.Business.Entities.Seguridad;
using IntegracionApogeo.Business.Entities.Excepciones;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using IntegracionApogeo.Data;

namespace IntegracionApogeo.Business
{
    /// <summary>
    /// Clase que controla todo el negocio de las transacciones relacionadas con socios de negocio
    /// </summary>
    public class BusinessSocioNegocio
    {
        #region Atributos
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private DataSocio accesoSocio;
        public DataConexionSAP midataConexion;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public BusinessSocioNegocio()
        {
            midataConexion = new DataConexionSAP();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Consulta de socio de negocios de tipo cliente por código
        /// </summary>
        /// <param name="codigoSocio">Código del socio de negocios en SAP Business One</param>
        /// <param name="conexion">Conexion con el servicio</param>
        /// <returns>Información de socio de negocios</returns>
        public SocioNegocio ConsultarSocio(string codigoSocio, ConexionWS conexion)
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                accesoSocio = new DataSocio();

                SocioNegocio socio = accesoSocio.ConsultarSocio(codigoSocio);

                //if (socio.CardCode.Length == 0)
                //    throw new BusinessException(42, "El valor enviado: " + codigoSocio + "no esta registrado como un cliente en SAP Business One");
                return socio;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            catch (BusinessException ex)
            {
                Util.ProcesarBusinessException(ex);
            }
            catch (Exception ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            return null;
        }

        /// <summary>
        /// Método para la creacion de socios de negocio en SAP
        /// </summary>
        /// <param name="socio"></param>
        /// <param name="conexion"></param>
        public void CrearSocio(SocioNegocio socio, ConexionWS conexion)
        {
            try
            {
                if (!Util.ValidarDatosAccesoServicio(conexion))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                accesoSocio = new DataSocio();
                if (midataConexion.Conectar())
                {
                    midataConexion.IniciarTransaccion();
                    accesoSocio.CrearSocio(socio);
                    midataConexion.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                }
            }
            catch (SAPException ex)
            {
                midataConexion.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Util.ProcesarSapException(ex, "Creacion de socio");
            }
            catch (DbException ex)
            {
                Exception outEx;
                midataConexion.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            catch (BusinessException ex)
            {
                midataConexion.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                ex.Data.Add("1", ex.IdError);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Mensaje);
                throw ex;
            }
            catch (Exception ex)
            {
                Exception outEx;
                midataConexion.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }

        }
        #endregion
    }
}

