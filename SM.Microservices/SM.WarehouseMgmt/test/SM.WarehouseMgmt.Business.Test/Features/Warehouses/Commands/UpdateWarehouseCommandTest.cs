using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.WarehouseMgmt.Business.Features.Warehouses.Commands;
using SM.WarehouseMgmt.DataAccess.Abstract;
using SM.WarehouseMgmt.Domain.Concrete;
using System.Linq.Expressions;

namespace SM.WarehouseMgmt.Business.Test.Features.Warehouses.Commands
{
    [TestFixture]
    public class UpdateWarehouseCommandTest
    {
        private UpdateWarehouseCommand _updateWarehouseCommand;
        private UpdateWarehouseCommand.UpdateWarehouseCommandHandler _updateWarehouseCommandHandler;

        private Mock<IWarehouseRepository> _warehouseRepository;

        List<Warehouse> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _warehouseRepository = new Mock<IWarehouseRepository>();

            _updateWarehouseCommand = new UpdateWarehouseCommand();
            _updateWarehouseCommandHandler = new UpdateWarehouseCommand.UpdateWarehouseCommandHandler(_warehouseRepository.Object);

            _fakeCategories = new()
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Test 1",
                    Address = "Test Street No:5",
                }
            };
        }

        [Test]
        public async Task UpdateWarehouse_Success()
        {
            _updateWarehouseCommand = new()
            {
                Id = 1,
                Name = "Test 2",
                Address = "Test 2 Street No:5",
            };

            _warehouseRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Warehouse, bool>>>())).ReturnsAsync(_fakeCategories.AsQueryable().FirstOrDefault(x => x.Id == _updateWarehouseCommand.Id));
            _warehouseRepository.Setup(x => x.Update(It.IsAny<Warehouse>())).Verifiable();

            var result = await _updateWarehouseCommandHandler.Handle(_updateWarehouseCommand, CancellationToken.None);
            _warehouseRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task UpdateWarehouse_RecordDoesNotExist()
        {
            _updateWarehouseCommand = new()
            {
                Id = 2,
                Name = "Test 2",
                Address = "Test 2 Street No:5",
            };

            _warehouseRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _warehouseRepository.Setup(x => x.Update(It.IsAny<Warehouse>())).Returns(new Warehouse());

            var result = await _updateWarehouseCommandHandler.Handle(_updateWarehouseCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
