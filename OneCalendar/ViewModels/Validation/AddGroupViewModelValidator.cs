using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels.Validation
{
    public class AddGroupViewModelValidator : AbstractValidator<AddCalenderGroupViewModel>
    {
        public AddGroupViewModelValidator()
        {
            RuleFor(vm => vm.GroupName).NotEmpty().WithMessage("GroupName cannot be empty");
            RuleFor(vm => vm.GroupUsers).NotEmpty().WithMessage("GroupUsers cannot be empty");
        }
    }
}
