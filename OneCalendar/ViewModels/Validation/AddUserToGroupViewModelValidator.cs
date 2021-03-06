﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels.Validation
{
    public class AddUserToGroupViewModelValidator : AbstractValidator<AddUserToGroupViewModel>
    {
        public AddUserToGroupViewModelValidator()
        {
            RuleFor(vm => vm.UserId).NotEmpty().WithMessage("UserId cannot be empty");
            RuleFor(vm => vm.GroupId).NotEmpty().WithMessage("GroupId cannot be empty");
        }
    }
}
