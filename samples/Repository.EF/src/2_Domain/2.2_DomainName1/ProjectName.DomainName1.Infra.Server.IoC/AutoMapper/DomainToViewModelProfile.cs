
using AutoMapper;

using ProjectName.DomainName1.Application.ViewModels;
using ProjectName.DomainName1.Domain.Models;

namespace ProjectName.DomainName1.Infra.Server.IoC.AutoMapper
{
    public class DomainToViewModelProfile : Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(c => c.CustomerId, opt => opt.MapFrom(c => c.Id))
                .ReverseMap();

            CreateMap<Address, AddressViewModel>()
                .ForMember(a => a.AddressId, opt => opt.MapFrom(a => a.Id))
                .ReverseMap();

            CreateMap<Contact, ContactViewModel>()
                .ForMember(c => c.ContactId, opt => opt.MapFrom(c => c.Id))
                .ReverseMap();

        }
    }
}