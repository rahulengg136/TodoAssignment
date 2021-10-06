using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using GraphQL.Types;

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
        public ToDoListType(ITodoListService todoListService)
        {
            Field(x => x.ExpectedDate);
            Field(x => x.ListName);
            Field(x => x.TodoListId);
            Field(x => x.UserId);

            Field<ListGraphType<LabelType>>(
                "labels",
                resolve: context => todoListService.GetListLabels(context.Source.TodoListId)
                );
        }
    }
}
