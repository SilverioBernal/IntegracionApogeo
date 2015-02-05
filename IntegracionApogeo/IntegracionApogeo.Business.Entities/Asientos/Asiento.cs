using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IntegracionApogeo.Business.Entities.Asientos
{
    /// <summary>
    /// Entidad para la gestion de asientos contables
    /// </summary>
    [DataContract(Namespace = "http://IntegracionApogeo")]
    public class Asiento
    {
        #region Atributos
        [DataMember]
        public DateTime RefDate { get; set; }
        [DataMember]
        public string Memo { get; set; }
        [DataMember]
        public string Ref1 { get; set; }
        [DataMember]
        public string Ref2 { get; set; }
        [DataMember]
        public string TransCode { get; set; }
        [DataMember]
        public string Project { get; set; }
        [DataMember]
        public DateTime TaxDate { get; set; }
        [DataMember]
        public DateTime VatDate { get; set; }
        [DataMember]
        public string StampTax { get; set; }
        [DataMember]
        public string AutoVat { get; set; }

        [DataMember]
        public List<AsientoDetalle> lineas { get; set; }
        #endregion

        #region Constructor
        public Asiento()
        {
            this.Memo = string.Empty;
            this.Ref1 = string.Empty;
            this.Ref2 = string.Empty;
            this.TransCode = string.Empty;
            this.Project = string.Empty;
            this.StampTax = string.Empty;
            this.AutoVat = string.Empty;

            lineas = new List<AsientoDetalle>();
        }
        #endregion
    }
}

