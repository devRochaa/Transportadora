using System.Net;
using Transportadora.Application.Order.Requests;
using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Order.Services;

public sealed class CreateOrderService(IRepository<OrderEntity> repository, IUnitOfWork unitOfWork) : ServiceBase<OrderEntity>(repository, unitOfWork)
{
    //public async Task<Guid> ExecuteAsync(CreateOrderRequest req, CancellationToken cancellationToken)
    //{

    //    //var order = new OrderEntity
    //    //{
    //    //    DestinataryId = req.DestinaryId,
    //    //    TotalAmount = req.TotalAmount,
    //    //    WeightCategory = req.WeightCategory,
    //    //    //DestinyZipCode = req.DestinyZipCode,
    //    //    OriginId = req.OriginId,
    //    //    OriginAddress = req.OriginAddress
    //    //};
    //    //await unitOfWork.BeginTransactionAsync(cancellationToken);
    //    //await repository.AddAsync(order, cancellationToken);
    //    //await unitOfWork.SaveChangesAsync(cancellationToken);
    //    //await unitOfWork.CommitAsync(cancellationToken);
    //    //return order.Id;
    //}
}
