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

        public override void Define() {
            TransactionFact fact = null;
            IEnumerable<RuleCondition> conditions = null;
            Console.WriteLine($"Triggered: {this.GetType().Name}");

            Dependency()
                .Resolve(() => _mediator);

            When()
                .Match<TransactionFact>(() => fact)
                .Match<RuleCondition>(
                        c => c.TransactionCode.Equals("all", StringComparison.InvariantCultureIgnoreCase) ? true : c.TransactionCode.Equals(fact.TransactionCode, StringComparison.InvariantCultureIgnoreCase),
                        c => c.CompanyCode.Equals("all", StringComparison.InvariantCultureIgnoreCase) ? true : c.CompanyCode.Equals(fact.Company, StringComparison.InvariantCultureIgnoreCase),
                        c => (
                            c.SalesAmountOperator.Equals(ComparisonOperator.Equal) ? c.SalesAmount.Equals(fact.SalesAmount) :
                            c.SalesAmountOperator.Equals(ComparisonOperator.GreaterThan) ? c.SalesAmount < fact.SalesAmount :
                            c.SalesAmountOperator.Equals(ComparisonOperator.GreaterThanOrEqual) ? c.SalesAmount <= fact.SalesAmount :
                            c.SalesAmountOperator.Equals(ComparisonOperator.LessThan) ? c.SalesAmount > fact.SalesAmount :
                            c.SalesAmountOperator.Equals(ComparisonOperator.LessThanOrEqual) ? c.SalesAmount >= fact.SalesAmount : false
                        )
                    );

            Then()
                .Do(ctx => SetNotifiedUser(_mediator, fact));
        }
        private void SetNotifiedUser(IMediator mediator, TransactionFact fact) {
            var command = new PingCommand();
            var results = mediator.Send(command).GetAwaiter().GetResult();
            Console.WriteLine($"PingCommand Results: {results}");
            fact.NotifiedUserInitials = "UserInitials";
        }
    }
}