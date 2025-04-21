
using Club_System_API.Services;

namespace Club_System_API
{
    public class Program


    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDependencies(builder.Configuration);        
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.UseExceptionHandler();


            app.Run();
        }
    }
}
