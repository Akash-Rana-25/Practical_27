using AutoMapper;
using Practical_20_middlewares.Dto;

using UnitOfWork.Core.Models;

namespace Practical20_Pattern_Middlewares.Profiles;

public class ProductProfile : Profile
{
	public ProductProfile()
	{
		CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, UpdateProductDto>();

        CreateMap<Product, ProductDto>();
    }
}
