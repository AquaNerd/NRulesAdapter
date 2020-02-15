using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RulesRunner.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RulesRunner {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly IRulesAdapter _rulesAdapter;
        private List<TransactionFact> _facts;
        private List<RuleCondition> _ruleConditions;

        public Worker(ILogger<Worker> logger, IRulesAdapter rulesAdapter) {
            _logger = logger;
            _rulesAdapter = rulesAdapter;
            _facts = new List<TransactionFact>();
            _ruleConditions = new List<RuleCondition>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            _logger.LogInformation("Starting Service");

            DataSetup();

            _rulesAdapter.Insert(_facts);
            _rulesAdapter.Insert(_ruleConditions);
            _rulesAdapter.Update(_facts);
            var results = _rulesAdapter.Fire().GetAwaiter().GetResult();

            await Task.Delay(1000);

            _facts.ForEach(f => _logger.LogDebug(JsonConvert.SerializeObject(f)));
        }

        private void DataSetup() {
            _facts.Add(new TransactionFact {
                TransactionCode = "1",
                Company = "WMT",
                Status = "Not",
                NotifiedUserInitials = "",
                Detail = "Bibbidy Bobbity Boop",
            });
            _facts.Add(new TransactionFact {
                TransactionCode = "2",
                Company = "AAPL",
                Status = "Ready",
                NotifiedUserInitials = "",
                Detail = "No Go!",
            });
            _facts.Add(new TransactionFact {
                TransactionCode = "3",
                Company = "GO",
                Status = "Ready",
                NotifiedUserInitials = "",
                Detail = "Abracadabra! Poof!"
            });
            _facts.Add(new TransactionFact {
                TransactionCode = "4",
                Company = "IBM",
                Status = "Ready",
                NotifiedUserInitials = "",
                Detail = "You've got the right to fight to parrrrtttaaayyy!"
            });
            _facts.Add(new TransactionFact {
                TransactionCode = "4",
                Company = "IGC",
                Status = "Ready",
                NotifiedUserInitials = "",
                Detail = "No Go!"
            });

            _ruleConditions.Add(new RuleCondition {
                TransactionCode = "1",
                CompanyCode = "AAPL",
                Status = "Ready",
                SalesAmountOperator = ComparisonOperator.GreaterThanOrEqual,
                SalesAmount = decimal.Zero
            });
            _ruleConditions.Add(new RuleCondition {
                TransactionCode = "2",
                CompanyCode = "FCS",
                Status = "Ready",
                SalesAmountOperator = ComparisonOperator.GreaterThanOrEqual,
                SalesAmount = decimal.Zero
            });
            _ruleConditions.Add(new RuleCondition {
                TransactionCode = "3",
                CompanyCode = "WMT",
                Status = "Not",
                SalesAmountOperator = ComparisonOperator.GreaterThanOrEqual,
                SalesAmount = decimal.Zero
            });
            _ruleConditions.Add(new RuleCondition {
                TransactionCode = "4",
                CompanyCode = "ALL",
                Status = "Ready",
                SalesAmountOperator = ComparisonOperator.LessThanOrEqual,
                SalesAmount = 400.00m
            });
        }
    }
}