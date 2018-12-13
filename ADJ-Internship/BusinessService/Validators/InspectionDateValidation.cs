using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Validators
{
    public class InspectionDateValidation : ValidationAttribute
    {
        private readonly string _otherProperty;

        public InspectionDateValidation(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);

            if ((value != null) && (otherValue != null))
            {
                DateTime InspecDate = Convert.ToDateTime(value);
                DateTime IntDate = Convert.ToDateTime(otherValue);
                DateTime today = Convert.ToDateTime(DateTime.Now.Date);
                if (InspecDate.CompareTo(IntDate) > 1)
                {
                    return new ValidationResult(ErrorMessage = " Inspection Date Date must be equal or less than Int Ship Date");
                }
                if (InspecDate.CompareTo(today) < 1)
                {
                    return new ValidationResult(ErrorMessage = " Inspection Date must be equal or  greater than Today ");
                }
            }
            return ValidationResult.Success;
        }
    }
}
