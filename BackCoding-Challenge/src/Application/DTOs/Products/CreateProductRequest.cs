
namespace BackCoding.Challenge.Application.DTOs.Products
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public decimal MinAmount { get; set; }
    }

    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public decimal MinAmount { get; set; }
    }
}
