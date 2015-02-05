using IntegracionApogeo.Business.Entities;
using IntegracionApogeo.data;
using IntegracionApogeo.DataAccessEF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionApogeo.Business
{
    public class BusinessExternalDB
    {
        public List<SAP_CABEZA_ASIENTO> GetSAP_CABEZA_ASIENTOList()
        {

            List<SAP_CABEZA_ASIENTO> lsSAP_CABEZA_ASIENTO = new List<SAP_CABEZA_ASIENTO>();

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lsSAP_CABEZA_ASIENTO = ctx.SAP_CABEZA_ASIENTO.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lsSAP_CABEZA_ASIENTO;
        }

        public List<SAP_CABEZA_ASIENTO> GetSAP_CABEZA_ASIENTOList(string estado)
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

        public SAP_CABEZA_ASIENTO GetSAP_CABEZA_ASIENTOById(long id)
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

        public void SaveSAP_CABEZA_ASIENTO(SAP_CABEZA_ASIENTO SAP_CABEZA_ASIENTOTarget)
        {

            try
            {
                using (var ctx = new SAP_TEMP_APOGEOEntities())
                {
                    //verify if the Sport exists
                    SAP_CABEZA_ASIENTO _SAP_CABEZA_ASIENTO = GetSAP_CABEZA_ASIENTOById(SAP_CABEZA_ASIENTOTarget.ID_CABEZA_ASIENTO);

                    if (_SAP_CABEZA_ASIENTO != null)
                    {
                        // if exists then edit 
                        ctx.SAP_CABEZA_ASIENTO.Attach(_SAP_CABEZA_ASIENTO);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(_SAP_CABEZA_ASIENTO, SAP_CABEZA_ASIENTOTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.SAP_CABEZA_ASIENTO.Add(SAP_CABEZA_ASIENTOTarget);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (DbEntityValidationException e)
            {
                StringBuilder oError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    oError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        oError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                string msg = oError.ToString();
                throw new Exception(msg);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
