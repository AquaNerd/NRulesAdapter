using MediatR;
using RulesRunner.Data;
using System.Threading;
using System.Threading.Tasks;

namespace RulesRunner.cqrs {
    public class GetNotifiedUserInitials : IRequest<NotificationInitial> {
        public string Department { get; }
        public GetNotifiedUserInitials(string department) {
            Department = department;
        }

        public class GetNotifiedUserCommandHandler : IRequestHandler<GetNotifiedUserInitials, NotificationInitial> {
            private readonly MyDbContext _dbContext;

            public GetNotifiedUserCommandHandler(MyDbContext dbContext) {
                _dbContext = dbContext;
            }

            public async Task<NotificationInitial> Handle(GetNotifiedUserInitials request, CancellationToken cancellationToken = default) {
                return await _dbContext.GetNotificationInitial(request.Department);
            }
        }
    }
}