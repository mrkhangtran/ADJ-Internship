using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ADJ.BusinessService.Dtos;
using FluentValidation;

namespace ADJ.BusinessService.Validators
{
  class CreateOrUpdatePurchaseOrderRqValidator : AbstractValidator<CreateOrUpdatePurchaseOrderRq>
  {
    public CreateOrUpdatePurchaseOrderRqValidator()
    {
      RuleFor(x => x.Test).NotEmpty();
    }
  }

  public class PortIsDifferent : ValidationAttribute
  {
    private readonly string _otherProperty;

    public PortIsDifferent(string otherProperty)
    {
      _otherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
    {
      var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);

      if ((value != null) && (otherValue != null))
      {
        if (value.ToString().CompareTo(otherValue.ToString()) == 0)
        {
          return new ValidationResult(ErrorMessage = "Ports cannot be the same");
        }
      }

      return ValidationResult.Success;
    }
  }

  public class SimilarOrLaterThanOtherDateAttribute : ValidationAttribute
  {
    private readonly string _otherProperty;

    public SimilarOrLaterThanOtherDateAttribute(string otherProperty)
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
        if (date1 < date2)
        {
          return new ValidationResult(ErrorMessage = "Cannot be before " + _otherProperty.ToString());
        }
      }

      return ValidationResult.Success;
    }
  }

  public class Not30DaysApartAttribute : ValidationAttribute
  {
    private readonly string _otherProperty;

    public Not30DaysApartAttribute(string otherProperty)
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
        if ((date1 - date2).Days > 30)
        {
          return new ValidationResult(ErrorMessage = "Cannot be more than 30 days apart");
        }
      }

      return ValidationResult.Success;
    }
  }

  public class NotInThePast : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      if (value != null)
      {
        DateTime date1 = Convert.ToDateTime(value);
        DateTime date2 = DateTime.Now.Date;
        if (date1 < date2)
        {
          return false;
        }
      }

      return true;
    }
  }
}
