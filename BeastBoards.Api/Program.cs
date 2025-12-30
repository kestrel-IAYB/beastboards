using BeastBoards.Api.Models;
using BeastBoards.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BeastBoards.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<BeastBoardsConfig>(builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("beastboards");

            builder.Services.AddDbContext<BeastBoardsContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IOptions<BeastBoardsConfig>>().Value);
            builder.Services.AddScoped<BeastBoardsJwtService>();
            builder.Services.AddScoped<BeastBoardsLeaderboardService>();
            builder.Services.AddScoped<SteamWebApiService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                Console.WriteLine(scope.ServiceProvider.GetService<BeastBoardsConfig>().IAmYourBeastAppId);
                scope.ServiceProvider.GetService<BeastBoardsContext>().Database.Migrate();
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
