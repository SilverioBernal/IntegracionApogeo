using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IntegracionApogeo.Business.Entities.SociosNegocio;

namespace IntegracionApogeo.Business.Entities.Asientos
{
    /// <summary>
    /// Entidad para la gestion de detalle de asientos contables
    /// </summary>
    [DataContract(Namespace = "http://IntegracionApogeo")]
    public class AsientoDetalle
    {
        #region Atributos
        [DataMember]
        public string Account { get; set; }
        [DataMember]
        public double Debit { get; set; }
        [DataMember]
        public double Credit { get; set; }
        [DataMember]
        public DateTime DuoDate { get; set; }
        [DataMember]
        public string LineMemo { get; set; }
        [DataMember]
        public DateTime RefDate { get; set; }
        [DataMember]
        public string Ref1 { get; set; }
        [DataMember]
        public string Ref2 { get; set; }
        [DataMember]
        public string Ref3Line { get; set; }
        [DataMember]
        public string Project { get; set; }
        [DataMember]
        public DateTime TaxDate { get; set; }
        [DataMember]
        public string ProfitCode { get; set; }
        [DataMember]
        public string OcrCode2 { get; set; }
        [DataMember]
        public string OcrCode3 { get; set; }
        [DataMember]
        public string U_InfoCo01 { get; set; }
        [DataMember]
        public SocioNegocio socioNegocio { get; set; }
        #endregion

        #region Constructor
        public AsientoDetalle()
        {
            this.Account = string.Empty;
            this.LineMemo = string.Empty;

            this.Ref1 = string.Empty;
            this.Ref2 = string.Empty;
            this.Ref3Line = string.Empty;
            this.Project = string.Empty;
            this.ProfitCode = string.Empty;
            this.OcrCode2 = string.Empty;
            this.OcrCode3 = string.Empty;
            this.U_InfoCo01 = string.Empty;
            socioNegocio = new SocioNegocio();
        }
        #endregion
    }
}
