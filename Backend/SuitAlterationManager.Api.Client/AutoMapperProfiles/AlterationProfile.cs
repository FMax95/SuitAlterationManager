using AutoMapper;
using SuitAlterationManager.Api.Client.RetailManagement.Models;
using SuitAlterationManager.Domain.RetailManagement.DTO;

namespace SuitAlterationManager.Api.Client.AutoMapperProfiles
{
    public class AlterationProfile : Profile
	{
		public AlterationProfile()
		{
			CreateMap<CreateAlterationModel, CreateAlterationDTO>()
				.ForMember(
					dest => dest.AlterationType,
					opt => opt.MapFrom(src => src.AlterationTypeEnum))
				.ForMember(
					dest => dest.AlterationDirection,
					opt => opt.MapFrom(src => src.AlterationTypeDirectionEnum));
		}
	}
}
