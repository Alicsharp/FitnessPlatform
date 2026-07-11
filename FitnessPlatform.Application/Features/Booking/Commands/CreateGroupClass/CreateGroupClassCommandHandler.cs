using FitnessPlatform.Domain.Booking;
using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Booking.Commands.CreateGroupClass
{
    public class CreateGroupClassCommandHandler : IRequestHandler<CreateGroupClassCommand, Guid>
    {
        private readonly IGroupClassRepository _repository;

        public CreateGroupClassCommandHandler(IGroupClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateGroupClassCommand request, CancellationToken cancellationToken)
        {
            var groupClass = GroupClass.Create(request.Title, request.StartTime, request.MaxCapacity);

            await _repository.AddAsync(groupClass, cancellationToken);

            return groupClass.Id;
        }
    }
}
