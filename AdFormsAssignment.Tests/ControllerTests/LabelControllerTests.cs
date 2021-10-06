using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.AutoMapper;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.Controllers;
using AdFormsAssignment.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdFormsAssignment.Tests
{
    public class LabelControllerTests
    {
        private readonly Mock<ILabelService> _mockLabelService;
        private static IMapper _mapper;
        public LabelControllerTests()
        {
            _mockLabelService = new Mock<ILabelService>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void GetAllLabels_HaveLabels_ReturnOk()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = int.MaxValue;
            string searchText = null;

            _mockLabelService.Setup(x => x.GetAllLabels(pageNumber, pageSize, searchText))
            .Returns(GetSampleLabels);
            var controller = GetLabelController();

            //Act
            var actionResult = controller.GetAllLabels(pageNumber, pageSize, searchText);
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as IEnumerable<ReadLabelDto>;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(GetSampleLabels().Result.Count(), actual.Count());
        }

        [Fact]
        public void GetAllLabels_NoLabels_ReturnNoContent()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = int.MaxValue;
            string searchText = null;

            _mockLabelService.Setup(x => x.GetAllLabels(pageNumber, pageSize, searchText))
            .Returns(Task.FromResult(new List<TblLabel>().AsEnumerable()));
            var controller = GetLabelController();

            //Act
            var actionResult = controller.GetAllLabels(pageNumber, pageSize, searchText);
            var result = actionResult.Result as NoContentResult;

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetSingleLabelInfo_LabelIdIsZero_ReturnsBadRequest()
        {
            //Arrange
            int labelId = 0;
            var controller = GetLabelController();

            //Act
            var actionResult = controller.GetSingleLabelInfo(labelId);
            var result = actionResult.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetSingleLabelInfo_LabelPresent_ReturnsOk()
        {
            //Arrange
            int labelId = 1;
            var singleLabel = GetSampleLabels().Result.First(x => x.LabelId == labelId);
            _mockLabelService.Setup(x => x.GetSingleLabelInfo(labelId)).Returns(Task.FromResult(singleLabel));
            var controller = GetLabelController();

            //Act
            var actionResult = controller.GetSingleLabelInfo(labelId);
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as ReadLabelDto;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
        }

        [Fact]
        public void GetSingleLabelInfo_LabelNotAvailable_ReturnsNoContent()
        {
            //Arrange
            int labelId = 3;
            var singleLabel = GetSampleLabels().Result.FirstOrDefault(x => x.LabelId == labelId);
            _mockLabelService.Setup(x => x.GetSingleLabelInfo(labelId)).Returns(Task.FromResult(singleLabel));
            var controller = GetLabelController();

            //Act
            var actionResult = controller.GetSingleLabelInfo(labelId);
            var result = actionResult.Result as NoContentResult;

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CreateLabel_LabelNameIsNull_ReturnsBadRequest()
        {
            //Arrange
            CreateLabelDto createLabelDto = new CreateLabelDto() { LabelName = "" };
            var controller = GetLabelController();
            //Act
            var actionResult = controller.CreateLabel(createLabelDto);
            var result = actionResult.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateLabel_LabelGetsCreated_ReturnOk()
        {
            //Arrange
            TblLabel labelDto = new TblLabel() { LabelName = "New Label" };
            CreateLabelDto createLabelDto = new CreateLabelDto() { LabelName = "New Label" };
            _mockLabelService.Setup(x => x.CreateLabel(labelDto)).Returns(Task.FromResult(1));
            var controller = GetLabelController();

            //Act
            var actionResult = controller.CreateLabel(createLabelDto);
            var result = actionResult.Result as CreatedResult;

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void DeleteLabel_PassedZeroLabelId_ReturnsBadRequest()
        {
            //Arrange
            // Do not change this label id from zero to something else, it can delete
            int labelId = 0;
            var controller = GetLabelController();

            //Act
            var actionResult = controller.DeleteLabel(labelId);
            var result = actionResult.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeleteLabel_DeletedSuccessfully_ReturnOk()
        {
            //Arrange
            int labelId = 1;
            var singleLabel = GetSampleLabels().Result.First(x => x.LabelId == labelId);
            _mockLabelService.Setup(x => x.GetSingleLabelInfo(labelId)).Returns(Task.FromResult(singleLabel));
            _mockLabelService.Setup(x => x.DeleteLabel(labelId)).Returns(Task.FromResult(labelId));
            var controller = GetLabelController();

            //Act
            var actionResult = controller.DeleteLabel(labelId);
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<TblLabel>(actual);
        }

        [Fact]
        public void DeleteLabel_LabelIdNotPresent_ReturnsBadRequest()
        {
            //Arrange
            int labelId = 11; // id not present in mocked data
            var singleLabel = GetSampleLabels().Result.FirstOrDefault(x => x.LabelId == labelId);

            _mockLabelService.Setup(x => x.GetSingleLabelInfo(labelId)).Returns(Task.FromResult(singleLabel));
            var controller = GetLabelController();

            //Act
            var actionResult = controller.DeleteLabel(labelId);
            var result = actionResult.Result as BadRequestObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #region Setup
        /// <summary>
        /// Set UP
        /// </summary>
        /// <returns>retuns controller</returns>
        private LabelController GetLabelController()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["X-Correlation-Id"] = "test-header";

            var controller = new LabelController(_mockLabelService.Object, _mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            return controller;
        }
        #endregion

        #region MockData
        /// <summary>
        /// Mock DATA
        /// </summary>
        /// <returns></returns>

        private Task<IEnumerable<TblLabel>> GetSampleLabels()
        {
            return Task.FromResult(new List<TblLabel>() {
            new TblLabel{ LabelId=1,LabelName="Planning"},
              new TblLabel{ LabelId=2,LabelName="Development"}
            }.AsEnumerable());

        }
        #endregion
    }
}