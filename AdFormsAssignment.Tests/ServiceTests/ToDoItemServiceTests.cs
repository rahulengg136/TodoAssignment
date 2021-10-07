using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.BLL.Implementations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdFormsAssignment.Tests.ServiceTests
{
    public class ToDoItemServiceTests
    {
        private readonly Mock<ITodoItemDal> _mockToDoItemDal;
        public ToDoItemServiceTests()
        {
            _mockToDoItemDal = new Mock<ITodoItemDal>();
        }
        [Fact]
        public void GetAllTodoItems_ItemsAvailable_ReturnsItemList()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = int.MaxValue;
            string searchText = null;
            int userId = 1;

            _mockToDoItemDal.Setup(x => x.GetAllTodoItems(pageNumber, pageSize, searchText, userId))
                      .Returns(GetSampleItems);
            var service = GetToDoItemService();

            //Act
            IEnumerable<TblTodoItem> items = service.GetAllTodoItems(pageNumber, pageSize, null, userId).GetAwaiter().GetResult();

            //Assert
            Assert.NotNull(items);
            Assert.Equal(GetSampleItems().Result.Count(), items.Count());
        }

        [Fact]
        public void GetAllTodoItems_NoItemsAvailable_ReturnsEmptyList()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = int.MaxValue;
            string searchText = null;
            int userId = 1;

            _mockToDoItemDal.Setup(x => x.GetAllTodoItems(pageNumber, pageSize, searchText, userId))
                      .Returns(Task.FromResult((new List<TodoItemDetail>()).AsEnumerable()));
            var service = GetToDoItemService();

            //Act
            IEnumerable<TblTodoItem> items = service.GetAllTodoItems(pageNumber, pageSize, null, userId).GetAwaiter().GetResult();

            //Assert
            Assert.Empty(items);
        }

        #region Setup
        /// <summary>
        /// Set UP
        /// </summary>
        /// <returns>returns service</returns>
        private ITodoItemService GetToDoItemService()
        {
            return new TodoItemService(_mockToDoItemDal.Object);
        }
        #endregion

        #region MockData
        /// <summary>
        /// Mock DATA
        /// </summary>
        /// <returns></returns>
        private Task<IEnumerable<TodoItemDetail>> GetSampleItems()
        {
            List<TblLabel> tblLabels = new List<TblLabel>();
            TblLabel label = new TblLabel()
            {
                LabelId = 1,
                LabelName = "My-Label"
            };
            tblLabels.Add(label);
            return Task.FromResult(new List<TodoItemDetail>() {
            new TodoItemDetail{ TodoItemId=1,Description="Planning",ExpectedDate=System.DateTime.Now,ListName="",TodoListId=18,UserId=1,Labels=tblLabels},
            new TodoItemDetail{ TodoItemId=2,Description="Development",ExpectedDate=System.DateTime.Now,ListName="",TodoListId=18,UserId=1,Labels=tblLabels},
            new TodoItemDetail{ TodoItemId=3,Description="Meetings",ExpectedDate=System.DateTime.Now,ListName="",TodoListId=18,UserId=1,Labels=tblLabels}
            }.AsEnumerable());
        }
        #endregion
    }
}