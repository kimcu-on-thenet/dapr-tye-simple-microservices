﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleStore.Infrastructure.Common.Extensions;
using SimpleStore.Infrastructure.EfCore;
using SimpleStore.ProductCatalog.Infrastructure.EfCore.Gateways;
using SimpleStore.ProductCatalog.Infrastructure.EfCore.Options;
using SimpleStore.ProductCatalog.Infrastructure.EfCore.Persistence;
using SimpleStore.ProductCatalog.Infrastructure.EfCore.PubSub;
using System;
using System.Reflection;


namespace SimpleStore.ProductCatalog.Infrastructure.EfCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<ServiceOptions>(configuration.GetSection("Services"))
                .Configure<DaprOptions>(configuration.GetSection("Dapr"))
                .AddEfCore<ProductCatalogDbContext>(configuration, Assembly.GetExecutingAssembly())
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddCustomRequestValidation()
                .AddDomainEventDispatcher();

            services.AddHttpClient<InventoriesGateway>((provider, client) =>
            {
                var serviceOptions = provider.GetRequiredService<IOptions<ServiceOptions>>();
                client.BaseAddress = new Uri(serviceOptions.Value.InventoriesApi.RestUri, UriKind.Absolute);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            
            services.AddHttpClient<DaprPublisher>((provider, client) =>
            {
                var baseAddress = $"http://localhost:{Environment.GetEnvironmentVariable("DAPR_HTTP_PORT")}";

                client.BaseAddress = new Uri(baseAddress);
            });

            return services;
        }
    }
}
