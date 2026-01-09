using Application.Controllers.Categories;
using Application.Interfaces.DataSources;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Categorie.Request;

namespace Categories.UnitTests.Application.Controllers
{
    public class CategorieControllerTests
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieController _controller;

        public CategorieControllerTests()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();
            _controller = new CategorieController(_dataSourceMock.Object);
        }

        [Fact]
        public async Task CreateCategorie_Should_Return_Success_When_Valid()
        {
            var request = new CategorieRequestDto { Name = "Nova", IsEditavel = true };

            _dataSourceMock.Setup(x => x.GetByName(request.Name))
                           .ReturnsAsync((CategorieInputDto?)null);
            var result = await _controller.CreateCategorie(request);

            result.Succeeded.Should().BeTrue();
            result.Data.Name.Should().Be("Nova");
            _dataSourceMock.Verify(x => x.CreateCategorie(It.IsAny<CategorieInputDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateCategorie_Should_Return_Conflict_When_Exists()
        {
            var request = new CategorieRequestDto { Name = "Existente", IsEditavel = true };
            var existing = new CategorieInputDto(Guid.NewGuid(), "Existente", true, DateTime.Now);

            _dataSourceMock.Setup(x => x.GetByName(request.Name))
                           .ReturnsAsync(existing);

            var result = await _controller.CreateCategorie(request);

            result.Succeeded.Should().BeFalse();
            result.Conflict.Should().BeTrue();
            result.Messages.Should().Contain(m => m.Contains("já existe"));
        }

        [Fact]
        public async Task CreateCategorie_Should_Handle_Exception()
        {
            var request = new CategorieRequestDto { Name = "Erro", IsEditavel = true };
            _dataSourceMock.Setup(x => x.GetByName(It.IsAny<string>())).ThrowsAsync(new Exception("Erro banco"));

            var result = await _controller.CreateCategorie(request);

            result.Succeeded.Should().BeFalse();
            result.Messages.Should().Contain("Error:Erro banco");
        }

        [Fact]
        public async Task DeleteCategorie_Should_Return_Success()
        {
            var id = Guid.NewGuid();
            var existing = new CategorieInputDto(id, "Teste", true, DateTime.Now);


            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync(existing);

            var result = await _controller.DeleteCategorie(id);

            result.Succeeded.Should().BeTrue();
            result.Messages.Should().Contain("Categoria excluida!");
        }

        [Fact]
        public async Task DeleteCategorie_Should_Handle_Exception()
        {
            var id = Guid.NewGuid();
            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ThrowsAsync(new Exception("Erro ao deletar"));

            var result = await _controller.DeleteCategorie(id);

            result.Succeeded.Should().BeFalse();
            result.Messages.Should().Contain("Error:Erro ao deletar");
        }

        [Fact]
        public async Task UpdateCategorie_Should_Return_Success()
        {
            var id = Guid.NewGuid();
            var request = new CategorieRequestDto { Name = "Editado", IsEditavel = false };
            var existing = new CategorieInputDto(id, "Antigo", true, DateTime.Now);


            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync(existing);

            var result = await _controller.UpdateCategorie(request, id);

            result.Succeeded.Should().BeTrue();
            result.Data.Name.Should().Be("Editado");
            result.Messages.Should().Contain("Categoria atualizada!");
        }

        [Fact]
        public async Task UpdateCategorie_Should_Handle_Exception()
        {
            var id = Guid.NewGuid();
            var request = new CategorieRequestDto { Name = "Editado", IsEditavel = false };

            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync((CategorieInputDto?)null);

            var result = await _controller.UpdateCategorie(request, id);

            result.Succeeded.Should().BeFalse();
            result.Messages.Should().Contain(m => m.Contains("not find") || m.Contains("Error"));
        }

        [Fact]
        public async Task GetAllCategories_Should_Return_List()
        {
            var dtos = new List<CategorieInputDto> { new CategorieInputDto(Guid.NewGuid(), "A", true, DateTime.Now) };
            _dataSourceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(dtos);

            var result = await _controller.GetAllCategoriesAsync();

            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetCategorieById_Should_Return_Data_When_Found()
        {
            var id = Guid.NewGuid();
            var dto = new CategorieInputDto(id, "B", true, DateTime.Now);
            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync(dto);

            var result = await _controller.GetCategorieByIdAsync(id);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetCategorieById_Should_Return_Error_When_Not_Found()
        {
            var id = Guid.NewGuid();
            _dataSourceMock.Setup(x => x.GetCategorieById(id)).ReturnsAsync((CategorieInputDto?)null);

            var result = await _controller.GetCategorieByIdAsync(id);

            result.Succeeded.Should().BeFalse();
            result.Messages.Should().Contain("Categorie not found.");
        }
    }
}