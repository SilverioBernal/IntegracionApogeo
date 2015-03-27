using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.ApogeoWinservice.Entities.Asientos;
using Orkidea.ApogeoWinservice.Entities.Excepciones;
using Orkidea.ApogeoWinservice.Entities.Seguridad;
using Orkidea.ApogeoWinservice.Entities.SociosNegocio;
using Orkidea.ApogeoWinservice.SAP;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Orkidea.ApogeoWinservice.Business
{
    public class BusinessAsientoContable
    {
        #region Atributos
        // ES ESTATICO NO REQUIERE Util utilidades;
        //ConexionSAP sapData;
        private DataConexionSAP sapData;
        private BusinessSocioNegocio socioData;
        private DataAsientoContable asientosData;
        #endregion

        #region Constructor
        public BusinessAsientoContable()
        {
            asientosData = new DataAsientoContable();
            //no requiere implementación utilidades = new Util();
            sapData = new DataConexionSAP();
            //no requiere implementación socioData = new SociosSAP();
        }
        #endregion

        #region Métodos
        public void SincronizarAsientosContables()
        {
            BusinessExternalDB externalData = new BusinessExternalDB();
            List<Asiento> lsAsiento = externalData.GetPendingJournals();

            foreach (Asiento item in lsAsiento)
            {
                CrearAsiento(item);
                externalData.JournalSynchronized(item);
            }

        }

        /// <summary>
        /// Generar el pago en SAP
        /// </summary>
        /// <returns>Listado de Cuentas</returns>
        private int CrearAsiento(Asiento asientoContable)
        {
            int numeroAsiento = -1;
            if (sapData.Conectar())
            {
                #region Contenido del Asiento
                try
                {
                    if (asientoContable.lineas.Count > 0)
                    {                        
                        sapData.IniciarTransaccion();
                        numeroAsiento = asientosData.CrearAsiento(asientoContable);
                        sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    }
                    return numeroAsiento;
                }
                catch (SAPException ex)
                {
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Util.ProcesarSapException(ex, "Gestión de Pagos");
                    return numeroAsiento;
                }
                catch (COMException ex)
                {
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Exception outEx;
                    if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;
                    }
                    else
                    {
                        throw;
                    }
                    return numeroAsiento;
                }
                catch (Exception ex)
                {
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
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
                        return 0;
                    }
                    return numeroAsiento;
                }
                #endregion
                return numeroAsiento;
            }
            return numeroAsiento;
        }
        #endregion
    }
}
