using Charger.Common;
using Charger.Configuration;
using Charger.Domain.Models;
using Charger.Enums;
using Charger.FrontEnd;
using Charger.FrontEnd.Connection;
using Charger.Hardware;
using Charger.Hardware.Connection;
using Charger.Interfaces;
using Charger.Logic;
using Charger.Models;
using Charger.MQTT;
using Charger.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using NLog.Extensions.Logging;
using System.Collections.Generic;

namespace Charger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            _configuration = new ConfigurationBuilder().AddXmlFile("config/ChargerConfig.xml", optional: true, reloadOnChange: true).Build();
            services.Configure<GpioConfig>(_configuration.GetSection("GpioConfig"));
            services.Configure<WagoConfig>(_configuration.GetSection("WagoConfig"));
            services.Configure<ChargingConfig>(_configuration.GetSection("LoadingConfig"));
            services.Configure<MqttConfig>(_configuration.GetSection("MqttConfig"));

            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IConversionHelper, ConversionHelper>();
            services.AddSingleton<IGuidHelper, GuidHelper>();

            services.AddSingleton<IHardwareManager, HardwareManager>();
            services.AddSingleton<IModbusProvider, ModbusProvider>();
            services.AddSingleton<IGpioProvider, GpioProvider>();

            services.AddSingleton<IChargerLogic, ChargerLogic>();
            services.AddSingleton<IBatteryService, BatteryService>();
            services.AddSingleton<IManagementService, ManagementService>();
            services.AddSingleton<IMqttConnection, MqttConnection>();

            services.AddControllers();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            }));
            services.AddLogging(o => o
                .AddDebug()
                .AddConsole()
                .AddNLog()
                );

            ConfigureCharger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureCharger(IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            var wagoConfig = provider.GetService<IOptions<WagoConfig>>().Value;
            var gpioConfig = provider.GetService<IOptions<GpioConfig>>().Value;
            var chargingConfig = provider.GetService<IOptions<ChargingConfig>>().Value;
            var mqttConfig = provider.GetService<IOptions<MqttConfig>>().Value;

            var hardwareManager = provider.GetService<IHardwareManager>();
            hardwareManager.InitHardwareConnection(wagoConfig.IpAddress, wagoConfig.Port, gpioConfig.InitGpio);

            var batteryService = provider.GetService<IBatteryService>();
            batteryService.ChargingConfig = chargingConfig;
            var managementService = provider.GetService<IManagementService>();

            batteryService.Batteries = new Dictionary<BatteryType, BatteryData>
            {
                { BatteryType.BatteryOne, wagoConfig.BatteryOne },
                { BatteryType.BatteryTwo, wagoConfig.BatteryTwo },
                { BatteryType.BatteryThree, wagoConfig.BatteryThree },
                { BatteryType.BatteryFour, wagoConfig.BatteryFour }
            };

            managementService.Controls = new Dictionary<ControlType, ControlData>
            {
                { ControlType.DoorSensor, gpioConfig.DoorSensor },
                { ControlType.LedBlue, gpioConfig.LedBlue },
                { ControlType.LedGreen, gpioConfig.LedGreen },
                { ControlType.LedYellow, gpioConfig.LedYellow }
            };

            var managementTopicList = new List<MqttTopicFilterBuilder>
            {
                new MqttTopicFilterBuilder().WithTopic(mqttConfig.TopicChargingState + "/set"),
                new MqttTopicFilterBuilder().WithTopic(mqttConfig.TopicChargingLimit + "/set"),
                new MqttTopicFilterBuilder().WithTopic(mqttConfig.TopicChargingTime + "/set")
            };

            var logic = provider.GetService<IChargerLogic>();
            var mqttConnection = provider.GetService<IMqttConnection>();
            mqttConnection.InitMqttConnection(mqttConfig.MqttBrokerUsername, mqttConfig.MqttBrokerPassword, mqttConfig.MqttBrokerAddress);

            var batteryConnection = new BatteryConnection(logic, provider.GetService<IMqttConnection>(), mqttConfig);
            var managementConnection = new ManagementConnection(logic, provider.GetService<IMqttConnection>(), mqttConfig);
            var chargerPresenter = new ChargerPresenter(logic, provider.GetService<IManagementService>());

            batteryConnection.PublishConfigurationMessage().Wait();
            managementConnection.PublishConfigurationMessage().Wait();
            batteryConnection.Init();
            managementConnection.Init();
            chargerPresenter.Init();
            mqttConnection.SubscribeMultipleTopics(managementTopicList);
            logic.StartProcess();
        }
    }
}
