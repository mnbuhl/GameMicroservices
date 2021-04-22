using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Application.Contracts.v1;

namespace Play.Catalog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemResponseDto> Items = new List<ItemResponseDto>
        {
            new ItemResponseDto(Guid.NewGuid(), "Potion", "Restores small amount of HP", 50, DateTimeOffset.UtcNow),
            new ItemResponseDto(Guid.NewGuid(), "Super Potion", "Restores medium amount of HP", 100, DateTimeOffset.UtcNow),
            new ItemResponseDto(Guid.NewGuid(), "Antidote", "Cures poisoned player", 75, DateTimeOffset.UtcNow)
        };
    }
}