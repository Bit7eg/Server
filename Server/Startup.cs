using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Resources.NetStandard;
using System.Resources.Extensions;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        public Startup()
        {
            LoadCatalogs();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 80;
            });
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMiddleware<TokenMiddleware>();
        }
        static void LoadCatalogs()
        {
            try
            {
                using (ResXResourceReader reader = new ResXResourceReader(@".\data.resx"))
                {
                    IDictionaryEnumerator dictionary = reader.GetEnumerator();
                    while (dictionary.MoveNext())
                    {
                        if (((string)dictionary.Key) == "main")
                        {
                            TokenMiddleware.main = (Catalog)dictionary.Value;
                            TokenMiddleware.current = TokenMiddleware.main;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                using (ResXResourceWriter writer = new ResXResourceWriter(@".\data.resx"))
                {
                    writer.AddResource("main", TokenMiddleware.main);
                }
            }
        }
    }
}
