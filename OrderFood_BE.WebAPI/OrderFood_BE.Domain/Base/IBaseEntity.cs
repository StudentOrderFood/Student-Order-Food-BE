﻿namespace OrderFood_BE.Domain.Base
{
    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        bool IsDeleted { get; set; }
    }
}
