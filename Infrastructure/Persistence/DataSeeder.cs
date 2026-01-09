using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class DataSeeder
    {
        private readonly IMongoCollection<CategorieDbModel> _categories;

        public DataSeeder(AppDbContext context)
        {
            _categories = context.Categories;
        }

        public async Task Initialize()
        {
            await SeedCategories();
        }

        private async Task SeedCategories()
        {
            var existingCategorie = await _categories
                .Find(_ => true)
                .Limit(1)
                .FirstOrDefaultAsync();

            if (existingCategorie is null)
            {
                var categoriesMock = new List<CategorieDbModel>()
                {
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Lanche", true),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Acompanhamento", false),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Bebida", false),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Sobremesa", false)
                };

                await _categories.InsertManyAsync(categoriesMock);
            }
        }
    }
}