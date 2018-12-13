using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Validators
{
    public class ReviseQuantityvalidate : ValidationAttribute
    {
        private readonly string _otherProperty;

        public ReviseQuantityvalidate(string otherProperty)
        {
            _otherProperty = otherProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);

            if ((value != null) && (otherValue != null))
            {
                int Quantity =int.Parse(otherValue.ToString());
                int Revise = int.Parse(value.ToString());             
                if (Revise < 0)
                {
                    return new ValidationResult(ErrorMessage = " Revise must be greater or equal zero ");
                }
                if (Revise > Quantity)
                {
                    return new ValidationResult(ErrorMessage = " Revise must be less or equal Quantity ");
                }
            }
            return ValidationResult.Success;
        }
    }
}
