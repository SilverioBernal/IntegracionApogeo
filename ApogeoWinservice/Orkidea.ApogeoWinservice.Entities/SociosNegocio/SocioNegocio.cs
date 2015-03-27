using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Orkidea.ApogeoWinservice.Entities.SociosNegocio
{
    /// <summary>
    /// Representa un socio de negocios en SAP Business One
    /// </summary>
    [DataContract(Namespace = "http://ApogeoWinservice")]
    public class SocioNegocio
    {
        public long id { get; set; }
        /// <summary>
        /// Código del socio de negocios
        /// </summary>
        [DataMember]
        public string CardCode { set; get; }
        /// <summary>
        /// Nombre del socio de negocios
        /// </summary>
        [DataMember]
        public string DebPayAcct { get; set; }
        [DataMember]
        public int? Territory { get; set; }
        [DataMember]
        public string AccCritria { get; set; }
        [DataMember]
        public string BlockDunn { get; set; }
        [DataMember]
        public string CardName { set; get; }
        [DataMember]
        public string CardFName { get; set; }
        [DataMember]
        public string CardType { get; set; }

        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Teléfono Celular
        /// </summary>
        [DataMember]
        public string Cellular { set; get; }
        [DataMember]
        public string CollecAuth { get; set; }
        [DataMember]
        public double CreditLine { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string DeferrTax { get; set; }
        /// <summary>
        /// E_Mail
        /// </summary>
        [DataMember]
        public string E_Mail { set; get; }
        [DataMember]
        public string Equ { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DataMember]
        public string Fax { set; get; }
        /// <summary>
        /// Cédula o NIT
        /// </summary>
        [DataMember]
        public string LicTradNum { set; get; }
        /// <summary>
        /// Teléfono 1
        /// </summary>
        [DataMember]
        public string Phone1 { set; get; }
        /// <summary>
        /// Teléfono 2
        /// </summary>
        [DataMember]
        public string Phone2 { set; get; }
        /// <summary>
        /// Inicializa atributos
        /// </summary>
        public SocioNegocio()
        {
            this.CardCode = String.Empty;
            this.CardName = String.Empty;
            this.LicTradNum = String.Empty;
            this.Phone1 = String.Empty;
            this.Phone2 = String.Empty;
            this.Cellular = String.Empty;
            this.Fax = String.Empty;
            this.E_Mail = String.Empty;
        }
    }
}
