using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.Core.Utilities.Paging;
using SM.WarehouseMgmt.Business.Features.Warehouses.Queries;
using SM.WarehouseMgmt.DataAccess.Abstract;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.Business.Test.Features.Warehouses.Queries
{
    [TestFixture]
    public class GetByFilterPagedHousesQueryTest
    {
        private GetByFilterPagedWarehousesQuery _getByFilterPagedWarehousesQueryTest;
        private GetByFilterPagedWarehousesQuery.GetByFilterPagedWarehousesQueryHandler _getByFilterPagedWarehousesQueryHandler;

        private Mock<IWarehouseRepository> _warehouseRepository;

        List<Warehouse> _fakeWarehouses;

        [SetUp]
        public void Setup()
        {
            _warehouseRepository = new Mock<IWarehouseRepository>();

            _getByFilterPagedWarehousesQueryTest = new GetByFilterPagedWarehousesQuery();
            _getByFilterPagedWarehousesQueryHandler = new GetByFilterPagedWarehousesQuery.GetByFilterPagedWarehousesQueryHandler(_warehouseRepository.Object);

            _fakeWarehouses = new()
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Warehouse A",
                    Address = "Warehouse A Adress.",
                    InsertTime = DateTime.Now,
                    UpdateTime = DateTime.Now.AddMinutes(1),
                },
                new Warehouse
                {
                    Id = 2,
                    Name = "Technology",
                    Address = "Technology Adress.",
                    InsertTime = DateTime.Now.AddDays(1),
                    UpdateTime = DateTime.Now.AddDays(1).AddMinutes(1),
                },
                new Warehouse
                {
                    Id = 3,
                    Name = "Test 2",
                    Address = "Test 2",
                    InsertTime = DateTime.Now.AddDays(2),
                    UpdateTime = DateTime.Now.AddDays(2).AddMinutes(1),
                },
            };
        }

        [TestCaseSource(nameof(GetPaginationTestData))]
        public async Task GetByFilterPagedWarehousesQuery_Success(string name, PaginationQuery paginationQuery, string orderBy)
        {
            _getByFilterPagedWarehousesQueryTest = new()
            {
                Name = name,
                OrderBy = orderBy,
                PaginationQuery = paginationQuery
            };
            _warehouseRepository.Setup(x => x.Query()).Returns(_fakeWarehouses.AsQueryable().BuildMock());
            _warehouseRepository.Setup(x => x.GetPagedListAsync(It.IsAny<IQueryable<Warehouse>>(), It.IsAny<PaginationQuery>()))
           .ReturnsAsync(new PagedList<Warehouse>(_fakeWarehouses, _fakeWarehouses.Count(), paginationQuery.PageNumber, paginationQuery.PageSize));


            var result = await _getByFilterPagedWarehousesQueryHandler.Handle(_getByFilterPagedWarehousesQueryTest, CancellationToken.None);
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
                "Warehouse A",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "NameASC"
                ).SetName("Name_And_OrderNameASC_Filter");

            yield return new TestCaseData(
                "Warehouse X",
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "NameDESC"
                ).SetName("OrderNameDESC_Filter");

            yield return new TestCaseData(
                null,
                new PaginationQuery { PageNumber = 1, PageSize = 2 },
                "InsertTimeASC"
                ).SetName("OrderInsertTimeASC_Filter");

            yield return new TestCaseData(
                "Warehouse A",
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
