using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Ubet.Stream.Mapping.Api.SetupActions
{
  public class SwaggerSetup
  {
    public static void SwaggerGen(SwaggerGenOptions options)
    {

      options.SwaggerDoc("swagger", new Info
      {
        Title = "Power Consumption API"
      });
      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
      options.IncludeXmlComments(xmlPath);
    }

    public static void Swagger(SwaggerOptions options)
    {
      options.RouteTemplate = "/{documentName}.json";
    }

    public static void SwaggerUi(SwaggerUIOptions options)
    {
      options.SwaggerEndpoint("/swagger.json", "Power Consumption API");
      options.RoutePrefix = "api";
    }
  }
}
