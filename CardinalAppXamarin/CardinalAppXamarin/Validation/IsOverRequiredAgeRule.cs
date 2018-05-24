using CardinalAppXamarin.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardinalAppXamarin.Validation
{
    public class IsOverRequiredAgeRule : IValidationRule<DateTime>
    {
        public string ValidationMessage { get; set; }

        public int RequiredYears { get; set; }

        public bool Check(DateTime value)
        {
            if (RequiredYears <= 0)
            {
                return true;
            }
            return DateTime.Now.AddYears(0 - RequiredYears) > value;
        }
    }
}