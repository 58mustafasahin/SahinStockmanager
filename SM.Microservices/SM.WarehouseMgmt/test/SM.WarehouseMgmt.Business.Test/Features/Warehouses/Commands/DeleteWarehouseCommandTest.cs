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
    public class DeleteWarehouseCommandTest
    {
        private DeleteWarehouseCommand _deleteWarehouseCommand;
        private DeleteWarehouseCommand.DeleteWarehouseCommandHandler _deleteWarehouseCommandHandler;

        private Mock<IWarehouseRepository> _warehouseRepository;

        List<Warehouse> _fakeCategories;

        [SetUp]
        public void Setup()
        {
            _warehouseRepository = new Mock<IWarehouseRepository>();

            _deleteWarehouseCommand = new DeleteWarehouseCommand();
            _deleteWarehouseCommandHandler = new DeleteWarehouseCommand.DeleteWarehouseCommandHandler(_warehouseRepository.Object);

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
        public async Task DeleteWarehouse_Success()
        {
            _deleteWarehouseCommand = new()
            {
                Id = 1,
            };

            _warehouseRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Warehouse, bool>>>())).ReturnsAsync(_fakeCategories.AsQueryable().FirstOrDefault(x => x.Id == _deleteWarehouseCommand.Id));
            _warehouseRepository.Setup(x => x.Delete(It.IsAny<Warehouse>())).Verifiable();

            var result = await _deleteWarehouseCommandHandler.Handle(_deleteWarehouseCommand, CancellationToken.None);
            _warehouseRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task DeleteWarehouse_RecordDoesNotExist()
        {
            _deleteWarehouseCommand = new()
            {
                Id = 2,
            };

            _warehouseRepository.Setup(x => x.Query()).Returns(_fakeCategories.AsQueryable().BuildMock());
            _warehouseRepository.Setup(x => x.Delete(It.IsAny<Warehouse>()));

            var result = await _deleteWarehouseCommandHandler.Handle(_deleteWarehouseCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
