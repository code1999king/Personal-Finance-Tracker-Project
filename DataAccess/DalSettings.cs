
using System.Configuration;

namespace DataAccess
{
    /// <summary>
    /// Contains Data Access layer settings.
    /// </summary>
    internal class DalSettings
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PFT_DB"].ConnectionString;
            }
        }
    }
}
