using System;
using System.ComponentModel.DataAnnotations;

namespace CORE.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class Default : Attribute
    {
        #region Properties

        public bool IsConstructorCall { get; private set; }

        public object[] Values { get; private set; }

        #endregion

        #region Constructor

        public Default()
        {
            SetDefault(true);
        }

        public Default(object value)
        {
            SetDefault(false, value);
        }

        public Default(Type type)
        {
            if (typeof(DateTime) == type)
                SetDefault(false, DateTime.Now);
            else
                SetDefault(false, Activator.CreateInstance(type));
        }

        public Default(Type type, object value)
        {
            SetDefault(false, Activator.CreateInstance(type, value));
        }

        #endregion

        #region Private Methods

        private void SetDefault(bool isConstructorCall, params object[] values)
        {
            IsConstructorCall = isConstructorCall;
            Values = values ?? new object[0];
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class PrimaryKey : Attribute
    {
        public string Sequence { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class Identity : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class NotNull : RequiredAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class MaxLength : MaxLengthAttribute
    {
        public MaxLength(int length) : base(length) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class MinLength : MinLengthAttribute
    {
        public MinLength(int length) : base(length) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class Length : StringLengthAttribute
    {
        public Length(int length) : base(length) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class Between : RangeAttribute
    {
        public Between(int minimum, int maximum) : base(minimum, maximum) { }
        public Between(double minimum, double maximum) : base(minimum, maximum) { }
        public Between(Type type, string minimum, string maximum) : base(type, minimum, maximum) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class Phone : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return new PhoneAttribute().IsValid(value);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class Email : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class RegularExpression : RegularExpressionAttribute
    {
        public RegularExpression(string pattern) : base(pattern) { }
    }
}
