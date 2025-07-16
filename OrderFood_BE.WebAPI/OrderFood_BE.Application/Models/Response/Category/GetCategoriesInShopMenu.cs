using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.Category
{
    public class GetCategoriesInShopMenu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
