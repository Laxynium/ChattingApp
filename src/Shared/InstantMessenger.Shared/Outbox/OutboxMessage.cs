using System;

namespace InstantMessenger.Shared.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public DateTime OccurredOn { get; set; }
        
        public string Data { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public OutboxMessage(Guid id, DateTime occurredOn,  string data)
        {
            this.Id = id;
            this.OccurredOn = occurredOn;
            this.Data = data;
        }

        private OutboxMessage()
        {
        }
    }
}