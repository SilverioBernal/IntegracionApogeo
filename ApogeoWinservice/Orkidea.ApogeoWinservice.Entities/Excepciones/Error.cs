using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Orkidea.ApogeoWinservice.Entities.Excepciones
{
    /// <summary>
    /// Encapsula las excepciones generadas para que sean enviadas al servicio WCF.
    /// </summary>
    [DataContract(Namespace = "http://ApogeoWinservice")]
    public class Error
    {
        #region Propiedades

        /// <summary>
        /// ID del error enviado por la capa de negocios
        /// </summary>
        public int IdError { set; get; }
        /// <summary>
        /// Descripcion del error enviado por la capa de negocios
        /// </summary>
        public string Descripcion { set; get; }
        /// <summary>
        /// Número de error generado por SAP Business One
        /// </summary>
        public string NumeroError { set; get; }
        /// <summary>
        /// Proceso donde ocurrio el error
        /// </summary>
        public string Proceso { set; get; }

        #endregion
    }
}
