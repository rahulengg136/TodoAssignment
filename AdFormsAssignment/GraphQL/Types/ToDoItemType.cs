using AdFormAssignment.DAL.Entities;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormsAssignment.GraphQL.Types
{
    /// <summary>
    /// GraphQL type : To-do item
    /// </summary>
    public class ToDoItemType : ObjectGraphType<TblTodoItem>
    {
        /// <summary>
        /// GraphQL type : to-do item
        /// </summary>
        public ToDoItemType()
        {
            Field(x => x.Description);
            Field(x => x.ExpectedDate);
            Field(x => x.TodoItemId);
            Field(x => x.TodoListId);
            Field(x => x.UserId);
        }
    }
}
