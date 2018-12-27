using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Validators
{
  public class LaterThanOtherDateAttribute : ValidationAttribute
  {
    private readonly string _otherProperty;

    public LaterThanOtherDateAttribute(string otherProperty)
    {
      _otherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
    {
      var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);

      if ((value != null) && (otherValue != null))
      {
        DateTime date1 = Convert.ToDateTime(value);
        DateTime date2 = Convert.ToDateTime(otherValue);
        if (date1 <= date2)
        {
          return new ValidationResult(ErrorMessage = "Must be after " + _otherProperty.ToString());
        }
        if ((date1 - date2).Days > 30)
        {
          return new ValidationResult(ErrorMessage = "Cannot be more than 30 days apart");
        }
      }

      return ValidationResult.Success;
    }
  }
}
