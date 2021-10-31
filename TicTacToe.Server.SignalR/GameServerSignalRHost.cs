using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.Server.SignalR
{
    public class GameServerSignalRHost : IDisposable
    {
        private readonly GameServer _server;
        private readonly string _url;

        private IWebHost _host;

        public GameServerSignalRHost(GameServer server, string url)
        {
            _server = server;
            _url = url;
        }

        public static GameServer GameServer { get; set; }

        public IHubContext<GameServerHub> Run()
        {
            GameServer = _server;

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(_url)   // "http://localhost:5003"
                .UseStartup<Startup>()
                .Build();

            Task.Run(async () => await _host.RunAsync());

            return _host.Services.GetService<IHubContext<GameServerHub>>();
        }

        public void Dispose()
        {
            _host.StopAsync().Wait();
            _host.Dispose();
        }

        private class Startup
        {
            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddSignalR();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<GameServerHub>("/GameServerHub");
                });
            }
        }
    }
}
