namespace VueApp1.Server.Helpers;

using AutoMapper;
using VueApp1.Server.Models.Entities;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<RegisterRequest, User>();

    CreateMap<User, UserView>().ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => Enum.GetName(src.AccountType)));

    CreateMap<UpdateRequest, User>()
        .ForAllMembers(x => x.Condition((src, dest, prop) =>
        {
          // Ignore null fields
          if (prop == null) { return false; }
          // Ignore empty or null string fields
          if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) { return false; }
          // Ignore non positive integer fields
          if (prop.GetType() == typeof(int) && (int)prop <= 0) { return false; }
          return true;
        }));

  }
}
