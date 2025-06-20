﻿using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<User> Users { get; set; }
    }
}
