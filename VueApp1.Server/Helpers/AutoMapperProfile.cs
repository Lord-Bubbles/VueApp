namespace VueApp1.Server.Helpers;

using AutoMapper;
using VueApp1.Server.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, AuthenticateResponse>();

        CreateMap<UpdateRequest, User>()
            .ForAllMembers(x => x.Condition((src, dest, prop) =>
            {
                // Ignore null fields
                if (prop == null) { return false; }
                // Ignore empty or null string fields
                if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) { return false; }
                // Ignore the ID field
                if (prop.GetType() == typeof(int) && (int)prop <= 0) { return false; }
                return true;
            }));

        CreateMap<User, UserView>();
    }
}