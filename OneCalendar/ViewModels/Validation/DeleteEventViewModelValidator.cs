using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels.Validation
{
    public class DeleteEventViewModelValidator : AbstractValidator<DeleteEventViewModel>
    {
        public DeleteEventViewModelValidator()
        {
            RuleFor(vm => vm.EventId).NotEmpty().WithMessage("EventId cannot be empty");
            RuleFor(vm => vm.GroupId).NotEmpty().WithMessage("GroupId cannot be empty");
        }
    }
}
