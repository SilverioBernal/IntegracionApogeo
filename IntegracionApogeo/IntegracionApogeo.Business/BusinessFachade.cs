using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using IntegracionApogeo.Business.Entities.Seguridad;
using System.Collections;
using IntegracionApogeo.Business.Entities.SociosNegocio;
using IntegracionApogeo.Business.Entities.Asientos;

namespace IntegracionApogeo.Business
{
    /// <summary>
    /// Clase fachada usada para el mediar entre el servicio y la capa de negocios
    /// </summary>
    public class BusinessFachade
    {
        #region Atributos
        /// <summary>
        /// Enumeración para el tipo de clases de negocio
        /// </summary>
        public enum BusinessClass { BusinessSocio, BusinessAsiento };
        /// <summary>
        /// Atributo Socio para la clase de negocios
        /// </summary>
        BusinessSocioNegocio socios;
        BusinessAsientoContable asientos;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="classType">Tipo de clase de negocios a instanciar</param>
        public BusinessFachade(BusinessClass classType)
        {
            switch (classType)
            {

                case BusinessClass.BusinessSocio: socios = new BusinessSocioNegocio();
                    break;
                case BusinessClass.BusinessAsiento: asientos = new BusinessAsientoContable();
                    break;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Crea un asientos contable en SAP
        /// </summary>
        /// <param name="asientos">Objeto de tipo asientos contable</param>
        /// <param name="conexion">Objeto conexion</param>
        /// <returns></returns>
        public int CrearAsiento(Asiento asientoContable, ConexionWS conexion)
        {
            return (asientos.CrearAsiento(asientoContable, conexion));
        }
        #endregion
    }
}

