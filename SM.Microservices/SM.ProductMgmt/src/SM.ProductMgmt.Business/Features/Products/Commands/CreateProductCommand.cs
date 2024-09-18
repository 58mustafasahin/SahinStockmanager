using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Products;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<DataResult<GetProductDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public int Unit { get; set; }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, DataResult<GetProductDto>>
        {
            private readonly IProductRepository _productRepository;

            public CreateProductCommandHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<DataResult<GetProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var product = request.Adapt<Product>();
                _productRepository.Add(product);
                await _productRepository.SaveChangesAsync();

                var getProductDto = product.Adapt<GetProductDto>();

                return new SuccessDataResult<GetProductDto>(getProductDto, Messages.SuccessfulOperation);
            }
        }
    }
}
