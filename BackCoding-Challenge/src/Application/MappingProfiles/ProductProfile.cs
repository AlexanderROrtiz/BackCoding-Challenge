using AutoMapper;
using BackCoding.Challenge.Application.DTOs.Products;
using BackCoding.Challenge.Domain.Entities;

namespace BackCoding.Challenge.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, ProductResponseDto>();
        }
    }
}
