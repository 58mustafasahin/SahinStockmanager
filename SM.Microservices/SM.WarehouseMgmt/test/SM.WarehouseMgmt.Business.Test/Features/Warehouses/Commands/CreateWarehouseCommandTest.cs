using FluentAssertions;
using Moq;
using NUnit.Framework;
using SM.WarehouseMgmt.Business.Features.Warehouses.Commands;
using SM.WarehouseMgmt.DataAccess.Abstract;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.Business.Test.Features.Warehouses.Commands
{
    [TestFixture]
    public class CreateWarehouseCommandTest
    {
        private CreateWarehouseCommand _createWarehouseCommand;
        private CreateWarehouseCommand.CreateWarehouseCommandHandler _createWarehouseCommandHandler;

        private Mock<IWarehouseRepository> _warehouseRepository;

        [SetUp]
        public void Setup()
        {
            _warehouseRepository = new Mock<IWarehouseRepository>();

            _createWarehouseCommand = new CreateWarehouseCommand();
            _createWarehouseCommandHandler = new CreateWarehouseCommand.CreateWarehouseCommandHandler(_warehouseRepository.Object);
        }

        [Test]
        public async Task CreateWarehouse_Success()
        {
            _createWarehouseCommand = new()
            {
                Name = "Test 1",
                Address = "Test Street No:5",
            };

            _warehouseRepository.Setup(x => x.Add(It.IsAny<Warehouse>())).Returns(new Warehouse());

            var result = await _createWarehouseCommandHandler.Handle(_createWarehouseCommand, CancellationToken.None);
            _warehouseRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }
    }
}
