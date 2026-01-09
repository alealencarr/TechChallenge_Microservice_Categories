using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Categories;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;

namespace Categories.UnitTests.Application.UseCases
{
    public class DeleteCategorieTests
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieGateway _gateway;
        private readonly DeleteCategorieUseCase _useCase;

        public DeleteCategorieTests()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();

            _gateway = CategorieGateway.Create(_dataSourceMock.Object);

            _useCase = DeleteCategorieUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Should_Delete_Categorie_Successfully()
        {
            var id = Guid.NewGuid();
            var existingCategory = new CategorieInputDto(id, "Categoria Teste", true, DateTime.Now);

            _dataSourceMock.Setup(x => x.GetCategorieById(id))
                           .ReturnsAsync(existingCategory);

            _dataSourceMock.Setup(x => x.Delete(id))
                           .Returns(Task.CompletedTask);
            await _useCase.Run(id);

            _dataSourceMock.Verify(x => x.Delete(id), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Categorie_Not_Found()
        {
            var id = Guid.NewGuid();

            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync((CategorieInputDto?)null);

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Run(id));

            exception.Message.Should().Contain("Categorie not find by Id");

            _dataSourceMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Never);
        }
    }
}