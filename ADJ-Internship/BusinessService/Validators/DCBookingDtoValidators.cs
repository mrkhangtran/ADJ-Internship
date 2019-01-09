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
          return new ValidationResult(ErrorMessage = "Booking Date must be equal to or later than Arrival Date and cannot be 30 days apart.");
        }
        if ((bookingDate - arrivalDate).TotalDays > 30)
        {
          return new ValidationResult(ErrorMessage = "Booking Date must be equal to or later than Arrival Date and cannot be 30 days apart.");
        }
      }
      return ValidationResult.Success;
    }
  }
}
