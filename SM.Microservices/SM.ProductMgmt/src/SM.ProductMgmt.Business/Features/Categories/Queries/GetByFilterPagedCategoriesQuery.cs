using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Paging;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Categories;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Categories.Queries
{
    public class GetByFilterPagedCategoriesQuery : IRequest<IResult>
    {
        public string Name { get; set; }
        public string OrderBy { get; set; }
        public PaginationQuery PaginationQuery { get; set; }

        public class GetByFilterPagedCategoriesQueryHandler : IRequestHandler<GetByFilterPagedCategoriesQuery, IResult>
        {
            private readonly ICategoryRepository _categoryRepository;

            public GetByFilterPagedCategoriesQueryHandler(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<IResult> Handle(GetByFilterPagedCategoriesQuery request, CancellationToken cancellationToken)
            {
                var query = _categoryRepository.Query();

                if (!string.IsNullOrWhiteSpace(request.Name))
                    query = query.Where(x => request.Name.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase));

                query = request.OrderBy switch
                {
                    "NameASC" => query.OrderBy(x => x.Name),
                    "NameDESC" => query.OrderByDescending(x => x.Name),
                    "InsertTimeASC" => query.OrderBy(x => x.InsertTime),
                    "InsertTimeDESC" => query.OrderByDescending(x => x.InsertTime),
                    "UpdateTimeASC" => query.OrderBy(x => x.UpdateTime),
                    "UpdateTimeDESC" => query.OrderByDescending(x => x.UpdateTime),
                    _ => query.OrderByDescending(x => x.UpdateTime),
                };

                var items = await _categoryRepository.GetPagedListAsync(query, request.PaginationQuery);

                var getCategoryList = items.Adapt<GetCategoryDto>();

                return new SuccessDataResult<GetCategoryDto>(getCategoryList, Messages.SuccessfulOperation);
            }
        }
    }
}
