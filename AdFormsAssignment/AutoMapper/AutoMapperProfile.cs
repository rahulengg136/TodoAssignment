using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using AutoMapper;
using System.Collections.Generic;

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
          
            CreateMap<CreateTodoListDto, TblTodoList>();
            CreateMap<CreateTodoItemDto, TblTodoItem>();
            CreateMap<CreateLabelDto, TblLabel>();

            CreateMap<TblTodoList, ReadTodoListDto>();
            CreateMap<TblTodoItem, ReadTodoItemDto>();
            CreateMap<TblLabel, ReadLabelDto>();
        }
    }
}
