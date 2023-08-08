using BlobStorageExample.Infrastructure;
using BlobStorageExample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BlobStorageExample;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddAzureStorageBlobServiceClients(Configuration);
    services.AddScoped<IBlobService, BlobService>();
    services.AddScoped<IContainerService, ContainerService>();

    services.AddControllers();

    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Version = "v1",
        Title = "Blob Store API",
        Description = "Api for communicating with Azure blob storage"
      });

      //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

      //c.IncludeXmlComments(xmlPath);
      c.EnableAnnotations();
    });
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}