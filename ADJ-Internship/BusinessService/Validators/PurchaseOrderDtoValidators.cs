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

    /*class UniquePONumberValidator: AbstractValidator<OrderDTO>
    {
        /*public bool UniquePONum(string PONumber, int? id)
        {
            if (id == null || id == 0)
            {
                return !db.GetDB().Orders.Any(x => x.PONumber == PONumber);
            }
            else
            {
                return !db.GetDB().Orders.Any(x => x.PONumber == PONumber && x.Id != id);
            }
        }
    }*/
    

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
                    return new ValidationResult(ErrorMessage = "Cannot be before Ship Date");
                }
            }

            return ValidationResult.Success;
        }
    }
}
