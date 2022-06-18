using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace ShopBridge.API.ExceptionMiddleware
{
    public static class ExceptionFilter
    {
        public static void UseProductExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appHandler =>
            {
                appHandler.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    await context.Response.WriteAsync("Exception occured while processing given request.");

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    var exception = exceptionHandlerPathFeature?.Error;

                    if (exception is DbUpdateConcurrencyException)
                    {
                        await context.Response.WriteAsync("Product you were trying to update was updated in another session. Please update you product details");
                    }

                    if (exception is DbUpdateException)
                    {
                        await context.Response.WriteAsync("Unable to Update Given Product. Please try again later");
                    }

                    if (exception is Exception)
                    {
                        await context.Response.WriteAsync("Error occured while processing your request. Try again later");
                    }
                });
            });
        }
    }
}
