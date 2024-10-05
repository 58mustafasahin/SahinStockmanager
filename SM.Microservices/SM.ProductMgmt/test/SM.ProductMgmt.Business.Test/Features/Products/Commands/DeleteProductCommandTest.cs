using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.ProductMgmt.Business.Features.Products.Commands;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;
using System.Linq.Expressions;

namespace SM.ProductMgmt.Business.Test.Features.Products.Commands
{
    [TestFixture]
    public class DeleteProductCommandTest
    {
        private DeleteProductCommand _deleteProductCommand;
        private DeleteProductCommand.DeleteProductCommandHandler _deleteProductCommandHandler;

        private Mock<IProductRepository> _ProductRepository;

        List<Product> _fakeProducts;

        [SetUp]
        public void Setup()
        {
            _ProductRepository = new Mock<IProductRepository>();

            _deleteProductCommand = new DeleteProductCommand();
            _deleteProductCommandHandler = new DeleteProductCommand.DeleteProductCommandHandler(_ProductRepository.Object);

            _fakeProducts = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                }
            };
        }

        [Test]
        public async Task DeleteProduct_Success()
        {
            _deleteProductCommand = new()
            {
                Id = 1,
            };

            _ProductRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(_fakeProducts.AsQueryable().FirstOrDefault(x => x.Id == _deleteProductCommand.Id));
            _ProductRepository.Setup(x => x.Delete(It.IsAny<Product>())).Verifiable();

            var result = await _deleteProductCommandHandler.Handle(_deleteProductCommand, CancellationToken.None);
            _ProductRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task DeleteProduct_RecordDoesNotExist()
        {
            _deleteProductCommand = new()
            {
                Id = 2,
            };

            _ProductRepository.Setup(x => x.Query()).Returns(_fakeProducts.AsQueryable().BuildMock());
            _ProductRepository.Setup(x => x.Delete(It.IsAny<Product>()));

            var result = await _deleteProductCommandHandler.Handle(_deleteProductCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
