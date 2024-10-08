using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using System.Net;
using System.Net.Mime;

namespace ETicaretAPI.API.Extensions
{
    static public class ConfigureExceptionHandlerExtension
    {
        public static async void ConfigureExceptionHandler<T>(this WebApplication app,ILogger<T> logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async (context) =>
                {
                    context.Response.StatusCode =  (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error.Message);
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Title = "An error is occured"
                        }));
                    }
                });
            });
        }
    }
}
