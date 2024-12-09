using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.WarehouseMgmt.Business.Dtos.Warehouses;
using SM.WarehouseMgmt.DataAccess.Abstract;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.Business.Features.Warehouses.Commands
{
    public class CreateWarehouseCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ResponsiblePerson { get; set; }

        public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, IResult>
        {
            private readonly IWarehouseRepository _warehouseRepository;

            public CreateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
            {
                _warehouseRepository = warehouseRepository;
            }

            public async Task<IResult> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
            {
                var warehouse = request.Adapt<Warehouse>();
                _warehouseRepository.Add(warehouse);
                await _warehouseRepository.SaveChangesAsync();

                var getWarehouseDto = warehouse.Adapt<GetWarehouseDto>();

                return new SuccessDataResult<GetWarehouseDto>(getWarehouseDto, Messages.SuccessfulOperation);
            }
        }
    }
}
