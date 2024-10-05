using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Paging;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Products;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Products.Queries
{
    public class GetByFilterPagedProductsQuery : IRequest<IResult>
    {
        public string Name { get; set; }
        public long? CategoryId { get; set; }
        public string OrderBy { get; set; }
        public PaginationQuery PaginationQuery { get; set; }

        public class GetByFilterPagedProductsQueryHandler : IRequestHandler<GetByFilterPagedProductsQuery, IResult>
        {
            private readonly IProductRepository _productRepository;

            public GetByFilterPagedProductsQueryHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<IResult> Handle(GetByFilterPagedProductsQuery request, CancellationToken cancellationToken)
            {
                var query = _productRepository.Query()
                    .Include(x => x.Category)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Name))
                    query = query.Where(x => request.Name.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase));

                if (request.CategoryId is not null)
                    query = query.Where(x => x.CategoryId == request.CategoryId);

                query = request.OrderBy switch
                {
                    "NameASC" => query.OrderBy(x => x.Name),
                    "NameDESC" => query.OrderByDescending(x => x.Name),
                    "CategoryNameASC" => query.OrderBy(x => x.Category.Name),
                    "CategoryNameDESC" => query.OrderByDescending(x => x.Category.Name),
                    "InsertTimeASC" => query.OrderBy(x => x.InsertTime),
                    "InsertTimeDESC" => query.OrderByDescending(x => x.InsertTime),
                    "UpdateTimeASC" => query.OrderBy(x => x.UpdateTime),
                    "UpdateTimeDESC" => query.OrderByDescending(x => x.UpdateTime),
                    _ => query.OrderByDescending(x => x.UpdateTime),
                };

                var items = await _productRepository.GetPagedListAsync(query, request.PaginationQuery);

                var getProductList = items.Adapt<GetProductDto>();

                return new SuccessDataResult<GetProductDto>(getProductList, Messages.SuccessfulOperation);
            }
        }
    }
}
