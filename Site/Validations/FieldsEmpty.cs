using System;
using System.Globalization;
using System.Windows.Controls;

namespace Site.Validations
{
    public class FieldsEmpty : ValidationRule
    {
        public FieldsEmpty()
        {

        }

        private int _min;

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int MaxLetters = 0;
            try
            {
                if (((string)value).Length > 0)
                    MaxLetters = int.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal caracters or " + e.Message);
            }

            if(MaxLetters < 0)
            {
                return new ValidationResult(false, "");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
