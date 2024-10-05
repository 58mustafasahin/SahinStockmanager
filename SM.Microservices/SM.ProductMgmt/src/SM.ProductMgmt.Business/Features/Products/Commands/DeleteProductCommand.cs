using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Products;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<IResult>
    {
        public long Id { get; set; }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;

            public DeleteProductCommandHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<IResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var currentProduct = await _productRepository.GetAsync(x => x.Id == request.Id);
                if (currentProduct is null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                _productRepository.Delete(currentProduct);
                await _productRepository.SaveChangesAsync();

                var getProductDto = currentProduct.Adapt<GetProductDto>();

                return new SuccessDataResult<GetProductDto>(getProductDto, Messages.SuccessfulOperation);
            }
        }
    }
}
