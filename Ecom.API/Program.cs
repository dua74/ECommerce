using Ecom.API.MiddleWare;
using Ecom.infrastructure;
using Microsoft.Extensions.FileProviders;
namespace Ecom.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
                              
                               
                    });
            });
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            object value = builder.Services.infrastuctureConfiguration(builder.Configuration);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var app = builder.Build();
            //app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));
          

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CORSPolicy");
            app.UseMiddleware<ExceptionMiddleWare>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                 FileProvider = new PhysicalFileProvider(
                 Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Images")),
                 RequestPath = "/Images"
            });



            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();



            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
