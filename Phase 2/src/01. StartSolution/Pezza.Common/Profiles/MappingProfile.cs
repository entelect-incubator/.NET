namespace Common.Profiles;

using AutoMapper;
using Common.DTO;
using Common.Entities;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		this.CreateMap<Stock, StockDTO>();
		this.CreateMap<StockDTO, Stock>();
	}
}
