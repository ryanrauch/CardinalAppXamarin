using CardinalAppXamarin.Validation.Interfaces;

namespace CardinalAppXamarin.Validation
{
    public class IsEmailAddressRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if(value == null)
            {
                return false;
            }
            var str = value as string;
            if(str.Contains("@"))
            {
                var split = str.Split('@');
                return split[0].Length > 0
                       && split[1].Contains(".")
                       && !split[1].EndsWith(".")
                       && !split[1].StartsWith(".");
            }
            return false;
        }
    }
}
