namespace Pezza.Core.Notify.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateNotifyCommand : IRequest<Notify>
    {
        public int CustomerId { get; set; }

        public string Email { get; set; }

        public bool Sent { get; set; }

        public int Retry { get; set; }
    }

    public class CreateNotifyCommandHandler : IRequestHandler<CreateNotifyCommand, Notify>
    {
        private readonly INotifyDataAccess dataAcess;

        public CreateNotifyCommandHandler(INotifyDataAccess dataAcess) 
            => this.dataAcess = dataAcess;

        public async Task<Notify> Handle(CreateNotifyCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(new Notify
            {
                CustomerId = request.CustomerId,
                Email = request.Email,
                Sent = request.Sent,
                Retry = request.Retry,
                DateSent = DateTime.Now
            });
    }
}
