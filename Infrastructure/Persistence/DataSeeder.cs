using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DataSeeder
    {

        private readonly AppDbContext _context;
        private bool _seedInDb = false;
        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task Initialize()
        {
            await SeedCategories();

        }


        private async Task SeedCategories()
        {
            var categoriesDb = await _context.Categorie.FirstOrDefaultAsync();

            if (categoriesDb is null)
            {
                var categoriesMock = new List<CategorieDbModel>()
                {
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Lanche",true),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Acompanhamento",false),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Bebida",false),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Sobremesa",false)
                };

                await _context.Categorie.AddRangeAsync(categoriesMock);
                _seedInDb = true;
            }
        }


    }
}
