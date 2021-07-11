using System;

namespace Play.Catalog.Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}