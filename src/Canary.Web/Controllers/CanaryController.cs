using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Canary.Web.Controllers
{
    public abstract class CanaryController : Controller
    {
        protected SqlConnection CreateDbConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["dev"].ConnectionString);
        }
    }
}