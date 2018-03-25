using System.ComponentModel.DataAnnotations;

namespace GlobalisationAndLocalisationDotNetCore.ViewModels.Info
{
    public class InfoViewModel
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

    }
}
