
using Bogus;
using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Data.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;



namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var serviceprovider = scope.ServiceProvider;
                var db = serviceprovider.GetRequiredService<TournamentApiContext>();

                await db.Database.MigrateAsync();


                if (await db.TournamentDetails.AnyAsync()) return;

                try
                {
                    var tournamentDetails = GenerateTournamentDetails(4);
                    db.AddRange(tournamentDetails);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

       


        private static IEnumerable<TournamentDetails> GenerateTournamentDetails(int nrOfTournaments)
        {
            var faker = new Faker<TournamentDetails>("sv").Rules((f, t) =>
            {
                t.Title = f.Music.Genre() + "Championship";
                t.StartDate = f.Date.Future();
                t.Games= GenerateGames(f.Random.Int(min: 2, max: 5));
            });

            return faker.Generate(nrOfTournaments);
        }
        private static ICollection<Game> GenerateGames(int nrOfGames)
        {
            var faker = new Faker<Game>("sv").Rules((f, g) =>
            {
                g.Title = f.Commerce.ProductName();
                g.Time = f.Date.Future();
                
               
            });

            return faker.Generate(nrOfGames);
        }
       

       
    }
}
