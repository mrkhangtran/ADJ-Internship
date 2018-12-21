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
          if ((InspecDate.Date-IntDate.Date).TotalDays > 0)
          {
            return new ValidationResult(ErrorMessage = " Inspection Date Date must be equal or less than Int Ship Date");
          }
          if ((InspecDate.Date - today.Date).TotalDays < 0)
          {
            return new ValidationResult(ErrorMessage = " Inspection Date must be equal or greater than Today ");
          }
        }
        return ValidationResult.Success;
      }
    }
    public class IntShipDateLaterThanInspectionDate : ValidationAttribute
    {
      private readonly string _otherProperty;

      public IntShipDateLaterThanInspectionDate(string otherProperty)
      {
        _otherProperty = otherProperty;
      }
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
        var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);

        if ((value != null) && (otherValue != null))
        {
          DateTime IntDate = Convert.ToDateTime(value);
          DateTime InspecDate = Convert.ToDateTime(otherValue);
          DateTime today = Convert.ToDateTime(DateTime.Now.Date);
          if ((IntDate.Date - InspecDate.Date).TotalDays < 0)
          {
            return new ValidationResult(ErrorMessage = " Int Ship Date must be equal or greater than Inspection Date");
          }
          if ((IntDate.Date - today.Date).TotalDays < 0)
          {
            return new ValidationResult(ErrorMessage = " Int Ship Date must be equal or  greater than Today ");
          }
          if ((IntDate.Date - InspecDate).TotalDays > 30)
          {
            return new ValidationResult(ErrorMessage = " Int Ship Date must be  greater than Inspection Date 30 days ");

          }
        }
        return ValidationResult.Success;
      }
    }
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
          decimal Quantity = decimal.Parse(otherValue.ToString());
          decimal Revise = decimal.Parse(value.ToString());
          if (Revise < 0)
          {
            return new ValidationResult(ErrorMessage = " Invalid value.Please try again ");
          }
          if (Revise > Quantity)
          {
            return new ValidationResult(ErrorMessage = " Invalid value.Please try again");
          }
        }
        return ValidationResult.Success;
      }
    }

  }
