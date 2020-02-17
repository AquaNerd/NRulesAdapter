using System;

namespace RulesRunner.Model {
    public class RuleCondition : ICondition {
        public string TransactionCode { get; set; }
        public string CompanyCode { get; set; }
        public string Status { get; set; }
        public ComparisonOperator SalesAmountOperator { get; set; }
        public decimal SalesAmount { get; set; }
    }

    //PRO: using a separate 'helpers' class there's a separation between the Condition and Fact objects (keeping these two classes clean POCOs)
    //CON: the Helper(s) classes could become unruly and hard to manage. 
    public static class RuleConditionHelper {
        public static bool SalesOrderComparison(this RuleCondition condition, TransactionFact fact) {
            bool result = false;
            switch (condition.SalesAmountOperator) {
                case ComparisonOperator.Equal:
                    result = condition.SalesAmount.Equals(fact.SalesAmount);
                    break;
                case ComparisonOperator.GreaterThan:
                    result = condition.SalesAmount < fact.SalesAmount;
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    result = condition.SalesAmount <= fact.SalesAmount;
                    break;
                case ComparisonOperator.LessThan:
                    result = condition.SalesAmount > fact.SalesAmount;
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    result = condition.SalesAmount >= fact.SalesAmount;
                    break;
            }

            return result;
        }

        public static bool TransactionCodeComparison(this RuleCondition condition, TransactionFact fact) {
            return condition.TransactionCode.Equals(fact.TransactionCode);
        }

        public static bool CompanyCodeComparison(this RuleCondition condition, TransactionFact fact) {
            if (condition.CompanyCode.Equals("all", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return condition.CompanyCode.Equals(fact.Company, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}