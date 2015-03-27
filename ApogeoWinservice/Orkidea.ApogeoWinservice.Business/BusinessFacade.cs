
using Orkidea.ApogeoWinservice.Entities.Asientos;
using Orkidea.ApogeoWinservice.Entities.Seguridad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.ApogeoWinservice.Business
{
    /// <summary>
    /// Clase fachada usada para el mediar entre el servicio y la capa de negocios
    /// </summary>
    public class BusinessFacade
    {
        #region Métodos
        /// <summary>
        /// Crea un asientos contable en SAP
        /// </summary>
        /// <param name="asientos">Objeto de tipo asientos contable</param>
        /// <param name="conexion">Objeto conexion</param>
        /// <returns></returns>
        public void SincronizarAsientosSocios()
        {
            BusinessSocioNegocio bizSocios = new BusinessSocioNegocio();
            BusinessAsientoContable bizAsientos = new BusinessAsientoContable();
            
            bizSocios.SincronizarSocios();
            bizAsientos.SincronizarAsientosContables();                        
        }
        #endregion
    }
}

