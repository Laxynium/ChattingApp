namespace InstantMessenger.Shared.Outbox
{
    internal sealed class OutboxOptions
    {
        public bool Enabled { get; set; }
        public int ExpiryHours { get; set; }
        public int CleanupIntervalHours { get; set; }
        public double IntervalMilliseconds { get; set; }
    }
}