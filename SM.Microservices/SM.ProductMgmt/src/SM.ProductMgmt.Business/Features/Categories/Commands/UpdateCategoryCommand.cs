using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Categories;
using SM.ProductMgmt.DataAccess.Abstract;

namespace SM.ProductMgmt.Business.Features.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<IResult>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IResult>
        {
            private readonly ICategoryRepository _categoryRepository;

            public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<IResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var currentCategory = await _categoryRepository.GetAsync(x => x.Id == request.Id);
                if (currentCategory is null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                currentCategory = request.Adapt(currentCategory);
                _categoryRepository.Update(currentCategory);
                await _categoryRepository.SaveChangesAsync();

                var getCategoryDto = currentCategory.Adapt<GetCategoryDto>();

                return new SuccessDataResult<GetCategoryDto>(getCategoryDto, Messages.SuccessfulOperation);
            }
        }
    }
}
