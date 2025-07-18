
using Club_System_API.Helper;
using Serilog;
using Stripe;

namespace Club_System_API
{
    public class Program


    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDependencies(builder.Configuration);
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.Configure<FrontStripe>(builder.Configuration.GetSection("FrontStripe"));

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();

            // Configure the HTTP request pipeline.
           // if (app.Environment.IsDevelopment())
           // {
                app.UseSwagger();
                app.UseSwaggerUI();
           // }

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
