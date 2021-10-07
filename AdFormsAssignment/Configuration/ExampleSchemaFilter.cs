using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AdFormsAssignment.DTO.Common;
using AdFormsAssignment.DTO.GetDto;
using AdFormsAssignment.DTO.PostDto;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace AdFormsAssignment.Configuration
{
    /// <summary>
    /// Put examples on swagger UI
    /// </summary>
    public class ExampleSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Implementation of schema filter
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = GetExampleOrNullFor(context.Type);
        }
        private IOpenApiAny GetExampleOrNullFor(Type type)
        {
            return type.Name switch
            {
                nameof(CreateLabelDto) => new OpenApiObject
                {
                    [nameof(CreateLabelDto.LabelName)] = new OpenApiString("Development"),
                },
                nameof(BadRequestInfo) => new OpenApiObject
                {
                    [nameof(BadRequestInfo.Message)] = new OpenApiString("This is a bad request. Please check your Inputs."),
                },
                nameof(UnauthorizedInfo) => new OpenApiString("Error: response status is 401"),
                nameof(ReadLabelDto) => new OpenApiObject
                {
                    [nameof(ReadLabelDto.LabelId)] = new OpenApiInteger(1),
                    [nameof(ReadLabelDto.LabelName)] = new OpenApiString("Planning"),
                },
                nameof(TblLabel) => new OpenApiObject
                {
                    [nameof(TblLabel.LabelId)] = new OpenApiInteger(1),
                    [nameof(TblLabel.LabelName)] = new OpenApiString("Planning"),
                },
                nameof(ReadTodoItemDto) => new OpenApiObject
                {
                    [nameof(ReadTodoItemDto.Labels)] = new OpenApiObject
                    {
                        [nameof(TblLabel.LabelId)] = new OpenApiInteger(1),
                        [nameof(TblLabel.LabelName)] = new OpenApiString("Planning"),
                    },
                    [nameof(ReadTodoItemDto.Description)] = new OpenApiString("ItemName-1"),
                    [nameof(ReadTodoItemDto.ExpectedDate)] = new OpenApiDateTime(DateTime.Now),
                    [nameof(ReadTodoItemDto.ListName)] = new OpenApiString("My To do List Name"),
                    [nameof(ReadTodoItemDto.TodoItemId)] = new OpenApiInteger(11),
                    [nameof(ReadTodoItemDto.TodoListId)] = new OpenApiInteger(22),
                },
                nameof(TodoItemDetail) => new OpenApiObject
                {
                    [nameof(TodoItemDetail.Labels)] = new OpenApiObject
                    {
                        [nameof(TblLabel.LabelId)] = new OpenApiInteger(1),
                        [nameof(TblLabel.LabelName)] = new OpenApiString("Planning"),
                    },
                    [nameof(TodoItemDetail.Description)] = new OpenApiString("ItemName-1"),
                    [nameof(TodoItemDetail.ExpectedDate)] = new OpenApiDateTime(DateTime.Now),
                    [nameof(TodoItemDetail.ListName)] = new OpenApiString("My To do List Name"),
                    [nameof(TodoItemDetail.TodoItemId)] = new OpenApiInteger(11),
                    [nameof(TodoItemDetail.TodoListId)] = new OpenApiInteger(22),
                },
                nameof(TodoListDetail) => new OpenApiObject
                {
                    [nameof(TodoListDetail.Labels)] = new OpenApiObject
                    {
                        [nameof(TblLabel.LabelId)] = new OpenApiInteger(1),
                        [nameof(TblLabel.LabelName)] = new OpenApiString("Planning"),
                    },
                    [nameof(TodoListDetail.ListName)] = new OpenApiString("ListName-1"),
                    [nameof(TodoListDetail.ExpectedDate)] = new OpenApiDateTime(DateTime.Now),
                    [nameof(TodoListDetail.TodoListId)] = new OpenApiInteger(11),
                },
                nameof(ReadTodoListDto) => new OpenApiObject
                {
                    [nameof(ReadTodoListDto.Labels)] = new OpenApiObject
                    {
                        [nameof(TblLabel.LabelId)] = new OpenApiInteger(1),
                        [nameof(TblLabel.LabelName)] = new OpenApiString("Planning"),
                    },
                    [nameof(ReadTodoListDto.ListName)] = new OpenApiString("ListName-1"),
                    [nameof(ReadTodoListDto.ExpectedDate)] = new OpenApiDateTime(DateTime.Now),
                    [nameof(ReadTodoListDto.TodoListId)] = new OpenApiInteger(11),
                },
                nameof(CreateTodoListDto) => new OpenApiObject
                {
                    [nameof(CreateTodoListDto.LabelIds)] = new OpenApiArray(),
                    [nameof(CreateTodoListDto.ListName)] = new OpenApiString("ListName-1"),
                    [nameof(CreateTodoListDto.ExpectedDate)] = new OpenApiDateTime(DateTime.Now),
                },
                nameof(ValidatedUserToken) => new OpenApiObject
                {
                    [nameof(ValidatedUserToken.Message)] = new OpenApiString("Your token in string format"),
                },
                nameof(CreateTodoItemDto) => new OpenApiObject
                {
                    [nameof(CreateTodoItemDto.Description)] = new OpenApiString("To do item -1 "),
                    [nameof(CreateTodoItemDto.ExpectedDate)] = new OpenApiDateTime(DateTime.Now),
                    [nameof(CreateTodoItemDto.LabelIds)] = new OpenApiArray(),
                    [nameof(CreateTodoItemDto.TodoListId)] = new OpenApiInteger(11),
                },
                _ => null,
            };
        }
    }
}
