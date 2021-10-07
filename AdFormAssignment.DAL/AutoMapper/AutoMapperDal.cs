using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AutoMapper;

namespace AdFormAssignment.DAL.AutoMapper
{
    public class AutoMapperDal : Profile
    {
        public AutoMapperDal()
        {
            CreateMap<TodoListDetail, TblTodoList>().ReverseMap();

        }
    }
}
