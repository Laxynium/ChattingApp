using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class RolePriority : SimpleValueObject<int>
    {
        public static readonly RolePriority Lowest = new RolePriority(-1);

        private RolePriority(int value) : base(value)
        {
        }

        public static bool operator <(RolePriority a, RolePriority b)
        {
            if (a == Lowest)
                return b != Lowest;
            return a.Value < b.Value;
        }

        public static bool operator >(RolePriority a, RolePriority b)
        {
            if (b == Lowest)
                return a != Lowest;
            return a.Value > b.Value;
        }

        public static RolePriority CreateFirst() => Create(0);

        public static RolePriority Create(int value)
        {
            if (value < 0)
                throw new ArgumentException($"User defined role priority must be greater or equal 0");
            return new RolePriority(value);
        }

        public RolePriority Increased() => Create(Value + 1);
    }
}