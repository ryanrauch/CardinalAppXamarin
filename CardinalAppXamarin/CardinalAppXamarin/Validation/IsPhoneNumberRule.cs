using CardinalAppXamarin.Validation.Interfaces;
using System;

namespace CardinalAppXamarin.Validation
{
    public class IsPhoneNumberRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if(value == null)
            {
                return false;
            }
            var str = value as string;
            var formatted = str.Replace("-","");
            formatted = formatted.Replace("(", "");
            formatted = formatted.Replace(")", "");
            if(formatted.Length != 10)
            {
                return false;
            }
            foreach(Char c in formatted)
            {
                if(!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
