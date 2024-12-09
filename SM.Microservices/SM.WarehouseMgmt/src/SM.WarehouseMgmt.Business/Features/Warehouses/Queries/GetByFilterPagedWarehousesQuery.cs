using Mapster;
using MediatR;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Paging;
using SM.Core.Utilities.Results;
using SM.WarehouseMgmt.Business.Dtos.Warehouses;
using SM.WarehouseMgmt.DataAccess.Abstract;

namespace SM.WarehouseMgmt.Business.Features.Warehouses.Queries
{
    public class GetByFilterPagedWarehousesQuery : IRequest<IResult>
    {
        public string Name { get; set; }
        public string OrderBy { get; set; }
        public PaginationQuery PaginationQuery { get; set; }

        public class GetByFilterPagedWarehousesQueryHandler : IRequestHandler<GetByFilterPagedWarehousesQuery, IResult>
        {
            private readonly IWarehouseRepository _warehouseRepository;

            public GetByFilterPagedWarehousesQueryHandler(IWarehouseRepository warehouseRepository)
            {
                _warehouseRepository = warehouseRepository;
            }

            public async Task<IResult> Handle(GetByFilterPagedWarehousesQuery request, CancellationToken cancellationToken)
            {
                var query = _warehouseRepository.Query();

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

                var items = await _warehouseRepository.GetPagedListAsync(query, request.PaginationQuery);

                var getWarehouseList = items.Adapt<GetWarehouseDto>();

                return new SuccessDataResult<GetWarehouseDto>(getWarehouseList, Messages.SuccessfulOperation);
            }
        }
    }
}
