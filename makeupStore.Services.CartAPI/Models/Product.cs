using System.ComponentModel.DataAnnotations;

namespace makeupStore.Services.CartAPI.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Price { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Count { get; set; }
}