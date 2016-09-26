using System;
using Swashbuckle.Application;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApplication4.Startup))]

namespace WebApplication4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            // Swagger Changes Start
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Demo API");
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.ResolveConflictingActions(x => x.First());

            }).EnableSwaggerUi();
            //Swagger Changes End 
            ConfigureAuth(app);
            WebApiConfig.Register(config);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        protected static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\WebApplication4.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
