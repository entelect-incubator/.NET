namespace Pezza.Common.Profiles;

using AutoMapper;
using Pezza.Common.Entities;
using Pezza.Common.Models;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		this.CreateMap<Pizza, PizzaModel>();
		this.CreateMap<PizzaModel, Pizza>();
	}
}
