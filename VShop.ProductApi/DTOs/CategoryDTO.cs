using System.ComponentModel.DataAnnotations;

namespace VShop.ProductApi.DTOs;

public class CategoryDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name is Required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }
    public ICollection<ProductDTO>? Products { get; set; }
}
