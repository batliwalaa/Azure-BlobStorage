using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System;

namespace BlobStorageExample.Infrastructure;

public static class SwaggerExtension
{
  public static void AddSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Version = "v1",
        Title = "Rest API - Azure Blob storage",
        Description = "Api for communicating with Azure blob storage"
      });

      string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

      if (File.Exists(xmlFile))
      {
        c.IncludeXmlComments(xmlPath);
      }

      c.EnableAnnotations();
    });
  }
}
