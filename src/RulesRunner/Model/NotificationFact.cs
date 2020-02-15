namespace RulesRunner.Model {
    public class NotificationFact : IFact {
        public string Company { get; set; }
        public string Status { get; set; }
        public decimal SalesAmount { get; set; }
        public bool IsUpdatableNotification { get; set; }
        public string NotifiedUserInitials { get; set; }
    }
}