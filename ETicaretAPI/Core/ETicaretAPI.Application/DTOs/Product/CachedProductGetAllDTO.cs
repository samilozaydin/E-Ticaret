using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Product
{
    public class CachedProductGetAllDTO
    {
        public List<ProductGetAllDTO> Products { get; set; }
        public int ProductsCount { get; set; }
    }
}
