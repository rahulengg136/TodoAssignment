using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AdFormsAssignment.DTO.GetDto;
using AdFormsAssignment.DTO.PostDto;
using AutoMapper;

namespace AdFormsAssignment.AutoMapper
{
    /// <summary>
    /// AutoMapper profile
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
            CreateMap<TodoItemDetail, object>().ReverseMap();
            CreateMap<TodoListDetail, TblTodoList>().ReverseMap();
        }
    }
}
