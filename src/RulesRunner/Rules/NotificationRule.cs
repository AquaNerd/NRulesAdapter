using MediatR;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using RulesRunner.cqrs;
using RulesRunner.Model;
using System;
using System.Collections.Generic;

namespace RulesRunner.Rules {
    [Name("UserNotificationRule")]
    [Priority(1)]
    // ensure the rule runs only once per match per update
    [Repeatability(RuleRepeatability.NonRepeatable)]
    public class NotificationRule : Rule {
        private readonly IMediator _mediator;

        public NotificationRule() {
            //NOTE: moved dep resolution into default ctor to avoid issues with methods needing to be called in a specific order.
            Dependency().Resolve(() => _mediator);
        }

        public override void Define() {
            TransactionFact fact = null;
            IEnumerable<RuleCondition> conditions = null;
            Console.WriteLine($"Triggered: {this.GetType().Name}");

            //Dependency()
            //    .Resolve(() => _mediator);

            When()
                .Match<TransactionFact>(() => fact)
                .Match<RuleCondition>(
                    c => c.TransactionCodeComparison(fact),
                    c => c.CompanyCodeComparison(fact),
                    c => c.SalesOrderComparison(fact)

                    //c => _transactionCodeComparison(c, fact),
                    //c => _companyCodeComparison(c, fact),
                    //c => _salesOrderComparison(c, fact)
                );

            Then()
                .Do(ctx => SetNotifiedUser(fact));
        }

        //Mediator probably doesn't need to be passed into this method since it's an 'instance method'
        //although if the resolving of the dependency in done in the Define() the order in the method calls create
        //an issue requiring a specific order of the method calls.
        private void SetNotifiedUser(TransactionFact fact) {
            var command = new PingCommand();
            var results = _mediator.Send(command).GetAwaiter().GetResult();
            Console.WriteLine($"PingCommand Results: {results}");
            fact.NotifiedUserInitials = "UserInitials";
        }

        #region using Func
        //private readonly Func<RuleCondition, TransactionFact, bool> _salesOrderComparison =
        //    (condition, fact) => {
        //        bool result = false;
        //        switch (condition.SalesAmountOperator) {
        //            case ComparisonOperator.Equal:
        //                result = condition.SalesAmount.Equals(fact.SalesAmount);
        //                break;
        //            case ComparisonOperator.GreaterThan:
        //                result = condition.SalesAmount < fact.SalesAmount;
        //                break;
        //            case ComparisonOperator.GreaterThanOrEqual:
        //                result = condition.SalesAmount <= fact.SalesAmount;
        //                break;
        //            case ComparisonOperator.LessThan:
        //                result = condition.SalesAmount > fact.SalesAmount;
        //                break;
        //            case ComparisonOperator.LessThanOrEqual:
        //                result = condition.SalesAmount >= fact.SalesAmount;
        //                break;
        //        }

        //        return result;
        //    };

        //private readonly Func<RuleCondition, TransactionFact, bool> _transactionCodeComparison =
        //    (condition, fact) => condition.TransactionCode.Equals(fact.TransactionCode);

        //private readonly Func<RuleCondition, TransactionFact, bool> _companyCodeComparison =
        //    (condition, fact) => {
        //        if (condition.CompanyCode.Equals("all", StringComparison.InvariantCultureIgnoreCase))
        //            return true;

        //        return condition.CompanyCode.Equals(fact.Company, StringComparison.InvariantCultureIgnoreCase);
        //    };
        #endregion
    }
}