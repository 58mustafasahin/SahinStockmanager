using FluentAssertions;
using Moq;
using NUnit.Framework;
using SM.ProductMgmt.Business.Features.Products.Commands;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Test.Features.Products.Commands
{
    [TestFixture]
    public class CreateProductCommandTest
    {
        private CreateProductCommand _createProductCommand;
        private CreateProductCommand.CreateProductCommandHandler _createProductCommandHandler;

        private Mock<IProductRepository> _productRepository;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();

            _createProductCommand = new CreateProductCommand();
            _createProductCommandHandler = new CreateProductCommand.CreateProductCommandHandler(_productRepository.Object);

        }

        [Test]
        public async Task CreateProduct_Success()
        {
            _createProductCommand = new()
            {
                Name = "Computer",
                Description = "Compatiple Laptop",
                Price = 1299.90,
                StockQuantity = 10,
                Unit = 20
            };

            _productRepository.Setup(x => x.Add(It.IsAny<Product>())).Returns(new Product());

            var result = await _createProductCommandHandler.Handle(_createProductCommand, CancellationToken.None);

            _productRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }
    }
}
