using Infrastructure;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Serialization;
using Persistence.DB;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddPharmacyApp(builder.Configuration, false);
            builder.Services.AddCors();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();                    

            //builder.Services.AddControllers();
            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwagger(options =>
                //{
                //    options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
                //});
                app.UseCors(options =>
                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            }
            else
            {
                app.UseCors();
            }

            app.UseStatusCodePages();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
