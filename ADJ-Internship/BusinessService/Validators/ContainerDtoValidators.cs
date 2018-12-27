using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Validators
{
  public class ContainerDtoValidators
  {
    public class ShipQuantityValidation : ValidationAttribute
    {
      private readonly string _otherProperty;

      public ShipQuantityValidation(string otherProperty)
      {
        _otherProperty = otherProperty;
      }

      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
        var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);
        if ((value != null) && (otherValue != null))
        {
          decimal shipQuantity = decimal.Parse(value.ToString());
          decimal bookQuantity = decimal.Parse(otherValue.ToString());
          if (shipQuantity > bookQuantity)
          {
            return new ValidationResult(ErrorMessage = "Ship Quantity is invalid, please try again");
          }
        }
        return ValidationResult.Success;
      }
    }
  }
}
