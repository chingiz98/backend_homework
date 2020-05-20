using System;
using System.Threading.Tasks;
using BackendHomework.Commands;
using MassTransit;

namespace BackendHomework.BusinessLogic.User
{
    public class UpdateUserInfoRequestHandler
    {
        private readonly IBus _bus;
        public UpdateUserInfoRequestHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(Guid id, string username, string name)
        {
            return _bus.Send(new UpdateUserInfoCommand
            {
                Id = id,
                Username = username,
                Name = name
            });
        }
    }
}