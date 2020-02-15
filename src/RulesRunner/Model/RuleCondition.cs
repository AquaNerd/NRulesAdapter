namespace RulesRunner.Model {
    public class RuleCondition : ICondition {
        public string TransactionCode { get; set; }
        public string CompanyCode { get; set; }
        public string Status { get; set; }
        public ComparisonOperator SalesAmountOperator { get; set; }
        public decimal SalesAmount { get; set; }
    }
}