using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Orkidea.ApogeoWinservice.Entities.Seguridad
{
    /// <summary>
    /// Conexión con el Servicio Web
    /// </summary>
    [DataContract(Namespace = "http://ApogeoWinservice")]
    public class ConexionWS
    {
        /// <summary>
        /// Usuario del Servicio Web
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Usuario { set; get; }
        /// <summary>
        /// Contraseña del Servicio Web
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Contrasena { set; get; }
        /// <summary>
        /// Usuario para la trazabilidad de las operaciones del Servicio Web
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UsuarioIntegracionApogeo { set; get; }
        /// <summary>
        /// Base de datos de SAP para realizar las pruebas
        /// </summary>
        string DataBase { set; get; }
    }
}
