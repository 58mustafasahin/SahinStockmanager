using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.WarehouseMgmt.Business.Features.Warehouses.Queries;
using SM.WarehouseMgmt.DataAccess.Abstract;
using SM.WarehouseMgmt.Domain.Concrete;
using System.Linq.Expressions;

namespace SM.WarehouseMgmt.Business.Test.Features.Warehouses.Queries
{
    [TestFixture]
    public class GetWarehouseByIdQueryTest
    {
        private GetWarehouseByIdQuery _getWarehouseByIdQuery;
        private GetWarehouseByIdQuery.GetWarehouseByIdQueryHandler _getWarehouseByIdQueryHandler;

        private Mock<IWarehouseRepository> _warehouseRepository;

        List<Warehouse> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _warehouseRepository = new Mock<IWarehouseRepository>();

            _getWarehouseByIdQuery = new GetWarehouseByIdQuery();
            _getWarehouseByIdQueryHandler = new GetWarehouseByIdQuery.GetWarehouseByIdQueryHandler(_warehouseRepository.Object);

            _fakeCategories = new()
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Test",
                    Address = "Test 2 Street No:5",
                }
            };
        }

        [Test]
        public async Task GetWarehouseByIdQuery_Success()
        {
            _getWarehouseByIdQuery = new()
            {
                Id = 1,
            };

            _warehouseRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Warehouse, bool>>>())).ReturnsAsync(_fakeCategories.AsQueryable().FirstOrDefault(x => x.Id == _getWarehouseByIdQuery.Id));
            _warehouseRepository.Setup(x => x.Update(It.IsAny<Warehouse>())).Verifiable();

            var result = await _getWarehouseByIdQueryHandler.Handle(_getWarehouseByIdQuery, CancellationToken.None);
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task GetWarehouseByIdQuery_RecordDoesNotExist()
        {
            _getWarehouseByIdQuery = new()
            {
                Id = 2,
            };

            _warehouseRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _warehouseRepository.Setup(x => x.Update(It.IsAny<Warehouse>())).Returns(new Warehouse());

            var result = await _getWarehouseByIdQueryHandler.Handle(_getWarehouseByIdQuery, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
