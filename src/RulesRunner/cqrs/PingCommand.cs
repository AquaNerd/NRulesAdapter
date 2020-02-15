using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace RulesRunner.cqrs {
    public class PingCommand : IRequest<string> {
        public class PingHandler : IRequestHandler<PingCommand, string> {
            public Task<string> Handle(PingCommand request, CancellationToken cancellationToken) {
                return Task.FromResult("Pong");
            }
        }
    }
}