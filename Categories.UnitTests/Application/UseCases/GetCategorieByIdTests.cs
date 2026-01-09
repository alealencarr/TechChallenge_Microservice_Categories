using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Categories;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;

namespace Categories.UnitTests.Application.UseCases
{
    public class GetCategorieByIdTests
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieGateway _gateway;
        private readonly GetCategorieByIdUseCase _useCase;

        public GetCategorieByIdTests()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();
            _gateway = CategorieGateway.Create(_dataSourceMock.Object);
            _useCase = GetCategorieByIdUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Should_Return_Categorie_When_Found()
        {
            var id = Guid.NewGuid();
            var expectedDto = new CategorieInputDto(id, "Categoria Teste", true, DateTime.Now);

            _dataSourceMock.Setup(x => x.GetCategorieById(id))
                           .ReturnsAsync(expectedDto);

            var result = await _useCase.Run(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Categoria Teste");
            _dataSourceMock.Verify(x => x.GetCategorieById(id), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Null_When_Not_Found()
        {
            var id = Guid.NewGuid();

            _dataSourceMock.Setup(x => x.GetCategorieById(id))
                           .ReturnsAsync((CategorieInputDto?)null);

            var result = await _useCase.Run(id);

            result.Should().BeNull();
            _dataSourceMock.Verify(x => x.GetCategorieById(id), Times.Once);
        }

        [Fact]
        public async Task Should_Wrap_Exception_When_Gateway_Fails()
        {
            var id = Guid.NewGuid();
            _dataSourceMock.Setup(x => x.GetCategorieById(id))
                           .ThrowsAsync(new Exception("Erro fatal"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Run(id));

            exception.Message.Should().StartWith("Error:Erro fatal");
        }
    }
}