using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendEmail.Services;

namespace SendEmail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo tempo de execução. Use este método para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Para suporte e manipulação de rotas swagger: 
            //https://medium.com/@salmanlone89/add-swagger-to-asp-net-core-2-1-web-api-f5ef0d170d4f
            //https://medium.com/@salmanlone89/customize-swagger-to-asp-net-core-2-1-web-api-8af4ff28835d
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "Prototype API for Emailing",
                    Description = "Email sending api",
                    TermsOfService = "None",
                    Contact= new Swashbuckle.AspNetCore.Swagger.Contact()
                    {
                        Name= "Matur Contábil",
                        Email= "matur@gmail.com",
                        Url= "https://www.matur.com.br/"
                    }
                });
            });
            
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
            });
            
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
        }

        // TEste método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Ative o middleware para servir o Swagger gerado como um terminal JSON.
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "SendEmailAPI V1");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
           
        }
        
        
        private string GetXmlCommentsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SendEmail.xml");
        }
        
    }
}
