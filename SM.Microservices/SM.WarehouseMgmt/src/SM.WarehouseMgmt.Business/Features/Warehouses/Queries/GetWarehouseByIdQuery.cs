using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.WarehouseMgmt.Business.Dtos.Warehouses;
using SM.WarehouseMgmt.DataAccess.Abstract;

namespace SM.WarehouseMgmt.Business.Features.Warehouses.Queries
{
    public class GetWarehouseByIdQuery : IRequest<IResult>
    {
        public long Id { get; set; }
        public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, IResult>
        {
            private readonly IWarehouseRepository _warehouseRepository;

            public GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
            {
                _warehouseRepository = warehouseRepository;
            }

            public async Task<IResult> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
            {
                var warehouse = await _warehouseRepository.GetAsync(x => x.Id == request.Id);
                if (warehouse == null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                var getWarehouseDto = warehouse.Adapt<GetWarehouseDto>();

                return new SuccessDataResult<GetWarehouseDto>(getWarehouseDto, Messages.SuccessfulOperation);
            }
        }
    }
}
