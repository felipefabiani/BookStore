
namespace BookStore.Helper
{
    public partial class BookStoreConstants
    {
        public static readonly string BookStoreOptions = "ArticleOptions";
        public static readonly string ConnectionString = "BOOKTORE_CONNECTIONSTRINGS";
        public static readonly string LogFilePath = "log/log-.txt";
        public static readonly int CachingTime = 20 * 60;
    }

    public partial class BookStoreConstants
    {
        public class Environment
        {
            public static readonly string Dev = "Development";
            public static readonly string Uat = "UAT";
            public static readonly string UatAutomatedTest = "UAT-AutomatedTest";
            public static readonly string Production = "Production";
        }
    }
    public partial class BookStoreConstants
    {
        public class Cors
        {
            public static readonly string BookStoreClient = "Article.Client";
            public static readonly string AllowedHostsSection = "AllowedHosts";
        }
        public class Security
        {
            public static readonly string Issuer = "Article.Issuer";
            public static readonly string Audience = "Article.Audience";
        }
    }
}