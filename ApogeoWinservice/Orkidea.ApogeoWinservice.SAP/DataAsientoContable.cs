using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Orkidea.ApogeoWinservice.Entities.Asientos;
using Orkidea.ApogeoWinservice.Entities.Seguridad;
using Orkidea.ApogeoWinservice.Entities.Excepciones;

namespace Orkidea.ApogeoWinservice.SAP
{
    /// <summary>
    /// Clase para la gestion de asientos contables en SAP
    /// </summary>
    public class DataAsientoContable
    {
        #region Propiedades
        /// <summary>
        /// Objeto que proporciona actividades de conexion a Base de Datos.
        /// </summary>        
        private Database baseDatos;
        #endregion

        #region Constructor
        /// <summary>
        /// contructor de la clase
        /// </summary>
        public DataAsientoContable()
        {
            //this.conexionBD = ConexionBD.Conexion;
            this.baseDatos = DatabaseFactory.CreateDatabase("SAP");
        }
        #endregion contructor

        #region Métodos
        /// <summary>
        /// metodo que realiza la creación de asientos contables
        /// </summary>
        /// <param name="unAsiento">Objeto de tipo asiento contable</param>
        /// <returns>Numero de aiento contable</returns>
        public int CrearAsiento(Asiento unAsiento)
        {
            JournalEntries miAsientoContable;
            int miResultado = -1;

            // se crea el encabezado del asiento
            miAsientoContable = (JournalEntries)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oJournalEntries);
            miAsientoContable.TransactionCode = unAsiento.TransCode;

            if (unAsiento.Memo.ToString().Length > 50)
                miAsientoContable.Memo = unAsiento.Memo.ToString().Substring(0, 49);
            else
                miAsientoContable.Memo = unAsiento.Memo.ToString();


            miAsientoContable.ReferenceDate = unAsiento.RefDate == null ? System.DateTime.Now : unAsiento.RefDate;
            miAsientoContable.Memo = unAsiento.Memo == null ? "" : unAsiento.Memo;

            miAsientoContable.Reference = unAsiento.Ref1 == null ? "" : unAsiento.Ref1;
            miAsientoContable.Reference2 = unAsiento.Ref2 == null ? "" : unAsiento.Ref2;
            miAsientoContable.TransactionCode = unAsiento.TransCode == null ? "" : unAsiento.TransCode;
            miAsientoContable.ProjectCode = unAsiento.Project == null ? "" : unAsiento.Project;

            miAsientoContable.TaxDate = unAsiento.TaxDate == null ? System.DateTime.Now : unAsiento.TaxDate;
            miAsientoContable.VatDate = unAsiento.VatDate == null ? System.DateTime.Now : unAsiento.VatDate;

            if (!string.IsNullOrEmpty(unAsiento.StampTax))
            {
                if (unAsiento.StampTax == "Yes")
                    miAsientoContable.StampTax = BoYesNoEnum.tYES;
                else
                    miAsientoContable.StampTax = BoYesNoEnum.tNO;
            }

            if (!string.IsNullOrEmpty(unAsiento.AutoVat))
            {
                if (unAsiento.AutoVat == "Yes")
                    miAsientoContable.AutoVAT = BoYesNoEnum.tYES;
                else
                    miAsientoContable.AutoVAT = BoYesNoEnum.tNO;
            }

            // Adicion de detalle de asiento 
            foreach (AsientoDetalle linea in unAsiento.lineas)
            {
                miAsientoContable.Lines.AccountCode = linea.Account;
                miAsientoContable.Lines.Debit = linea.Debit;
                miAsientoContable.Lines.Credit = linea.Credit;
                miAsientoContable.Reference = unAsiento.Ref1;
                miAsientoContable.Lines.Reference1 = linea.Ref1 == null ? "" : linea.Ref1;
                miAsientoContable.Lines.Reference2 = linea.Ref2 == null ? "" : linea.Ref2;
                miAsientoContable.Lines.AdditionalReference = linea.Ref3Line == null ? "" : linea.Ref3Line;

                if (linea.TaxDate.Year != 1)
                    miAsientoContable.TaxDate = linea.TaxDate;

                if (linea.DuoDate != null || linea.DuoDate.Year != 1)
                    miAsientoContable.Lines.DueDate = linea.DuoDate;

                miAsientoContable.Lines.LineMemo = linea.LineMemo == null ? "" : linea.LineMemo;

                if (linea.RefDate != null || linea.RefDate.Year != 1)
                    miAsientoContable.Lines.ReferenceDate1 = linea.RefDate == null ? System.DateTime.Now : linea.RefDate;

                miAsientoContable.Lines.LineMemo = linea.LineMemo == null ? "" : linea.LineMemo;
                miAsientoContable.Lines.ProjectCode = linea.Project == null ? "" : linea.Project;

                if (linea.TaxDate != null || linea.TaxDate.Year != 1)
                    miAsientoContable.Lines.TaxDate = linea.TaxDate == null ? System.DateTime.Now : linea.TaxDate;

                miAsientoContable.Lines.CostingCode = linea.ProfitCode == null ? "" : linea.ProfitCode;
                miAsientoContable.Lines.CostingCode2 = linea.OcrCode2 == null ? "" : linea.OcrCode2;
                miAsientoContable.Lines.CostingCode3 = linea.OcrCode3 == null ? "" : linea.OcrCode3;
                
                //UDF Values
                miAsientoContable.Lines.UserFields.Fields.Item("U_InfoCo01").Value = linea.U_InfoCo01 == null ? "" : linea.U_InfoCo01;

                miAsientoContable.Lines.Add();
            }
            //Creacion del asiento contable
            miResultado = miAsientoContable.Add();
            if (miResultado != 0)
                throw new SAPException(miResultado, ConexionSAP.Conexion.compania.GetLastErrorDescription());
            else
                return Convert.ToInt32(ConexionSAP.Conexion.compania.GetNewObjectKey());
        }
        #endregion
    }
}

