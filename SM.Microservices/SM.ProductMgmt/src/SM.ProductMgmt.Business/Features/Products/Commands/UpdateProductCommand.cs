using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Products;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<IResult>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public int Unit { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;

            public UpdateProductCommandHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<IResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var currentProduct = await _productRepository.GetAsync(x => x.Id == request.Id);
                if (currentProduct is null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                currentProduct = request.Adapt<Product>();
                _productRepository.Update(currentProduct);
                await _productRepository.SaveChangesAsync();

                var getProductDto = currentProduct.Adapt<GetProductDto>();

                return new SuccessDataResult<GetProductDto>(getProductDto, Messages.SuccessfulOperation);
            }
        }
    }
}
