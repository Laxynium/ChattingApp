using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class Permissions : ValueObject
    {
        public int Value { get; private set; }

        private Permissions(int value)
        {
            Value = value;
        }

        public static Permissions From(int value)=>new Permissions(value);

        public static Permissions Empty()=>new Permissions(0x0);

        public Permissions Add(Permission permission)
        {
            return new Permissions(Value | permission.Value);
        }

        public Permissions Remove(Permission permission)
        {
            return new Permissions(Value & ~permission.Value);
        }        
        public Permissions Add(Permissions permissions)
        {
            return new Permissions(Value | permissions.Value);
        }

        public Permissions Remove(Permissions permissions)
        {
            return new Permissions(Value & ~permissions.Value);
        }

        public bool Has(params Permission[] permissions) 
            => permissions.Aggregate(false, (current, permission) => current | (Value & permission.Value) == permission.Value);

        public IEnumerable<Permission> ToListOfPermissions() 
            => Permission.List.Where(p => this.Has(p)).ToArray();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}