using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ClearCanvas.Common.Specifications;

namespace ClearCanvas.Enterprise.Core.Modelling
{
    /// <summary>
    /// Specifies minimum and maximum allowable length of the value of a given string-typed property of an object.
    /// </summary>
    public class LengthSpecification : SimpleInvariantSpecification
    {
        private int _min;
        private int _max;

        public LengthSpecification(PropertyInfo property, int min, int max)
            :base(property)
        {
            _min = min;
            _max = max;
        }

        public override TestResult Test(object obj)
        {
            object value = GetPropertyValue(obj);

            // ignore null values
            if (value == null)
                return new TestResult(true);

            try
            {
                string text = (string)value;

                return text.Length >= _min && text.Length <= _max ? new TestResult(true) :
                    new TestResult(false, new TestResultReason(GetMessage()));

            }
            catch (InvalidCastException e)
            {
                throw new SpecificationException(SR.ExceptionExpectedStringValue, e);
            }
        }

        private string GetMessage()
        {
            return string.Format(SR.RuleLength,
                TerminologyTranslator.Translate(this.Property), _min, _max);
        }
    }
}
