using FluentValidation;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels.Validation
{
    public class UpdateEventResponseModelValidator: AbstractValidator<AddEventViewModel>
    {
        public UpdateEventResponseModelValidator()
        {
            RuleFor(vm => vm.UserId).NotEmpty().WithMessage("UserId cannot be empty");
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("UserId cannot be empty");
            RuleFor(vm => vm.Start).NotEmpty().WithMessage("Start cannot be empty");
            RuleFor(vm => vm.End).NotEmpty().WithMessage("End cannot be empty");
            RuleFor(vm => vm.GroupId).NotEmpty().WithMessage("GroupId cannot be empty");
            RuleFor(vm => vm.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(vm => vm.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(vm => vm.EventColor).NotEmpty().WithMessage("EventColor cannot be empty");

        }
    }
}
