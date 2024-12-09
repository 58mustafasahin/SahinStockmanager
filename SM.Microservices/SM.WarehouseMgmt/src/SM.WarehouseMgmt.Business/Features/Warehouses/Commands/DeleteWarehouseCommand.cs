using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.WarehouseMgmt.Business.Dtos.Warehouses;
using SM.WarehouseMgmt.DataAccess.Abstract;
namespace SM.WarehouseMgmt.Business.Features.Warehouses.Commands
{
    public class DeleteWarehouseCommand : IRequest<IResult>
    {
        public long Id { get; set; }

        public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, IResult>
        {
            private readonly IWarehouseRepository _warehouseRepository;

            public DeleteWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
            {
                _warehouseRepository = warehouseRepository;
            }

            public async Task<IResult> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
            {
                var currentWarehouse = await _warehouseRepository.GetAsync(x => x.Id == request.Id);
                if (currentWarehouse is null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                _warehouseRepository.Delete(currentWarehouse);
                await _warehouseRepository.SaveChangesAsync();

                var getWarehouseDto = currentWarehouse.Adapt<GetWarehouseDto>();

                return new SuccessDataResult<GetWarehouseDto>(getWarehouseDto, Messages.SuccessfulOperation);
            }
        }
    }
}
