using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace IntegracionApogeo.Business.Entities.Excepciones
{
    /// <summary>
    /// Encapsula las excepciones enviadas desde la capa de negocio
    /// </summary>
    public class BusinessException : Exception
    {
        #region Atributos
        /// <summary>
        /// ID del error enviado por la capa de negocios
        /// </summary>
        public int IdError { set; get; }
        /// <summary>
        /// Mensaje de error asociado
        /// </summary>
        public string Mensaje { set; get; }
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializa las propiedades del objeto
        /// </summary>
        /// <param name="IdError">ID del Error</param>     
        /// <param name="Mensaje">Mensaje</param>
        public BusinessException(int IdError, string Mensaje)
        {
            this.IdError = IdError;
            this.Mensaje = Mensaje;
        }
        #endregion

    }
}
