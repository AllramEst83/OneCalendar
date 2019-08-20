using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Oops! Something went wrong. Please check the email adress.")]
        public string UserMail { get; set; }
    }
}
