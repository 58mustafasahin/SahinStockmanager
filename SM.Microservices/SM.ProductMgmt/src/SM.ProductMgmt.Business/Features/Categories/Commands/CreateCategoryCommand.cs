using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.ProductMgmt.Business.Dtos.Categories;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.Business.Features.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, IResult>
        {
            private readonly ICategoryRepository _categoryRepository;

            public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<IResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = request.Adapt<Category>();
                _categoryRepository.Add(category);
                await _categoryRepository.SaveChangesAsync();

                var getCategoryDto = category.Adapt<GetCategoryDto>();

                return new SuccessDataResult<GetCategoryDto>(getCategoryDto, Messages.SuccessfulOperation);
            }
        }
    }
}
