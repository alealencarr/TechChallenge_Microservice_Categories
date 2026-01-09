using Domain.Entities;
using FluentAssertions;

namespace Domain.UnitTests.Entities
{
    public class CategorieTests
    {

        [Fact]
        public void Deve_Criar_Categoria_Nova_Com_Sucesso()
        {
            var nome = "Bebida";
            var isEditavel = true;

            var categoria = new Categorie(nome, isEditavel);

            categoria.Should().NotBeNull();
            categoria.Id.Should().NotBeEmpty();
            categoria.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            categoria.Name.Should().Be(nome);
            categoria.IsEditavel.Should().BeTrue();
        }

        [Fact]
        public void Deve_Reconstituir_Categoria_Existente()
        {
            var id = Guid.NewGuid();
            var createdAt = DateTime.Now.AddDays(-100);
            var nome = "Acompanhamento";
            var isEditavel = false;

            var categoria = new Categorie(nome, id, createdAt, isEditavel);

            categoria.Id.Should().Be(id);
            categoria.CreatedAt.Should().Be(createdAt);
            categoria.Name.Should().Be(nome);
            categoria.IsEditavel.Should().Be(isEditavel);
        }

        [Fact]
        public void Deve_Instanciar_Construtor_Vazio()
        {
            var categoria = new Categorie();

            categoria.Should().NotBeNull();
            categoria.Id.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Lanche")]
        [InlineData("lanche")]
        [InlineData("LANCHE")]
        [InlineData("LaNcHe")]
        public void IsLanche_Deve_Retornar_True_Ignorando_Case_Sensitive(string nomeVariado)
        {
            var categoria = new Categorie(nomeVariado, true);

            var ehLanche = categoria.IsLanche();

            ehLanche.Should().BeTrue($"porque o nome '{nomeVariado}' deve ser considerado Lanche");
        }

        [Theory]
        [InlineData("Bebida")]
        [InlineData("Sobremesa")]
        [InlineData("Lanches")]
        [InlineData("Lanche ")]
        public void IsLanche_Deve_Retornar_False_Para_Outros_Nomes(string outroNome)
        {
            var categoria = new Categorie(outroNome, true);

            var ehLanche = categoria.IsLanche();

            ehLanche.Should().BeFalse();
        }
    }
}