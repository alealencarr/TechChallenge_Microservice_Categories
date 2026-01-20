using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Categorie.Input
{
    [ExcludeFromCodeCoverage]
    public record CategorieInputDto(Guid Id, string Name, bool IsEditavel, DateTime CreatedAt);
}
