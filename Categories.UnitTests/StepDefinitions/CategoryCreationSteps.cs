using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Categories;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.DataSources;
using Moq;
using Reqnroll;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Categorie.Request;

namespace Categories.UnitTests.StepDefinitions
{
    [Binding]
    public class CategoryCreationSteps
    {
        private readonly Mock<ICategorieDataSource> _dataSourceMock;
        private readonly CategorieGateway _gatewayMock;
        private readonly CreateCategorieUseCase _useCase;

        private string _nomeInput;
        private bool _isEditavelInput;
        private Categorie? _categoriaResultado; 

        public CategoryCreationSteps()
        {
            _dataSourceMock = new Mock<ICategorieDataSource>();
            _gatewayMock = CategorieGateway.Create(_dataSourceMock.Object);
            _useCase = CreateCategorieUseCase.Create(_gatewayMock);
        }


        [Given("que eu quero criar uma categoria com nome {string} e editável")]
        public void SetupCategory(string nomeCategoria)
        {
            _nomeInput = nomeCategoria;
            _isEditavelInput = true;
        }


        [When("eu solicito a criação desta categoria")]
        public async Task RequestCategoryCreation()
        {
            var dto = new CategorieRequestDto { Name = _nomeInput, IsEditavel = _isEditavelInput };

            var result = await _useCase.Run(dto);

            _categoriaResultado = result.Item1 as Categorie;
        }

        [Then("a categoria deve ser salva no repositório")]
        public void VerifyPersistence()
        {
            _dataSourceMock.Verify(x => x.CreateCategorie(It.IsAny<CategorieInputDto>()), Times.Once);
        }

        [Then("a categoria não deve ser considerada um Lanche")]
        public void VerifyIsNotSnack()
        {
            _categoriaResultado.Should().NotBeNull();
            _categoriaResultado!.IsLanche().Should().BeFalse();
        }

        [Then("a categoria deve ser identificada como Lanche")]
        public void VerifyIsSnack()
        {
            _categoriaResultado.Should().NotBeNull();
            _categoriaResultado!.IsLanche().Should().BeTrue();
        }

        [Then("a data de criação deve ser recente")]
        public void VerifyCreationDate()
        {
            _categoriaResultado.Should().NotBeNull();

            _categoriaResultado!.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(2));
            _categoriaResultado.Id.Should().NotBeEmpty();
        }
    }
}
