
using Orkidea.ApogeoWinservice.DAL;
using Orkidea.ApogeoWinservice.Entities;
using Orkidea.ApogeoWinservice.Entities.Asientos;
using Orkidea.ApogeoWinservice.Entities.Seguridad;
using Orkidea.ApogeoWinservice.Entities.SociosNegocio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.ApogeoWinservice.Business
{
    public class BusinessExternalDB
    {
        #region Asientos contables
        public List<Asiento> GetPendingJournals()
        {
            List<Asiento> lsJournals = new List<Asiento>();

            List<SAP_CABEZA_ASIENTO> lsHeaders = GetPendingJournalHeaders("N");

            foreach (SAP_CABEZA_ASIENTO item in lsHeaders)
            {
                Asiento asiento = new Asiento()
                {
                    Memo = string.IsNullOrEmpty(item.Memo) ? "" : (item.Memo.ToString().Length > 50 ? item.Memo.ToString().Substring(0, 49) : item.Memo.ToString()),
                    Ref1 = item.Ref1 == null ? "" : item.Ref1,
                    Ref2 = item.Ref2 == null ? "" : item.Ref2,
                    TransCode = item.TransCode,
                    Project = item.Project == null ? "" : item.Project,
                    StampTax = item.StampTax == null ? "" : item.StampTax,
                    RefDate = item.RefDate == null ? DateTime.Now : (DateTime)item.RefDate,
                    TaxDate = item.TaxDate == null ? DateTime.Now : (DateTime)item.TaxDate,
                    TransId = (int)item.TransId,
                    lineas = GetPendingJournalDetails((int)item.TransId),
                };

                lsJournals.Add(asiento);
            }

            return lsJournals;
        }

        private List<SAP_CABEZA_ASIENTO> GetPendingJournalHeaders(string estado)
        {
            List<SAP_CABEZA_ASIENTO> lsSAP_CABEZA_ASIENTO = new List<SAP_CABEZA_ASIENTO>();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lsSAP_CABEZA_ASIENTO = ctx.SAP_CABEZA_ASIENTO.Where(x => x.ESTADO == estado).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lsSAP_CABEZA_ASIENTO;
        }

        private List<AsientoDetalle> GetPendingJournalDetails(int idAsiento)
        {
            List<AsientoDetalle> lsDetalle = new List<AsientoDetalle>();
            List<SAP_DETALLE_ASIENTO> lsSAP_DETALLE_ASIENTO = new List<SAP_DETALLE_ASIENTO>();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lsSAP_DETALLE_ASIENTO = ctx.SAP_DETALLE_ASIENTO.Where(x => x.TransId == idAsiento).ToList();

                    foreach (SAP_DETALLE_ASIENTO item in lsSAP_DETALLE_ASIENTO)
                    {
                        AsientoDetalle asientoDetalle = new AsientoDetalle()
                        {
                            Account = item.Account,
                            Debit = Convert.ToDouble(item.Debit),
                            Credit = Convert.ToDouble(item.Credit),
                            Ref1 = item.Ref1 == null ? "" : item.Ref1,
                            Ref2 = item.Ref2 == null ? "" : item.Ref2,
                            Ref3Line = item.Ref3Line == null ? "" : item.Ref3Line,
                            TaxDate = item.TaxDate != null && Convert.ToDateTime(item.TaxDate).Year != 1 ? Convert.ToDateTime(item.TaxDate) : DateTime.Now,
                            RefDate = item.RefDate != null && Convert.ToDateTime(item.RefDate).Year != 1 ? Convert.ToDateTime(item.RefDate) : DateTime.Now,
                            LineMemo = item.LineMemo == null ? "" : item.LineMemo,
                            Project = item.Project == null ? "" : item.Project,
                            ProfitCode = item.ProfitCode == null ? "" : item.ProfitCode,
                            OcrCode2 = item.OcrCode2 == null ? "" : item.OcrCode2,
                            OcrCode3 = item.OcrCode3 == null ? "" : item.OcrCode3,
                            U_InfoCo01 = item.U_InfoCo01 == null ? "" : item.U_InfoCo01,
                            id = item.ID_DETALLE_ASIENTO
                        };

                        lsDetalle.Add(asientoDetalle);
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return lsDetalle;
        }

        public void JournalSynchronized(Asiento asiento)
        {
            try
            {
                SAP_CABEZA_ASIENTO header = GetJournalHeader(asiento.TransId);

                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    if (header != null)
                    {
                        header.ESTADO = "P";
                        ctx.SAP_CABEZA_ASIENTO.Attach(header);
                        ctx.SaveChanges();

                        foreach (AsientoDetalle item in asiento.lineas)
                        {
                            SAP_DETALLE_ASIENTO lineaAsiento = GetJournalSpecificDetail(item.id);

                            if (lineaAsiento != null)
                            {
                                lineaAsiento.ESTADO = "P";
                                ctx.SAP_DETALLE_ASIENTO.Attach(lineaAsiento);
                                ctx.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private SAP_CABEZA_ASIENTO GetJournalHeader(int id)
        {

            SAP_CABEZA_ASIENTO oSAP_CABEZA_ASIENTO = new SAP_CABEZA_ASIENTO();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    oSAP_CABEZA_ASIENTO = ctx.SAP_CABEZA_ASIENTO.Where(x => x.ID_CABEZA_ASIENTO.Equals(id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSAP_CABEZA_ASIENTO;
        }

        private SAP_DETALLE_ASIENTO GetJournalSpecificDetail(long id)
        {
            SAP_DETALLE_ASIENTO oSAP_DETALLE_ASIENTO = new SAP_DETALLE_ASIENTO();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    oSAP_DETALLE_ASIENTO = ctx.SAP_DETALLE_ASIENTO.Where(x => x.ID_DETALLE_ASIENTO.Equals(id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSAP_DETALLE_ASIENTO;
        }
        #endregion

        #region Socios de negocio
        public List<SocioNegocio> GetPendingBusinessPartners()
        {
            List<SocioNegocio> lsBusinessPartner = new List<SocioNegocio>();
            List<SAP_SOCIOS_NEGOCIO> lsSAP_SOCIOS_NEGOCIO = new List<SAP_SOCIOS_NEGOCIO>();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lsSAP_SOCIOS_NEGOCIO = ctx.SAP_SOCIOS_NEGOCIO.Where(x => x.ESTADO == "N").ToList();
                }

                foreach (SAP_SOCIOS_NEGOCIO item in lsSAP_SOCIOS_NEGOCIO)
                {
                    SocioNegocio socioNegocio = new SocioNegocio()
                    {
                        CardCode = item.CardCode,
                        DebPayAcct = item.DebitorAccount,
                        Territory = int.Parse(item.Territory),
                        AccCritria = item.AccrualCriteria,
                        BlockDunn = item.BlockDunning,
                        CardFName = item.CardForeignName,
                        CardName = item.CardName,
                        CardType = item.CardType,
                        Cellular = item.Cellular,
                        CollecAuth = item.CollectionAuthorization,
                        CreditLine = double.Parse(item.CreditLimit),
                        Currency = item.Currency,
                        DeferrTax = item.DeferredTax,
                        E_Mail = item.EmailAddress,
                        Equ = item.Equalization,
                        Fax = item.Fax,
                        LicTradNum = item.FederalTaxID,
                        Phone1 = item.Phone1,
                        Phone2 = item.Phone2,
                        id = item.ID_SOCIO_NEGOCIO
                    };

                    lsBusinessPartner.Add(socioNegocio);
                }
            }
            catch (Exception ex) { throw ex; }

            return lsBusinessPartner;
        }

        public void PartnerSynchronized(SocioNegocio socioNegocio)
        {
            try
            {
                SAP_SOCIOS_NEGOCIO bp = GetSpecificBusinessPartner(socioNegocio.id);

                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    if (bp != null)
                    {
                        bp.ESTADO = "P";
                        ctx.SAP_SOCIOS_NEGOCIO.Attach(bp);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private SAP_SOCIOS_NEGOCIO GetSpecificBusinessPartner(long id)
        {

            SAP_SOCIOS_NEGOCIO oSAP_SOCIOS_NEGOCIO = new SAP_SOCIOS_NEGOCIO();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    oSAP_SOCIOS_NEGOCIO = ctx.SAP_SOCIOS_NEGOCIO.Where(x => x.ID_SOCIO_NEGOCIO.Equals(id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSAP_SOCIOS_NEGOCIO;
        }
        #endregion
    }


}
