using System.Collections.Generic;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public abstract class UsageCounter : ValueObject
    {
        public string Type { get; }
        protected UsageCounter() : base() { }

        protected UsageCounter(string type)
        {
            Type = type;
        }

        public abstract bool CanUse();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
        }

        public abstract UsageCounter Use();
    }

    public class InfiniteUsageCounter : UsageCounter
    {
        public InfiniteUsageCounter() : base(nameof(InfiniteUsageCounter))
        {
        }

        public override bool CanUse()
        {
            return true;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }

        public override UsageCounter Use()
        {
            return this;
        }
    }

    public class BoundedUsageCounter : UsageCounter
    {
        public int Value { get; }
        public BoundedUsageCounter(int value) : base(nameof(BoundedUsageCounter))
        {
            if (value < 0)
                throw new InvalidUsageCounterException();
            Value = value;
        }

        public override bool CanUse()
        {
            return Value != 0;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override UsageCounter Use()
        {
            if (Value == 0)
                throw new InvalidInvitationException();
            return new BoundedUsageCounter(Value - 1);
        }
    }
    public class UsageCounterContainer
    {
        private UsageCounterType _type;
        private int? _value;

        public UsageCounter UsageCounter
        {
            get
            {
                return _type switch
                {
                    UsageCounterType.Bounded => new BoundedUsageCounter(_value.Value),
                    UsageCounterType.Infinite => new InfiniteUsageCounter(),
                    _ => throw new InvalidUsageCounterException()
                };
            }
            set
            {
                switch (value)
                {
                    case BoundedUsageCounter cuc:
                        _type = UsageCounterType.Bounded;
                        _value = cuc.Value;
                        break;
                    case InfiniteUsageCounter iuc:
                        _type = UsageCounterType.Infinite;
                        _value = null;
                        break;
                }
            }
        }
    }
    public enum UsageCounterType
    {
        Infinite,
        Bounded
    }
}