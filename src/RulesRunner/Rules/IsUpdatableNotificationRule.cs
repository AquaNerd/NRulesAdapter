using NRules.Fluent.Dsl;
using RulesRunner.Model;

namespace RulesRunner.Rules {
    [Name("IsUpdatableNotification")]
    [Priority(0)]
    public class IsUpdatableNotificationRule : Rule {
        public override void Define() {
            TransactionFact notification = null;

            When()
                .Match(() => notification);

            Filter()
                 .OnChange(() => notification.NotifiedUserInitials);

            Then()
                .Do(ctx => ctx.Update(notification, SetIsUpdatableNotification));
        }

        private void SetIsUpdatableNotification(TransactionFact notification) {
            // notification cannot be updated if the notified user is set
            notification.IsUpdatableNotification = string.IsNullOrWhiteSpace(notification.NotifiedUserInitials);
        }
    }
}