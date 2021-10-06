using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdFormsAssignment.Tests.ServiceTests
{
    public class ToDoListServiceTests
    {
        private readonly Mock<ITodoListDal> _mockToDoListDAL;
        public ToDoListServiceTests()
        {
            _mockToDoListDAL = new Mock<ITodoListDal>();
        }

        [Fact]
        public void GetAllTodoLists_ListsAvailable_ReturnsToDoLists()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = int.MaxValue;
            string searchText = null;
            int userId = 1;

            _mockToDoListDAL.Setup(x => x.GetAllTodoLists(pageNumber, pageSize, searchText, userId))
                      .Returns(GetSampleLists);
            var service = GetToDoListService();

            //Act
            IEnumerable<TblTodoList> items = service.GetAllTodoLists(pageNumber, pageSize, searchText, userId).GetAwaiter().GetResult();

            //Assert
            Assert.NotNull(items);
            Assert.Equal(GetSampleLists().Result.Count(), items.Count());
        }
        [Fact]
        public void GetAllTodoList_NoListsAvailable_ReturnsEmptyList()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = int.MaxValue;
            string searchText = null;
            int userId = 1;

            _mockToDoListDAL.Setup(x => x.GetAllTodoLists(pageNumber, pageSize, searchText, userId))
                      .Returns(Task.FromResult((new List<TodoListDetail>()).AsEnumerable()));
            var service = GetToDoListService();

            //Act
            IEnumerable<TblTodoList> items = service.GetAllTodoLists(pageNumber, pageSize, searchText, userId).GetAwaiter().GetResult();

            //Assert
            Assert.Empty(items);
        }
        #region Setup
        /// <summary>
        /// Set UP
        /// </summary>
        /// <returns>returns service</returns>
        private ITodoListService GetToDoListService()
        {
            return new ToDoListService(_mockToDoListDAL.Object);
        }
        #endregion

        #region MockData
        /// <summary>
        /// Mock DATA
        /// </summary>
        /// <returns></returns>
        private Task<IEnumerable<TodoListDetail>> GetSampleLists()
        {
            List<TblLabel> tblLabels = new List<TblLabel>();
            TblLabel label = new TblLabel()
            {
                LabelId = 1,
                LabelName = "My-Label"
            };
            tblLabels.Add(label);
            return Task.FromResult(new List<TodoListDetail>() {
                 new TodoListDetail{TodoListId=1,ListName="List-1",ExpectedDate=DateTime.Now,UserId=1, Labels=tblLabels},
                 new TodoListDetail{TodoListId=2,ListName="List-2",ExpectedDate=DateTime.Now,UserId=1, Labels=tblLabels},
                 new TodoListDetail{TodoListId=3,ListName="List-3",ExpectedDate=DateTime.Now,UserId=1, Labels=tblLabels}
            }.AsEnumerable());
        }
        #endregion
    }
}
