
using System.ComponentModel.DataAnnotations.Schema;

namespace makeupStore.Services.CartAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public ProductDto? Product { get; set; }
    }
}
