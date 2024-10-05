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
    public class UpdateProductCommandTest
    {
        private UpdateProductCommand _updateProductCommand;
        private UpdateProductCommand.UpdateProductCommandHandler _updateProductCommandHandler;

        private Mock<IProductRepository> _productRepository;

        List<Product> _fakeProducts;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();

            _updateProductCommand = new UpdateProductCommand();
            _updateProductCommandHandler = new UpdateProductCommand.UpdateProductCommandHandler(_productRepository.Object);

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
        public async Task UpdateProduct_Success()
        {
            _updateProductCommand = new()
            {
                Id = 1,
                Name = "Laptop",
                Description = "Portable Computer",
            };

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(_fakeProducts.AsQueryable().FirstOrDefault(x => x.Id == _updateProductCommand.Id));
            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Verifiable();

            var result = await _updateProductCommandHandler.Handle(_updateProductCommand, CancellationToken.None);
            _productRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task UpdateProduct_RecordDoesNotExist()
        {
            _updateProductCommand = new()
            {
                Id = 2,
                Name = "Laptop",
                Description = "Portable Computer",
            };

            _productRepository.Setup(x => x.Query()).Returns(_fakeProducts.AsQueryable().BuildMock());
            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(new Product());

            var result = await _updateProductCommandHandler.Handle(_updateProductCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
