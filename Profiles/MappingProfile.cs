using AutoMapper;
using PlateWebAPI.DTOs;
using PlateWebAPI.Entities;
using PlateWebAPI.Models;

namespace WebApplicationAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDTO>();

            CreateMap<ItemDTO, Item>();

            CreateMap<Category, CategoryDTO>();

            CreateMap<ItemCreatingDTO, Item>();

            CreateMap<ItemUpdatingDTO, Item>();

            CreateMap<CategoryCreatingDTO, Category>();

            CreateMap<CategoryUpdatingDTO, Category>();

            /*           CreateMap<City, CityWithoutPointOfInterestDTO>();
                        CreateMap<City, CityDTO>();
                        CreateMap<PointOfInterest, PointOfInterestDTO>();
                        CreateMap<PointOfInterestForCreationDTO, PointOfInterest>();

                        CreateMap<PointOfInterestForUpdateDTO, PointOfInterest>();

                        CreateMap<PointOfInterest, PointOfInterestForUpdateDTO>();*/
        }
    }
}
