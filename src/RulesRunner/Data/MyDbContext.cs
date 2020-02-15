using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RulesRunner.Data {
    public class MyDbContext : DbContext {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {
        }

        public async Task<NotificationInitial> GetNotificationInitial(string department) {
            switch (department.ToLowerInvariant()) {
                case "apsinitials":
                    return new NotificationInitial { UserInitials = "APS" };
                case "casemgrinitials":
                    return new NotificationInitial { UserInitials = "CSE" };
                default:
                    return new NotificationInitial { UserInitials = "DFT" };
            }
        }
    }
}