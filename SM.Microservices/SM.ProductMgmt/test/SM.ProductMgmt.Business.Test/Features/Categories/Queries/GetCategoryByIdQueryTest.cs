using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.ProductMgmt.Business.Features.Categories.Queries;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;
using System.Linq.Expressions;

namespace SM.ProductMgmt.Business.Test.Features.Categories.Queries
{
    [TestFixture]
    public class GetCategoryByIdQueryTest
    {
        private GetCategoryByIdQuery _getCategoryByIdQuery;
        private GetCategoryByIdQuery.GetCategoryByIdQueryHandler _getCategoryByIdQueryHandler;

        private Mock<ICategoryRepository> _categoryRepository;

        List<Category> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _categoryRepository = new Mock<ICategoryRepository>();

            _getCategoryByIdQuery = new GetCategoryByIdQuery();
            _getCategoryByIdQueryHandler = new GetCategoryByIdQuery.GetCategoryByIdQueryHandler(_categoryRepository.Object);

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
        public async Task GetCategoryByIdQuery_Success()
        {
            _getCategoryByIdQuery = new()
            {
                Id = 1,
            };

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(_fakeCategories.AsQueryable().FirstOrDefault(x => x.Id == _getCategoryByIdQuery.Id));
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>())).Verifiable();

            var result = await _getCategoryByIdQueryHandler.Handle(_getCategoryByIdQuery, CancellationToken.None);
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task GetCategoryByIdQuery_RecordDoesNotExist()
        {
            _getCategoryByIdQuery = new()
            {
                Id = 2,
            };

            _categoryRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>())).Returns(new Category());

            var result = await _getCategoryByIdQueryHandler.Handle(_getCategoryByIdQuery, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
