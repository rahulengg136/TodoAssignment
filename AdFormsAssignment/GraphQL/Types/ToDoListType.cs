using AdFormAssignment.DAL.Entities;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormsAssignment.GraphQL.Types
{
    /// <summary>
    /// GraphQL type : to-do list
    /// </summary>
    public class ToDoListType : ObjectGraphType<TblTodoList>
    {
        /// <summary>
        /// GraphQL type : to-do list
        /// </summary>
        public ToDoListType()
        {
            Field(x => x.ExpectedDate);
            Field(x => x.ListName);
            Field(x => x.TodoListId);
            Field(x => x.UserId);
        }
    }
}
