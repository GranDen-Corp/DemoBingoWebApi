using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GranDen.Game.ApiLib.Bingo.Models;
using GranDen.Game.ApiLib.Bingo.Options;
using GranDen.Game.ApiLib.Bingo.ServicesRegistration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoBingoWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureBingoDbService(services, Configuration, _env);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            DoPresetBingoGameDb(app, env);
        }

        private static void ConfigureBingoDbService(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.ConfigureBingoGameOption(configuration.GetSection("BingoGame"));
            services.ConfigureBingoGameTableOption(configuration.GetSection("GameTable"));
            services.AddBingoGameDbContext(builder =>
            {
                builder.UseSqlServer(configuration.GetConnectionString("DemoBingoDb"));
                if (env.IsDevelopment())
                {
                    builder.EnableSensitiveDataLogging();
                    builder.EnableDetailedErrors();
                }
            });
            services.ConfigPresetGeoPointData(new[]
            {
                "geoPoint_01", "geoPoint_02", "geoPoint_03", "geoPoint_04", "geoPoint_05", "geoPoint_06", "geoPoint_07",
                "geoPoint_08", "geoPoint_09", "geoPoint_10", "geoPoint_11", "geoPoint_12", "geoPoint_13", "geoPoint_14",
                "geoPoint_15", "geoPoint_16"
            });

            services.AddGeoPointIdProvider((bingoGameName, bingoPlayerId) =>
                new Dictionary<(int x, int y), string>
                {
                    {(0, 0), "geoPoint_01"},
                    {(0, 1), "geoPoint_02"},
                    {(0, 2), "geoPoint_03"},
                    {(0, 3), "geoPoint_04"},
                    {(1, 0), "geoPoint_05"},
                    {(1, 1), "geoPoint_06"},
                    {(1, 2), "geoPoint_07"},
                    {(1, 3), "geoPoint_08"},
                    {(2, 0), "geoPoint_09"},
                    {(2, 1), "geoPoint_10"},
                    {(2, 2), "geoPoint_11"},
                    {(2, 3), "geoPoint_12"},
                    {(3, 0), "geoPoint_13"},
                    {(3, 1), "geoPoint_14"},
                    {(3, 2), "geoPoint_15"},
                    {(3, 3), "geoPoint_16"},
                });

            var bingoGameOption = new BingoGameOption();
            configuration.GetSection("BingoGame").Bind(bingoGameOption);
            services.ConfigPresetBingoGameData(bingoGameOption);
        }

        private static void DoPresetBingoGameDb(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            var bingoGameDbContext = serviceProvider.GetService<BingoGameDbContext>();

            if (env.IsDevelopment())
            {
                if (bingoGameDbContext.Database.CanConnect())
                {
                    bingoGameDbContext.Database.Migrate();
                }
                else
                {
                    throw new Exception("Cannot connect to Bingo Game Database!");
                }
            }

            serviceProvider.InitPresetBingoGameData();
            serviceProvider.InitPresetGeoPointData();
        }
    }
}