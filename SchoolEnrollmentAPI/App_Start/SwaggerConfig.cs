using System.Web.Http;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SchoolEnrollmentAPI.App_Start.SwaggerConfig), "Register")]

namespace SchoolEnrollmentAPI.App_Start
{
    public static class SwaggerConfig
    {
        public static void Register()
        {
            // Disponibiliza a documentação interativa sem misturar essa configuração aos controllers.
            GlobalConfiguration.Configuration
                .EnableSwagger(configuration => configuration
                    .SingleApiVersion("v1", "School Enrollment API")
                    .Description("API desenvolvida como teste prático para controle de matrículas escolares. " +
                                 "Nesta primeira etapa, estão disponíveis os endpoints do CRUD de alunos."))
                .EnableSwaggerUi();
        }
    }
}
