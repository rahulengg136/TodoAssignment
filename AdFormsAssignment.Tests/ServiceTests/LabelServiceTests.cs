using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.BLL.Implementations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdFormsAssignment.Tests.ServiceTests
{
    public class LabelServiceTests
    {
        private readonly Mock<ILabelDal> _mockLabelDal;
        public LabelServiceTests()
        {
            _mockLabelDal = new Mock<ILabelDal>();
        }
        [Fact]
        public void GetAllLabels_LabelsAvailable_ReturnsLabelList()
        {
            //Arrange
            int pageNumber = 10;
            int pageSize = int.MaxValue;
            string searchText = null;

            _mockLabelDal.Setup(x => x.GetAllLabels(pageNumber, pageSize, searchText))
                      .Returns(GetSampleLabels);
            var service = GetLabelService();

            //Act
            IEnumerable<TblLabel> labels = service.GetAllLabels(pageNumber, pageSize, null).GetAwaiter().GetResult();

            //Assert
            Assert.NotNull(labels);
            Assert.Equal(GetSampleLabels().Result.Count(), labels.Count());
        }

        [Fact]
        public void GetAllLabels_LabelsNotAvailable_ReturnsBlankList()
        {
            //Arrange
            int pageNumber = 10;
            int pageSize = int.MaxValue;
            string searchText = null;

            _mockLabelDal.Setup(x => x.GetAllLabels(pageNumber, pageSize, searchText))
                      .Returns(Task.FromResult((new List<TblLabel>()).AsEnumerable()));
            var service = GetLabelService();

            //Act
            IEnumerable<TblLabel> labels = service.GetAllLabels(pageNumber, pageSize, null).GetAwaiter().GetResult();

            //Assert
            Assert.Empty(labels);
        }

        [Fact]
        public void GetSingleLabelInfo_LabelAvailable_ReturnLabelDetails()
        {
            //Arrange
            int labelId = 2;
            var labelAccToId = GetSampleLabels().GetAwaiter().GetResult().SingleOrDefault(x => x.LabelId == labelId);
            _mockLabelDal.Setup(x => x.GetSingleLabel(labelId))
                   .Returns(Task.FromResult(labelAccToId));
            var service = GetLabelService();

            //Act
            TblLabel labelInfo = service.GetSingleLabelInfo(2).GetAwaiter().GetResult();

            //Assert
            Assert.NotNull(labelInfo);
        }

        [Fact]
        public void GetSingleLabelInfo_LabelNotAvailable_ReturnNull()
        {
            //Arrange
            int labelId = 22; // id not there in mocked data
            var labelAccToId = GetSampleLabels().GetAwaiter().GetResult().SingleOrDefault(x => x.LabelId == labelId);
            _mockLabelDal.Setup(x => x.GetSingleLabel(labelId))
                   .Returns(Task.FromResult(labelAccToId));
            var service = GetLabelService();

            //Act
            TblLabel labelInfo = service.GetSingleLabelInfo(2).GetAwaiter().GetResult();

            //Assert
            Assert.Null(labelInfo);
        }

        [Fact]
        public void CreateLabel_LabelsGetsCreated_ReturnNewlyCreatedLabelId()
        {
            //Arrange
            TblLabel label = new TblLabel() { LabelName = "New Label" };
            _mockLabelDal.Setup(x => x.CreateLabel(label))
                 .Returns(Task.FromResult(1));

            var service = GetLabelService();

            //Act
            int newlyCreatedLabelId = service.CreateLabel(label).GetAwaiter().GetResult();

            //Assert
            Assert.IsType<int>(newlyCreatedLabelId);
        }

        [Fact]
        public void DeleteLabel_LabelDeletesSuccessfully_ReturnOk()
        {
            //Arrange
            int labelId = 1;
            _mockLabelDal.Setup(x => x.DeleteLabel(labelId))
                 .Returns(Task.FromResult(1));

            var service = GetLabelService();

            //Act
            int deletedLabelId = service.DeleteLabel(1).GetAwaiter().GetResult();

            //Assert
            Assert.IsType<int>(deletedLabelId);
        }


        #region Setup
        /// <summary>
        /// Set UP
        /// </summary>
        /// <returns>returns service</returns>
        private ILabelService GetLabelService()
        {
            return new LabelService(_mockLabelDal.Object);
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