using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Products;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<IResult>
    {
        public long Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, IResult>
        {
            private readonly IProductRepository _productRepository;

            public GetProductByIdQueryHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<IResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetAsync(x => x.Id == request.Id);
                if (product == null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                var getProductDto = product.Adapt<GetProductDto>();

                return new SuccessDataResult<GetProductDto>(getProductDto, Messages.SuccessfulOperation);
            }
        }
    }
}
