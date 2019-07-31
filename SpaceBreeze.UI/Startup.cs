using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JDPDO.SpaceBreeze.UI
{
    public class Startup
    {
        // Adding localisation field
        // readonly IStringLocalizer localizer;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Overview}/{action=Server}/{id?}");
            });

            BootstrapElectron();
        }

        public async void BootstrapElectron()
        {
            var options = new BrowserWindowOptions
            {
                Frame = true,
                Show = false,
                AutoHideMenuBar = false,
            };

            // Open Electron window
            var mainWindow = await Electron.WindowManager.CreateWindowAsync(options);
            mainWindow.OnReadyToShow += () => mainWindow.Show();

            // Define menu of window
            var menu = new MenuItem[]
            {
                new MenuItem
                {
                    Label = "File",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "Exit",
                            Click = () => { Electron.App.Exit(); }
                        }
                    }
                }
            };
            Electron.Menu.SetApplicationMenu(menu);
        }
    }
}
