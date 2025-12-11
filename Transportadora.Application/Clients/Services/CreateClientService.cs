using Transportadora.Application.Clients.Requests;
using Transportadora.Domain.Abstractions;
using Transportadora.Domain.Entities;

namespace Transportadora.Application.Clients.Services;

public sealed class CreateClientService
{
    private readonly IRepository<PersonEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientService(IRepository<PersonEntity> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> ExecuteAsync(CreateClientRequest req, CancellationToken cancellationToken)
    {
        if (await _repository.AnyAsync(c => c.NationalDocument == req.NationalDocument))
            throw new InvalidOperationException("Cliente já cadastrado.");

        var client = new PersonEntity
        {
            Fullname = req.Fullname,
            NationalDocument = req.NationalDocument,
            Phone = req.Phone,
            BirthDate = DateOnly.FromDateTime(req.BirthDate)
        };
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _repository.AddAsync(client, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return client.Id;
    }
}
