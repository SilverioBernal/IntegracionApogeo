using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SAPbobsCOM;
using System.Configuration;


namespace Orkidea.ApogeoWinservice.Entities.Seguridad
{
    /// <summary>
    /// clase que se usa para manejar la información de la conexión
    /// </summary>
    public class ConexionSAP
    {
        #region Atributos
        /// <summary>
        /// Permite implementar el patron de diseño Singleton con el fin de tener solo una instancia.
        /// </summary>
        private static ConexionSAP conexion;
        /// <summary>
        /// Atributo de conexión a la base de datos
        /// </summary>
        public Company compania;
        #endregion
        static readonly object padlock = new object();
        /// <summary>
        /// Constructor Privado para implementar el patron Singleton
        /// </summary>
        private ConexionSAP(string dataBase, string licenceServer, string DatabaseServer, string user, string password, string userBd, string passwordBD, string serverType)
        {
            compania = new Company();
            compania.CompanyDB = dataBase;
            compania.UserName = user;
            compania.Server = DatabaseServer;
            compania.LicenseServer = licenceServer;
            compania.Password = password;
            compania.DbPassword = passwordBD;
            compania.DbUserName = userBd;
            compania.language = BoSuppLangs.ln_Spanish_La;
            switch (serverType)
            {
                case "MSSQL2005":
                    compania.DbServerType = BoDataServerTypes.dst_MSSQL2005;
                    break;
                case "MSSQL2008":
                    compania.DbServerType = BoDataServerTypes.dst_MSSQL2008;
                    break;
                default:
                    compania.DbServerType = BoDataServerTypes.dst_MSSQL2012;
                    break;
            }

        }
        /// <summary>
        /// Devuelve solo una instanacia del objeto Company
        /// </summary>
        public static ConexionSAP Conexion
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
                        conexion = new ConexionSAP(ConfigurationManager.AppSettings["DataBase"],
                            ConfigurationManager.AppSettings["LicenceServer"], ConfigurationManager.AppSettings["DataBaseServer"],
                            ConfigurationManager.AppSettings["UserSAP"], ConfigurationManager.AppSettings["PasswordSAP"],
                            ConfigurationManager.AppSettings["UserBD"], ConfigurationManager.AppSettings["PasswordBD"],
                            ConfigurationManager.AppSettings["ServerType"]);
                    }
                    return conexion;
                }
            }
        }
        #region propiedades
        /// <summary>
        ///// Usuario para la conexión al servicio Web
        ///// </summary>
        //public string Usuario
        //{

        //    set;
        //    get;
        //}
        ///// <summary>
        ///// Contraseña para el usuario del SQL Server.
        ///// </summary>
        //public string Clave
        //{

        //    set;
        //    get;
        //}

        ///// <summary>
        ///// Nombre de la base de datos de SAP.
        ///// </summary>
        //public string DatabaseName
        //{
        //    get ; 
        //    set ;
        //}
        ///// <summary>
        ///// IP del Servidor de SQL Server.
        ///// </summary>
        //public string DatabaseServer
        //{
        //    set;
        //    get;
        //}

        ///// <summary>
        ///// Tipo de la base de datos de SAP.
        ///// </summary>
        //public string DatabaseType
        //{

        //    set;
        //    get;
        //}
        ///// <summary>
        ///// Usuario con permisos sobre la base de datos de SAP.
        ///// </summary>
        //public string DatabaseUserName
        //{
        //    set;
        //    get;
        //}
        ///// <summary>
        ///// Contraseña para el usuario del SQL Server.
        ///// </summary>
        //public string DatabasePassword
        //{
        //    set;
        //    get;
        //}
        ///// <summary>
        ///// servidor que tiene las licencias de SAP, se especifica direccion ip y puerto
        ///// </summary>
        //public string ServidorLicencia
        //{
        //    set;
        //    get;
        //}
        #endregion
    }
}
