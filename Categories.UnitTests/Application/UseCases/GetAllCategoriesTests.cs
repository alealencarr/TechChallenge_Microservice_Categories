using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Categories;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;

namespace Categories.UnitTests.Application.UseCases
{
    public class GetAllCategoriesTests
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieGateway _gateway;
        private readonly GetAllCategoriesUseCase _useCase;

        public GetAllCategoriesTests()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();
            _gateway = CategorieGateway.Create(_dataSourceMock.Object);
            _useCase = GetAllCategoriesUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Should_Return_All_Categories_Successfully()
        {
            var dtos = new List<CategorieInputDto>
            {
                new CategorieInputDto(Guid.NewGuid(), "Bebidas", true, DateTime.Now),
                new CategorieInputDto(Guid.NewGuid(), "Lanche", true, DateTime.Now)
            };

            _dataSourceMock.Setup(x => x.GetAllCategories())
                           .ReturnsAsync(dtos);

            var result = await _useCase.Run();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Bebidas");
            _dataSourceMock.Verify(x => x.GetAllCategories(), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Empty_List_When_No_Categories_Found()
        {
            _dataSourceMock.Setup(x => x.GetAllCategories())
                           .ReturnsAsync(new List<CategorieInputDto>());
            var result = await _useCase.Run();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _dataSourceMock.Verify(x => x.GetAllCategories(), Times.Once);
        }

        [Fact]
        public async Task Should_Wrap_Exception_When_Gateway_Fails()
        {
            _dataSourceMock.Setup(x => x.GetAllCategories())
                           .ThrowsAsync(new Exception("Falha de conexão"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Run());

            exception.Message.Should().StartWith("Error:Falha de conexão");
        }
    }
}