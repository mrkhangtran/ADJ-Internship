using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Validators
{
	public class ContainerDtoValidators
	{
		public class QuantityValidation : ValidationAttribute
		{
			private readonly string _otherProperty;

			public QuantityValidation(string otherProperty)
			{
				_otherProperty = otherProperty;
			}

			protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			{
				var otherValue = validationContext.ObjectType.GetProperty(_otherProperty).GetValue(validationContext.ObjectInstance, null);
				if ((value != null) && (otherValue != null))
				{
					decimal shipQuantity = decimal.Parse(value.ToString());
					decimal openQuantity = decimal.Parse(otherValue.ToString());
					if (shipQuantity > openQuantity)
					{
						return new ValidationResult(ErrorMessage = "Quantity is invalid, please try again");
					}
				}
				return ValidationResult.Success;
			}
		}
	}
}