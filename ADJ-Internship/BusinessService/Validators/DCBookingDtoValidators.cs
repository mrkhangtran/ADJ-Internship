using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Validators
{
  public class DCBookingDtoValidators : ValidationAttribute
  {
    private readonly string _otherProperty;

    public DCBookingDtoValidators(string otherProperty)
    {
      _otherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);
      if ((value != null) && (otherValue != null))
      {
        DateTime bookingDate = Convert.ToDateTime(value);
        DateTime arrivalDate = Convert.ToDateTime(otherValue);
        if (bookingDate < arrivalDate)
        {
          return new ValidationResult(ErrorMessage = "Booking Date Must Be Equal Or Later Than Arrival Date 30 days.");
        }
        if ((bookingDate - arrivalDate).TotalDays > 30)
        {
          return new ValidationResult(ErrorMessage = "Booking Date Must Be Equal Or Later Than Arrival Date 30 days.");
        }
      }
      return ValidationResult.Success;
    }
  }
}
