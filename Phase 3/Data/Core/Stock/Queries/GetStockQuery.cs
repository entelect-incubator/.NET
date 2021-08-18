﻿namespace Pezza.Core.Stock.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetStockQuery : IRequest<Result<StockDTO>>
    {
        public int Id { get; set; }
    }

    public class GetStockQueryHandler : IRequestHandler<GetStockQuery, Result<StockDTO>>
    {
        private readonly IDataAccess<StockDTO> DataAccess;

        public GetStockQueryHandler(IDataAccess<StockDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<StockDTO>> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAsync(request.Id);
            return Result<StockDTO>.Success(search);
        }
    }
}