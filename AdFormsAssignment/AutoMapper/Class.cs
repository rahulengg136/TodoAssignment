using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormsAssignment.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TodoListDto, tblTodoList>();
            CreateMap<TodoItemDto, tblTodoItem>();
            CreateMap<LabelDto, tblLabel>();
        }
    }
}
