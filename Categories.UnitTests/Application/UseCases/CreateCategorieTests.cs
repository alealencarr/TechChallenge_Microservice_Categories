using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Categories;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Categorie.Request;

namespace Categories.UnitTests.Application.UseCases
{
    public class CreateCategorieTests
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieGateway _gateway;
        private readonly CreateCategorieUseCase _useCase;

        public CreateCategorieTests()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();
            _gateway = CategorieGateway.Create(_dataSourceMock.Object);
            _useCase = CreateCategorieUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Should_Return_Existing_Categorie_When_Name_Already_Exists()
        {
            var nome = "Bebidas";
            var requestDto = new CategorieRequestDto { Name = nome, IsEditavel = true };

            var existingDto = new CategorieInputDto(Guid.NewGuid(), nome, true, DateTime.Now);

            _dataSourceMock.Setup(x => x.GetByName(nome))
                           .ReturnsAsync(existingDto);

            var result = await _useCase.Run(requestDto);

            result.Item1.Should().NotBeNull();
            result.Item1.Name.Should().Be(nome);
            result.Item2.Should().BeTrue(); 

            _dataSourceMock.Verify(x => x.CreateCategorie(It.IsAny<CategorieInputDto>()), Times.Never);
        }

        [Fact]
        public async Task Should_Create_New_Categorie_Successfully()
        {
            var requestDto = new CategorieRequestDto { Name = "Nova Categoria", IsEditavel = true };

            _dataSourceMock.Setup(x => x.GetByName(requestDto.Name))
                           .ReturnsAsync((CategorieInputDto?)null);
            var result = await _useCase.Run(requestDto);

            result.Item1.Should().NotBeNull();
            result.Item2.Should().BeFalse();

            _dataSourceMock.Verify(x => x.CreateCategorie(It.IsAny<CategorieInputDto>()), Times.Once);
        }

        [Fact]
        public async Task Should_Rethrow_ArgumentException()
        {
            var requestDto = new CategorieRequestDto { Name = "Erro", IsEditavel = true };

            _dataSourceMock.Setup(x => x.GetByName(It.IsAny<string>()))
                           .ThrowsAsync(new ArgumentException("Argumento invalido"));

            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Run(requestDto));
        }

        [Fact]
        public async Task Should_Wrap_Generic_Exception()
        {
            var requestDto = new CategorieRequestDto { Name = "Erro Genérico", IsEditavel = true };

            _dataSourceMock.Setup(x => x.GetByName(It.IsAny<string>()))
                           .ThrowsAsync(new Exception("Banco caiu"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Run(requestDto));

            exception.Message.Should().StartWith("Error:Banco caiu");
        }
    }
}