using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.WarehouseMgmt.Business.Dtos.Warehouses;
using SM.WarehouseMgmt.DataAccess.Abstract;

namespace SM.WarehouseMgmt.Business.Features.Warehouses.Commands
{
    public class UpdateWarehouseCommand : IRequest<IResult>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ResponsiblePerson { get; set; }

        public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, IResult>
        {
            private readonly IWarehouseRepository _warehouseRepository;

            public UpdateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
            {
                _warehouseRepository = warehouseRepository;
            }

            public async Task<IResult> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
            {
                var currentWarehouse = await _warehouseRepository.GetAsync(x => x.Id == request.Id);
                if (currentWarehouse is null)
                    return new ErrorResult(Messages.RecordDoesNotExist);

                currentWarehouse = request.Adapt(currentWarehouse);
                _warehouseRepository.Update(currentWarehouse);
                await _warehouseRepository.SaveChangesAsync();

                var getWarehouseDto = currentWarehouse.Adapt<GetWarehouseDto>();

                return new SuccessDataResult<GetWarehouseDto>(getWarehouseDto, Messages.SuccessfulOperation);
            }
        }
    }
}
