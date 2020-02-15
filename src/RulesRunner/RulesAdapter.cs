using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NRules;
using NRules.Extensibility;
using NRules.Fluent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RulesRunner {
    public class RulesAdapter : IRulesAdapter {
        private readonly ILogger _logger;
        private readonly ISession _ruleSession;
        private readonly IDependencyResolver _dependencyResolver;
        public RulesAdapter(ILogger<RulesAdapter> logger, IDependencyResolver dependencyResolver) {
            _logger = logger;

            var ruleRepository = new RuleRepository();
            ruleRepository.Load(x => x.From(Assembly.GetExecutingAssembly()));
            _logger.LogDebug($"{ruleRepository.GetRules().Count()} rules loaded.");

            var ruleFactory = ruleRepository.Compile();
            _dependencyResolver = dependencyResolver;
            ruleFactory.DependencyResolver = _dependencyResolver;

            _ruleSession = ruleFactory.CreateSession();

            _ruleSession.Events.LhsExpressionEvaluatedEvent += Events_LhsExpressionEvaluatedEvent;
            _ruleSession.Events.LhsExpressionFailedEvent += Events_LhsExpressionFailedEvent;
            _ruleSession.Events.RhsExpressionEvaluatedEvent += Events_RhsExpressionEvaluatedEvent;
            _ruleSession.Events.RhsExpressionFailedEvent += Events_RhsExpressionFailedEvent;
        }

        private void Events_RhsExpressionFailedEvent(object sender, NRules.Diagnostics.RhsExpressionErrorEventArgs e) {
            _logger.LogTrace($"Rule: {e.Match.Rule.Name} RHSExpressionFailed: {e.Exception.Message}");
        }

        private void Events_RhsExpressionEvaluatedEvent(object sender, NRules.Diagnostics.RhsExpressionEventArgs e) {
            var result = e.Result == null ? string.Empty : e.Result.ToString();
            _logger.LogTrace($"Rule: {e.Match.Rule.Name} RHSExpressionEvaluated: {result} Fact: {JsonConvert.SerializeObject(e.Match.Facts.FirstOrDefault().Value)}");
        }

        private void Events_LhsExpressionFailedEvent(object sender, NRules.Diagnostics.LhsExpressionErrorEventArgs e) {
            _logger.LogTrace($"Rule: {e.Rules.FirstOrDefault().Name} LHSExpressionFailed: {e.Exception.Message}");
        }

        private void Events_LhsExpressionEvaluatedEvent(object sender, NRules.Diagnostics.LhsExpressionEventArgs e) {
            _logger.LogTrace($"Rule: {e.Rules.FirstOrDefault().Name} LHSExpressionEvaluated: {e.Result.ToString()} Fact: {JsonConvert.SerializeObject(e.Facts.FirstOrDefault())}");
        }

        public Task<int> Fire() {
            _logger.LogDebug($"Rule Session Firing");
            return Task.Run(() => _ruleSession.Fire());
        }

        public void Insert(IEnumerable<object> facts) {
            _ruleSession.InsertAll(facts);
            _logger.LogDebug($"Inserted {facts.Count()} {facts.GetType().Name} facts.");
        }

        public void Update(IEnumerable<object> facts) {
            _ruleSession.UpdateAll(facts);
            _logger.LogDebug($"Updated {facts.Count()} {facts.GetType().Name} facts.");

        }
    }
}