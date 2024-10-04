using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.Core.Utilities.Paging;
using SM.ProductMgmt.Business.Features.Categories.Queries;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Test.Features.Categories.Queries
{
    [TestFixture]
    public class GetByFilterPagedCategoriesQueryTest
    {
        private GetByFilterPagedCategoriesQuery _getByFilterPagedCategoriesQueryTest;
        private GetByFilterPagedCategoriesQuery.GetByFilterPagedCategoriesQueryHandler _getByFilterPagedCategoriesQueryHandler;

        private Mock<ICategoryRepository> _categoryRepository;

        List<Category> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _categoryRepository = new Mock<ICategoryRepository>();

            _getByFilterPagedCategoriesQueryTest = new GetByFilterPagedCategoriesQuery();
            _getByFilterPagedCategoriesQueryHandler = new GetByFilterPagedCategoriesQuery.GetByFilterPagedCategoriesQueryHandler(_categoryRepository.Object);

            _fakeCategories = new()
            {
                new Category
                {
                    Id = 1,
                    Name = "Category A",
                    Description = "Category A Desc.",
                    InsertTime = DateTime.Now,
                    UpdateTime = DateTime.Now.AddMinutes(1),
                },
                new Category
                {
                    Id = 2,
                    Name = "Technology",
                    Description = "Technology Desc.",
                    InsertTime = DateTime.Now.AddDays(1),
                    UpdateTime = DateTime.Now.AddDays(1).AddMinutes(1),
                },
                new Category
                {
                    Id = 3,
                    Name = "Test 2",
                    Description = "Test 2",
                    InsertTime = DateTime.Now.AddDays(2),
                    UpdateTime = DateTime.Now.AddDays(2).AddMinutes(1),
                },
            };
        }

        [TestCaseSource(nameof(GetPaginationTestData))]
        public async Task GetByFilterPagedCategoriesQuery_Success(string name, PaginationQuery paginationQuery, string orderBy)
        {
            _getByFilterPagedCategoriesQueryTest = new()
            {
                Name = name,
                OrderBy = orderBy,
                PaginationQuery = paginationQuery
            };
            _categoryRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _categoryRepository.Setup(x => x.GetPagedListAsync(It.IsAny<IQueryable<Category>>(), It.IsAny<PaginationQuery>()))
           .ReturnsAsync(new PagedList<Category>(_fakeCategories, _fakeCategories.Count(), paginationQuery.PageNumber, paginationQuery.PageSize));


            var result = await _getByFilterPagedCategoriesQueryHandler.Handle(_getByFilterPagedCategoriesQueryTest, CancellationToken.None);
            result.Success.Should().BeTrue();
        }

        // TestCaseSource method to provide test data for pagination
        public static IEnumerable<TestCaseData> GetPaginationTestData()
        {
            yield return new TestCaseData(
                "Technology",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                null
                ).SetName("Name_Filter");

            yield return new TestCaseData(
                "Category A",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "NameASC"
                ).SetName("Name_And_OrderNameASC_Filter");

            yield return new TestCaseData(
                "Category X",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "NameDESC"
                ).SetName("OrderNameDESC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "InsertTimeASC"
                ).SetName("OrderInsertTimeASC_Filter");

            yield return new TestCaseData(
                "Category A",
                new PaginationQuery { PageNumber = 2, PageSize = 2 },
                "InsertTimeDESC"
                ).SetName("OrderInsertTimeDESC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 2, PageSize = 2 },
                "UpdateTimeASC"
                ).SetName("OrderUpdateTimeASC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 2, PageSize = 2 },
                "UpdateTimeDESC"
                ).SetName("OrderUpdateTimeASC_Filter");
        }
    }
}
