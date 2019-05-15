using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels.Validation
{
    public class DeleteGroupViewModelValidator : AbstractValidator<DeleteGroupViewModel>
    {
        public DeleteGroupViewModelValidator()
        {
            RuleFor(vm => vm.Groups).NotEmpty().WithMessage("Groups cannot be empty");
        }
    }
}
