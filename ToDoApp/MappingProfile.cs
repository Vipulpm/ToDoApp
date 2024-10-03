using AutoMapper;
using ToDoApp.Domain;
using ToDoApp.DTOs;

namespace ToDoApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoItem,ToDoItemDto>();
            CreateMap<ToDoItemDto,ToDoItem>();
        }
    }
}
