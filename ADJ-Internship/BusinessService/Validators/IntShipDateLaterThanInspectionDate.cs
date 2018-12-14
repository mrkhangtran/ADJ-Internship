using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
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
                if (IntDate.CompareTo(InspecDate) <0) { 
                    return new ValidationResult(ErrorMessage = " Int Ship Date must be equal or greater than Inspection Date");
                }
                if (IntDate.CompareTo(today)<0) {
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
}