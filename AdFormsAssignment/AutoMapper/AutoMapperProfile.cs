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
            CreateMap<TodoListDto, tblTodoList>();
            CreateMap<TodoItemDto, tblTodoItem>();
            CreateMap<LabelDto, tblLabel>();
        }
    }
}
