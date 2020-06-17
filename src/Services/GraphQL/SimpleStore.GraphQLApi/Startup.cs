using HotChocolate;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleStore.GraphQLApi.Options;
using SimpleStore.Infrastructure.Common.Extensions;
using SimpleStore.Infrastructure.Common.GraphQL;
using SimpleStore.Infrastructure.Common.Options;
using SimpleStore.Infrastructure.Common.Tye;
using System;
using Serilog;
using SimpleStore.Infrastructure.Common.Tracing;

namespace SimpleStore.GraphQLApi
{
    public class Startup
    {
        private readonly ServiceOptions _serviceOptions;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this._serviceOptions = this.Configuration.GetOptions<ServiceOptions>("Services");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // C# 8.0, this is local function
            Uri GetGraphQLUri(ServiceConfig service)
            {
                var graphqlUri = new Uri("graphql", UriKind.Relative);
                var serviceUri = this.Configuration.GetCustomServiceUri(service);

                return new Uri(serviceUri, graphqlUri);
            }
            // End of local function

            services.AddHttpClient(nameof(ServiceOptions.ProductCatalogApi), (sp, client) =>
            {
                client.BaseAddress = GetGraphQLUri(this._serviceOptions.ProductCatalogApi);
            }); 
            services.AddHttpClient(nameof(ServiceOptions.InventoriesApi), (sp, client) =>
            {
                client.BaseAddress = GetGraphQLUri(this._serviceOptions.InventoriesApi);
            });

            services
                .AddGraphQLSubscriptions()
                .AddStitchedSchema(stitchingBuilder =>
                    stitchingBuilder
                        .AddSchemaFromHttp(nameof(ServiceOptions.ProductCatalogApi))
                        .AddSchemaFromHttp(nameof(ServiceOptions.InventoriesApi)));

            services.AddCustomOpenTelemetry(this.Configuration, this._serviceOptions.GraphQLApi);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.Listen(this.Configuration, this._serviceOptions.GraphQLApi);
            }
            //app.UseSerilogRequestLogging();
            app.UseCustomGraphQL();
        }
    }
}
