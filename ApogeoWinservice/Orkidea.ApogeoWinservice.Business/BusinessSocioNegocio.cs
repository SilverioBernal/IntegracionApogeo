using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.ApogeoWinservice.Entities.SociosNegocio;
using Orkidea.ApogeoWinservice.Entities.Seguridad;
using Orkidea.ApogeoWinservice.Entities.Excepciones;
using Orkidea.ApogeoWinservice.SAP;

namespace Orkidea.ApogeoWinservice.Business
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
        public void SincronizarSocios()
        {
            BusinessExternalDB externalData = new BusinessExternalDB();
            List<SocioNegocio> lsPendingPartners = externalData.GetPendingBusinessPartners();

            foreach (SocioNegocio item in lsPendingPartners)
            {
                SocioNegocio socio = ConsultarSocio(item.LicTradNum);

                if (string.IsNullOrEmpty(socio.CardCode))
                    CrearSocio(item);

                externalData.PartnerSynchronized(item);
            }
        }

        /// <summary>
        /// Consulta de socio de negocios de tipo cliente por código
        /// </summary>
        /// <param name="codigoSocio">Código del socio de negocios en SAP Business One</param>
        /// <param name="conexion">Conexion con el servicio</param>
        /// <returns>Información de socio de negocios</returns>
        private SocioNegocio ConsultarSocio(string codigoSocio)
        {
            try
            {
                accesoSocio = new DataSocio();

                SocioNegocio socio = accesoSocio.ConsultarSocio(codigoSocio);

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
        private void CrearSocio(SocioNegocio socio)
        {
            try
            {

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

