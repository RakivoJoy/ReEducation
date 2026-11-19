using AutoMapper;
using StorageApi.DTOs;

namespace StorageApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map Product entity to ProductDto
        CreateMap<Product, ProductDto>();

        // Map CreateProductDto to Product entity
        CreateMap<CreateProductDto, Product>();
    }
}
