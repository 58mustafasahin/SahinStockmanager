using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.ProductMgmt.Business.Features.Categories.Commands;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;
using System.Linq.Expressions;

namespace SM.ProductMgmt.Business.Test.Features.Categories.Commands
{
    [TestFixture]
    public class UpdateCategoryCommandTest
    {
        private UpdateCategoryCommand _updateCategoryCommand;
        private UpdateCategoryCommand.UpdateCategoryCommandHandler _updateCategoryCommandHandler;

        private Mock<ICategoryRepository> _categoryRepository;

        List<Category> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _categoryRepository = new Mock<ICategoryRepository>();

            _updateCategoryCommand = new UpdateCategoryCommand();
            _updateCategoryCommandHandler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(_categoryRepository.Object);

            _fakeCategories = new()
            {
                new Category
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                }
            };
        }

        [Test]
        public async Task UpdateCategory_Success()
        {
            _updateCategoryCommand = new()
            {
                Id = 1,
                Name = "Technology",
                Description = "Technology Desc.",
            };

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(_fakeCategories.AsQueryable().FirstOrDefault(x => x.Id == _updateCategoryCommand.Id));
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>())).Verifiable();

            var result = await _updateCategoryCommandHandler.Handle(_updateCategoryCommand, CancellationToken.None);
            _categoryRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task UpdateCategory_RecordDoesNotExist()
        {
            _updateCategoryCommand = new()
            {
                Id = 2,
                Name = "Technology",
                Description = "Technology Desc.",
            };

            _categoryRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>())).Returns(new Category());

            var result = await _updateCategoryCommandHandler.Handle(_updateCategoryCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
