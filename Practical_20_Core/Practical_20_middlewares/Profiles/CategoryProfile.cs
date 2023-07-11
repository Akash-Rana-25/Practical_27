using AutoMapper;
using Practical_20_middlewares.Dto;
using UnitOfWork.Core.Models;

namespace Practical20_Pattern_Middlewares.Profiles;

public class CategoryProfile : Profile
{
	public CategoryProfile()
	{
		CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, UpdateCategoryDto>();

        CreateMap<Category, CategoryDto>();
    }
}
