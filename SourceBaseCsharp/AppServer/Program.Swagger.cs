using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AppServer
{
    public partial class Program
    {
        private static void AddSwagger(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            var nameSwagger = string.IsNullOrEmpty(config["MySwagger:Name"]) ? "name" : config["MySwagger:Name"];
            var titleApiInfoSwagger = string.IsNullOrEmpty(config["MySwagger:ApiInfo:Title"]) ? "title" : config["MySwagger:ApiInfo:Title"];
            var versionApiInfoSwagger = string.IsNullOrEmpty(config["MySwagger:ApiInfo:Version"]) ? "version" : config["MySwagger:ApiInfo:Version"];
            var nameSecuritySwagger = string.IsNullOrEmpty(config["MySwagger:Security:Name"]) ? "name" : config["MySwagger:Security:Name"];
            var descriptionSecuritySwagger = string.IsNullOrEmpty(config["MySwagger:Security:Description"]) ? "description" : config["MySwagger:Security:Description"];

            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                c.CustomSchemaIds(type => type.FullName); // Dùng tên đầy đủ (namespace + tên lớp) làm schemaId

                c.SwaggerDoc(nameSwagger, new OpenApiInfo
                {
                    Title = titleApiInfoSwagger,
                    Version = versionApiInfoSwagger
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = nameSecuritySwagger,
                    Description = descriptionSecuritySwagger,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Chỉ bao gồm các controller có [ApiController]
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var hasApiControllerAttribute = apiDesc.ActionDescriptor.EndpointMetadata
                        .Any(em => em is ApiControllerAttribute);
                    return hasApiControllerAttribute;
                });
            });
        }

        private static void UseSwagger(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DefaultModelsExpandDepth(-1);
                });
            }
        }
    }
}
