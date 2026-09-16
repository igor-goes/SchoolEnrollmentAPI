using System.Web.Http;

namespace SchoolEnrollmentAPI
{
    /// <summary>
    /// Representa o ciclo de vida da aplicação hospedada pelo IIS Express ou IIS.
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(App_Start.WebApiConfig.Register);
        }
    }
}
