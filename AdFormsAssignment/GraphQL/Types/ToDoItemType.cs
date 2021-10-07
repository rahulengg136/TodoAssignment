using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using GraphQL.Types;
using System.Collections.Generic;
using System.Security.Claims;

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
        public ToDoItemType(ITodoItemService todoItemService)
        {
            Field(x => x.Description);
            Field(x => x.ExpectedDate);
            Field(x => x.TodoItemId);
            Field(x => x.TodoListId);
            Field(x => x.UserId);
            Field<ListGraphType<LabelType>>(
                "labels",
                resolve: context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    if (user.Identity.IsAuthenticated)
                    {
                        return todoItemService.GetItemLabels(context.Source.TodoItemId);
                    }
                    return new List<TblLabel>();
                }
                );
        }
    }
}
