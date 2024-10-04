using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Categories;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<IResult>
    {
        public long Id { get; set; }

        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IResult>
        {
            private readonly ICategoryRepository _categoryRepository;

            public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<IResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var currentCategory = await _categoryRepository.GetAsync(x => x.Id == request.Id);
                if (currentCategory is null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                _categoryRepository.Delete(currentCategory);
                await _categoryRepository.SaveChangesAsync();

                var getCategoryDto = currentCategory.Adapt<GetCategoryDto>();

                return new SuccessDataResult<GetCategoryDto>(getCategoryDto, Messages.SuccessfulOperation);
            }
        }
    }
}
