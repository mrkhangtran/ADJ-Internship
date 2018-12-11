using System;
using System.Collections.Generic;
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
}
