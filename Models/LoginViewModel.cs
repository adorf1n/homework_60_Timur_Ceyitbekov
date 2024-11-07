using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Логин")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }
}