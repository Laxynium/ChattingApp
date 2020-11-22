using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public abstract class ExpirationTime : ValueObject
    {
        public DateTimeOffset Start { get; }

        protected ExpirationTime() { }

        protected ExpirationTime( DateTimeOffset start)
        {
            Start = start;
        }
        public abstract bool IsExpired(DateTimeOffset now);
    }

    public class InfiniteExpirationTime : ExpirationTime
    {
        public InfiniteExpirationTime(DateTimeOffset start) : base(start)
        {
        }

        public override bool IsExpired(DateTimeOffset now)
        {
            return false;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { };
        }
    }

    public class BoundedExpirationTime : ExpirationTime
    {
        public TimeSpan Period { get; }

        public BoundedExpirationTime(DateTimeOffset start, TimeSpan period) : base(start)
        {
            Period = period;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Period;
        }

        public override bool IsExpired(DateTimeOffset now)
        {
            if (now < Start)
                throw new ArgumentException();
            var diff = now - Start;
            return diff > Period;
        }
    }

    public enum ExpirationTimeType
    {
        Infinite,
        Bounded
    }

    public class ExpirationTimeContainer
    {
        private ExpirationTimeType _type;
        private DateTimeOffset _start;
        private TimeSpan? _period;

        public ExpirationTime ExpirationTime
        {
            get
            {
                return _type switch
                {
                    ExpirationTimeType.Infinite => new InfiniteExpirationTime(_start),
                    ExpirationTimeType.Bounded => new BoundedExpirationTime(_start,_period.Value),
                    _ => throw new InvalidExpirationTimeException()
                };
            }
            set
            {
                ((Action)(value switch
                {
                    InfiniteExpirationTime x => () =>
                    {
                        _type = ExpirationTimeType.Infinite;
                        _start = x.Start;
                        _period = null;
                    },
                    BoundedExpirationTime x => () =>
                    {
                        _type = ExpirationTimeType.Bounded;
                        _start = x.Start;
                        _period = x.Period;
                    },
                    _ => ()=>{}
                })).Invoke();
            }
        }
    }
}