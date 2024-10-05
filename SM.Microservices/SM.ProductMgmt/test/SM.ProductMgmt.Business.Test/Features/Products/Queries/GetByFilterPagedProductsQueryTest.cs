using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.Core.Utilities.Paging;
using SM.ProductMgmt.Business.Features.Products.Queries;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Test.Features.Products.Queries
{
    [TestFixture]
    public class GetByFilterPagedProductsQueryTest
    {
        private GetByFilterPagedProductsQuery _getByFilterPagedProductsQueryTest;
        private GetByFilterPagedProductsQuery.GetByFilterPagedProductsQueryHandler _getByFilterPagedProductsQueryHandler;

        private Mock<IProductRepository> _productRepository;

        List<Product> _fakeProducts;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();

            _getByFilterPagedProductsQueryTest = new GetByFilterPagedProductsQuery();
            _getByFilterPagedProductsQueryHandler = new GetByFilterPagedProductsQuery.GetByFilterPagedProductsQueryHandler(_productRepository.Object);

            _fakeProducts = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Product A",
                    Description = "Product A Desc.",
                    InsertTime = DateTime.Now,
                    UpdateTime = DateTime.Now.AddMinutes(1),
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop",
                    Description = "Portable Computer",
                    InsertTime = DateTime.Now.AddDays(1),
                    UpdateTime = DateTime.Now.AddDays(1).AddMinutes(1),
                },
                new Product
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
        public async Task GetByFilterPagedProductsQuery_Success(string name, PaginationQuery paginationQuery, string orderBy)
        {
            _getByFilterPagedProductsQueryTest = new()
            {
                Name = name,
                OrderBy = orderBy,
                PaginationQuery = paginationQuery
            };
            _productRepository.Setup(x => x.Query()).Returns(_fakeProducts.AsQueryable().BuildMock());
            _productRepository.Setup(x => x.GetPagedListAsync(It.IsAny<IQueryable<Product>>(), It.IsAny<PaginationQuery>()))
           .ReturnsAsync(new PagedList<Product>(_fakeProducts, _fakeProducts.Count(), paginationQuery.PageNumber, paginationQuery.PageSize));


            var result = await _getByFilterPagedProductsQueryHandler.Handle(_getByFilterPagedProductsQueryTest, CancellationToken.None);
            result.Success.Should().BeTrue();
        }

        // TestCaseSource method to provide test data for pagination
        public static IEnumerable<TestCaseData> GetPaginationTestData()
        {
            yield return new TestCaseData(
                "Laptop",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                null
                ).SetName("Name_Filter");

            yield return new TestCaseData(
                "Product A",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "NameASC"
                ).SetName("Name_And_OrderNameASC_Filter");

            yield return new TestCaseData(
                "Product X",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "NameDESC"
                ).SetName("OrderNameDESC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 2, PageSize = 2 },
                "CategoryNameASC"
                ).SetName("OrderCategoryNameASC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 2, PageSize = 2 },
                "CategoryNameDESC"
                ).SetName("OrderCategoryNameASC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "InsertTimeASC"
                ).SetName("OrderInsertTimeASC_Filter");

            yield return new TestCaseData(
                "Product A",
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
