using FluentAssertions;
using Moq;
using NUnit.Framework;
using SM.ProductMgmt.Business.Features.Categories.Commands;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Test.Features.Categories.Commands
{
    [TestFixture]
    public class CreateCateogryCommandTest
    {
        private CreateCategoryCommand _createCategoryCommand;
        private CreateCategoryCommand.CreateCategoryCommandHandler _createCategoryCommandHandler;

        private Mock<ICategoryRepository> _categoryRepository;

        [SetUp]
        public void Setup()
        {
            _categoryRepository = new Mock<ICategoryRepository>();

            _createCategoryCommand = new CreateCategoryCommand();
            _createCategoryCommandHandler = new CreateCategoryCommand.CreateCategoryCommandHandler(_categoryRepository.Object);
        }

        [Test]
        public async Task CreateCategory_Success()
        {
            _createCategoryCommand = new()
            {
                Name = "Technology",
                Description = "Technology Desc.",
            };

            _categoryRepository.Setup(x => x.Add(It.IsAny<Category>())).Returns(new Category());

            var result = await _createCategoryCommandHandler.Handle(_createCategoryCommand, CancellationToken.None);
            _categoryRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }
    }
}
