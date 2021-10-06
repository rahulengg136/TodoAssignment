using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using AutoMapper;

namespace AdFormsAssignment.AutoMapper
{
    /// <summary>
    /// Automapper profile
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// All required mapping configurations
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<CreateTodoListDto, TblTodoList>().ReverseMap();
            CreateMap<CreateTodoItemDto, TblTodoItem>().ReverseMap();
            CreateMap<CreateLabelDto, TblLabel>().ReverseMap();
            CreateMap<TblTodoList, ReadTodoListDto>().ReverseMap();
            CreateMap<TblTodoItem, ReadTodoItemDto>().ReverseMap();
            CreateMap<TblLabel, ReadLabelDto>().ReverseMap();
            CreateMap<TodoListDetail, ReadTodoListDto>().ReverseMap();
            CreateMap<TodoItemDetail, ReadTodoItemDto>().ReverseMap();
            CreateMap<TblTodoItemExtension, CreateTodoItemDto>().ReverseMap();
            CreateMap<TblTodoListExtension, CreateTodoListDto>().ReverseMap();
        }
    }
}
