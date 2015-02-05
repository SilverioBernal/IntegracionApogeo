using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace IntegracionApogeo.Business.Entities.Excepciones
{
    /// <summary>
    /// Excepción generada en el procesamiento contra SAP
    /// </summary>
    public class SAPException : Exception
    {
        #region Propiedades
        /// <summary>
        /// Número de error generado por SAP Business One
        /// </summary>
        public int NumeroError { set; get; }
        /// <summary>
        /// Descripción de la incidencia generada en SAP
        /// </summary>
        public string Descripcion { set; get; }
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="NumeroError">Número de error generado por SAP Business One</param>
        /// <param name="Descripcion">Descripción de la incidencia generada en SAP</param>
        public SAPException(int NumeroError, string Descripcion)
        {
            this.NumeroError = NumeroError;
            this.Descripcion = Descripcion;
        }
        #endregion

    }
}
