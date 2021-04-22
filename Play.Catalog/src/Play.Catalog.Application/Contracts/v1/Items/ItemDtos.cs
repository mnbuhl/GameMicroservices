using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Application.Contracts.v1.Items
{
    public record ItemResponseDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

    public record CreateItemDto(
        [Required] string Name, 
        [Required] string Description, 
        [Range(0, int.MaxValue)] decimal Price);

    public record UpdateItemDto(
        [Required] string Name, 
        [Required] string Description, 
        [Range(0, int.MaxValue)] decimal Price);
}