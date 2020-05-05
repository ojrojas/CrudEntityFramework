using System;
using System.Globalization;
using System.Windows.Controls;

namespace Site.Validations
{
    public class NotEmptyValidationRule : ValidationRule
    {
        private string _textValue;

        public string TextValue
        {
            get { return _textValue; }
            set { _textValue = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var Values = "";
            try
            {
                if (((string)value).Length < 0)
                    Values = (string)value
;
            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                return new ValidationResult(false, "El campo es requerido.");
            }
            if (string.IsNullOrWhiteSpace(((string)value)))
                return new ValidationResult(false, "El campo es requerido.");
            else
                return ValidationResult.ValidResult;

        }
    }
}
