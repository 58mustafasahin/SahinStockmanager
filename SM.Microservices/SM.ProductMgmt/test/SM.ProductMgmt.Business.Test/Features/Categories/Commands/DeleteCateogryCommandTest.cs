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
    public class DeleteCateogryCommandTest
    {
        private DeleteCategoryCommand _deleteCategoryCommand;
        private DeleteCategoryCommand.DeleteCategoryCommandHandler _deleteCategoryCommandHandler;

        private Mock<ICategoryRepository> _categoryRepository;

        List<Category> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _categoryRepository = new Mock<ICategoryRepository>();

            _deleteCategoryCommand = new DeleteCategoryCommand();
            _deleteCategoryCommandHandler = new DeleteCategoryCommand.DeleteCategoryCommandHandler(_categoryRepository.Object);

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
        public async Task DeleteCategory_Success()
        {
            _deleteCategoryCommand = new()
            {
                Id = 1,
            };

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(_fakeCategories.AsQueryable().FirstOrDefault(x => x.Id == _deleteCategoryCommand.Id));
            _categoryRepository.Setup(x => x.Delete(It.IsAny<Category>())).Verifiable();

            var result = await _deleteCategoryCommandHandler.Handle(_deleteCategoryCommand, CancellationToken.None);
            _categoryRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task DeleteCategory_RecordDoesNotExist()
        {
            _deleteCategoryCommand = new()
            {
                Id = 2,
            };

            _categoryRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _categoryRepository.Setup(x => x.Delete(It.IsAny<Category>()));

            var result = await _deleteCategoryCommandHandler.Handle(_deleteCategoryCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
