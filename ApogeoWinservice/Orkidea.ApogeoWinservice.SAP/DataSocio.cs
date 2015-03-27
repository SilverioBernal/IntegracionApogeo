using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

using System.Data.Common;
using SAPbobsCOM;
using Orkidea.ApogeoWinservice.Entities.SociosNegocio;
using Orkidea.ApogeoWinservice.Entities.Seguridad;
using Orkidea.ApogeoWinservice.Entities.Excepciones;


namespace Orkidea.ApogeoWinservice.SAP
{
    /// <summary>
    /// Clase para la gestión de socios de negocios en SAP
    /// </summary>
    public class DataSocio
    {
        #region Atributos
        /// <summary>
        /// Atributos de conexión a la base de datos
        /// </summary>
        private Database baseDatos;
        /// <summary>
        /// Lector
        /// </summary>
        private IDataReader reader;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DataSocio()
        {
            this.baseDatos = DatabaseFactory.CreateDatabase("SAP");
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Consulta un socio de negocios en SAP Business One
        /// </summary>
        /// <param name="codigo">Codigo de socio de negocio</param>
        /// <returns>Socio con la información</returns>
        public SocioNegocio ConsultarSocio(string codigo)
        {
            StringBuilder miSentencia = new StringBuilder("SELECT  CardCode,CardName, LicTradNum,Phone1, Phone2,Cellular, Fax,E_Mail  ");
            miSentencia.Append("FROM OCRD T0 ");
            miSentencia.Append("WHERE T0.LicTradNum = @CardCode ");
            miSentencia.Append("AND CardType= 'C' ");
            DbCommand miComando = this.baseDatos.GetSqlStringCommand(miSentencia.ToString());
            this.baseDatos.AddInParameter(miComando, "CardCode", DbType.String, codigo);
            SocioNegocio socio = new SocioNegocio();
            using (this.reader = this.baseDatos.ExecuteReader(miComando))
            {
                while (this.reader.Read())
                {
                    socio = new SocioNegocio();
                    socio.CardCode = this.reader.IsDBNull(0) ? "" : this.reader.GetValue(0).ToString();
                    socio.CardName = this.reader.IsDBNull(1) ? "" : this.reader.GetValue(1).ToString();
                    socio.LicTradNum = this.reader.IsDBNull(2) ? "" : this.reader.GetValue(2).ToString();
                    socio.Phone1 = this.reader.IsDBNull(3) ? "" : this.reader.GetValue(3).ToString();
                    socio.Phone2 = this.reader.IsDBNull(4) ? "" : this.reader.GetValue(4).ToString();
                    socio.Cellular = this.reader.IsDBNull(5) ? "" : this.reader.GetValue(5).ToString();
                    socio.Fax = this.reader.IsDBNull(6) ? "" : this.reader.GetValue(6).ToString();
                    socio.E_Mail = this.reader.IsDBNull(7) ? "" : this.reader.GetValue(7).ToString();


                }
            }
            return socio;
        }

        /// <summary>
        /// Método para la creacion de socios de negocio en SAP
        /// </summary>
        /// <param name="socio"></param>
        public void CrearSocio(SocioNegocio socio)
        {
            BusinessPartners bp; //= new BusinessPartners();

            bp = (BusinessPartners)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            bp.CardCode = socio.CardCode;
            bp.CardType = BoCardTypes.cCustomer;
            bp.FederalTaxID = socio.LicTradNum;
            bp.CardName = socio.CardName;

            if (!string.IsNullOrEmpty(socio.Address))
                bp.Address = socio.Address;

            if (!string.IsNullOrEmpty(socio.DebPayAcct))
                bp.DebitorAccount = socio.DebPayAcct;

            if (socio.Territory != null)
                bp.Territory = (int)socio.Territory;


            if (socio.AccCritria != null)
            {
                bp.AccrualCriteria = BoYesNoEnum.tNO;

                if (socio.AccCritria == "Yes")
                    bp.AccrualCriteria = BoYesNoEnum.tYES;
            }


            if (!string.IsNullOrEmpty(socio.BlockDunn))
            {
                bp.BlockDunning = BoYesNoEnum.tNO;

                if (socio.BlockDunn == "Yes")
                    bp.BlockDunning = BoYesNoEnum.tYES;
            }

            if (!string.IsNullOrEmpty(socio.CardName))
                bp.CardName = socio.CardName;

            if (!string.IsNullOrEmpty(socio.CardFName))
                bp.CardForeignName = socio.CardFName;

            if (!string.IsNullOrEmpty(socio.Cellular))
                bp.Cellular = socio.Cellular;

            if (!string.IsNullOrEmpty(socio.CollecAuth))
            {
                bp.CollectionAuthorization = BoYesNoEnum.tNO;

                if (socio.CollecAuth == "Yes")
                    bp.CollectionAuthorization = BoYesNoEnum.tYES;
            }

            if (socio.CreditLine != null)
                bp.CreditLimit = socio.CreditLine;

            if (!string.IsNullOrEmpty(socio.Currency))
                bp.Currency = socio.Currency;


            if (!string.IsNullOrEmpty(socio.DeferrTax))
            {
                bp.DeferredTax = BoYesNoEnum.tNO;

                if (socio.DeferrTax == "Yes")
                    bp.DeferredTax = BoYesNoEnum.tYES;
            }

            if (!string.IsNullOrEmpty(socio.E_Mail))
                bp.EmailAddress = socio.E_Mail;

            if (!string.IsNullOrEmpty(socio.Equ))
            {
                bp.Equalization = BoYesNoEnum.tNO;

                if (socio.Equ == "Yes")
                    bp.Equalization = BoYesNoEnum.tYES;
            }

            if (!string.IsNullOrEmpty(socio.Fax))
                bp.Fax = socio.Fax;

            if (!string.IsNullOrEmpty(socio.LicTradNum))
                bp.FederalTaxID = socio.LicTradNum;


            if (!string.IsNullOrEmpty(socio.Phone1))
                bp.Phone1 = socio.Phone1;


            if (!string.IsNullOrEmpty(socio.Phone2))
                bp.Phone2 = socio.Phone2;


            if (bp.Add() != 0)
                throw new SAPException(ConexionSAP.Conexion.compania.GetLastErrorCode(), ConexionSAP.Conexion.compania.GetLastErrorDescription());

        }
        #endregion
    }
}
