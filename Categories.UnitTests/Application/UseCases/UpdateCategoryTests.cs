using Application.Gateways;
using Application.Interfaces.DataSources; 
using Application.UseCases.Categories;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Request;
using Shared.DTO.Categorie.Input; 
using Xunit;

namespace Categories.UnitTests.Application.UseCases
{
    public class UpdateCategorieTests
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieGateway _gateway; 
        private readonly UpdateCategorieUseCase _useCase; 

        public UpdateCategorieTests()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();

            _gateway = CategorieGateway.Create(_dataSourceMock.Object);

            _useCase = UpdateCategorieUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Should_Update_Categorie_Successfully()
        {
             var id = Guid.NewGuid();
            var requestDto = new CategorieRequestDto { Name = "Nome Editado", IsEditavel = false };
 
            var existingCategory = new CategorieInputDto(id, "Nome Original", true, DateTime.Now);
 
            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync(existingCategory);

            var result = await _useCase.Run(requestDto, id);

            result.Should().NotBeNull();
            result.Name.Should().Be("Nome Editado");
            result.IsEditavel.Should().BeFalse();
            _dataSourceMock.Verify(x => x.UpdateCategorie(It.IsAny<CategorieInputDto>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Categorie_Not_Found()
        {
            var id = Guid.NewGuid();
            var requestDto = new CategorieRequestDto { Name = "Teste", IsEditavel = true };

            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync((CategorieInputDto?)null);
 
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Run(requestDto, id));

            exception.Message.Should().Contain("Categorie not find by Id");

            _dataSourceMock.Verify(x => x.UpdateCategorie(It.IsAny<CategorieInputDto>()), Times.Never);
        }
    }
}