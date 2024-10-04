using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Categories;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<IResult>
    {
        public long Id { get; set; }

        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, IResult>
        {
            private readonly ICategoryRepository _categoryRepository;

            public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<IResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                var category = await _categoryRepository.GetAsync(x => x.Id == request.Id);
                if (category == null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                var getCategoryDto = category.Adapt<GetCategoryDto>();

                return new SuccessDataResult<GetCategoryDto>(getCategoryDto, Messages.SuccessfulOperation);
            }
        }
    }
}
