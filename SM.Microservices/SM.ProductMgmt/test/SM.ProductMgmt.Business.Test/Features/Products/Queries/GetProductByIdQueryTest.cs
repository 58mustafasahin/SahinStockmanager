using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.ProductMgmt.Business.Features.Products.Queries;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;
using System.Linq.Expressions;

namespace SM.ProductMgmt.Business.Test.Features.Products.Queries
{
    [TestFixture]
    public class GetProductByIdQueryTest
    {
        private GetProductByIdQuery _getProductByIdQuery;
        private GetProductByIdQuery.GetProductByIdQueryHandler _getProductByIdQueryHandler;

        private Mock<IProductRepository> _productRepository;

        List<Product> _fakeProducts;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();

            _getProductByIdQuery = new GetProductByIdQuery();
            _getProductByIdQueryHandler = new GetProductByIdQuery.GetProductByIdQueryHandler(_productRepository.Object);

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
        public async Task GetProductByIdQuery_Success()
        {
            _getProductByIdQuery = new()
            {
                Id = 1,
            };

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(_fakeProducts.AsQueryable().FirstOrDefault(x => x.Id == _getProductByIdQuery.Id));
            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Verifiable();

            var result = await _getProductByIdQueryHandler.Handle(_getProductByIdQuery, CancellationToken.None);
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task GetProductByIdQuery_RecordDoesNotExist()
        {
            _getProductByIdQuery = new()
            {
                Id = 2,
            };

            _productRepository.Setup(x => x.Query()).Returns(_fakeProducts.AsQueryable().BuildMock());
            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(new Product());

            var result = await _getProductByIdQueryHandler.Handle(_getProductByIdQuery, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}
