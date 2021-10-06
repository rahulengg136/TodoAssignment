using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.GraphQL.Types;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;

namespace AdFormsAssignment.GraphQL
{
    /// <summary>
    /// GraphQL queries
    /// </summary>
    [Authorize]
    public class Queries : ObjectGraphType
    {
        /// <summary>
        /// GraphQL queries
        /// </summary>
        public Queries(ILabelService labelService, ITodoItemService todoItemService, ITodoListService todoListService)
        {
            Field<ListGraphType<LabelType>>("labels", resolve: context => labelService.GetAllLabels(1, int.MaxValue, null));
            Field<ListGraphType<ToDoItemType>>("items", resolve: context => todoItemService.GetAllTodoItems(1, int.MaxValue, null, 1));
            Field<ListGraphType<ToDoListType>>("lists", resolve: context => todoListService.GetAllTodoLists(1, int.MaxValue, null, 1));
        }
    }
}
