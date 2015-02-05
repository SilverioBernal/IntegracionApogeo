using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;

namespace IntegracionApogeo.Business.Entities.Seguridad
{
    public class ConexionBD
    {
        static ConexionBD conexion;
        /// <summary>
        /// Atributo de conexión a la base de datos
        /// </summary>
        public Database baseDatos;
        static readonly object padlock = new object();

        private ConexionBD()
        {
            try
            {
                baseDatos = new SqlDatabase(@"Database=" + ConexionSAP.Conexion.compania.CompanyDB + ";Server=" + ConexionSAP.Conexion.compania.Server + ";user Id=" + ConfigurationManager.AppSettings["UserBD"] + ";Password=" + ConfigurationManager.AppSettings["PasswordBD"] + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ConexionBD Conexion
        {
            set
            {
                conexion = value;
            }
            get
            {
                lock (padlock)
                {
                    if (conexion == null)
                    {
                        if (!ConexionSAP.Conexion.compania.CompanyDB.ToLower().Equals("database"))
                            conexion = new ConexionBD();
                    }
                    return conexion;
                }
            }
        }
    }
}
