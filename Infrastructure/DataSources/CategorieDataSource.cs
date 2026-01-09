using Application.Interfaces.DataSources;
using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shared.DTO.Categorie.Input;
using System.Linq;


namespace Infrastructure.DataSources
{
    public class CategorieDataSource : ICategorieDataSource
    {
        private readonly IMongoCollection<CategorieDbModel> _categories;

        public CategorieDataSource(AppDbContext mongoDbContext)
        {
            _categories = mongoDbContext.Categories;
        }

        public async Task CreateCategorie(CategorieInputDto categorie)
        {
            var categorieDbModel = new CategorieDbModel(
                categorie.Id,
                categorie.Name,
                categorie.IsEditavel,
                categorie.CreatedAt
            );

            await _categories.InsertOneAsync(categorieDbModel);
        }

        public async Task UpdateCategorie(CategorieInputDto categorie)
        {
            var categorieDb = await _categories
                .Find(x => x.Id == categorie.Id)
                .FirstOrDefaultAsync()
                ?? throw new Exception("Customer not find by Id.");

            categorieDb.Name = categorie.Name;
            categorieDb.IsEditavel = categorie.IsEditavel;

            await _categories.ReplaceOneAsync(x => x.Id == categorie.Id, categorieDb);
        }

        public async Task<List<CategorieInputDto>> GetAllCategories()
        {
            var categories = await _categories
                .Find(_ => true)
                .ToListAsync();

            return categories.Select(x =>
                new CategorieInputDto(x.Id, x.Name, x.IsEditavel, x.CreatedAt)
            ).ToList();
        }

        public async Task<CategorieInputDto?> GetByName(string name)
        {
            var categorie = await _categories
                .Find(x => x.Name == name)
                .FirstOrDefaultAsync();

            return categorie is not null
                ? new CategorieInputDto(categorie.Id, categorie.Name, categorie.IsEditavel, categorie.CreatedAt)
                : null;
        }

        public async Task<CategorieInputDto?> GetCategorieById(Guid id)
        {
            var categorie = await _categories
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return categorie is not null
                ? new CategorieInputDto(categorie.Id, categorie.Name, categorie.IsEditavel, categorie.CreatedAt)
                : null;
        }

        public async Task Delete(Guid id)
        {
            var result = await _categories.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount == 0)
                throw new Exception("Categorie not find by Id.");
        }
    }
}
