using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Http;

namespace AccountManagement.Application.Contracts.Account;

public class CreateAccount
{
    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Fullname { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Username { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Password { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Mobile { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = ValidationMessage.IsRequired)]
    public long RoleId { get; set; }

    public IFormFile ProfilePhoto { get; set; }
    public List<RoleViewModel> Roles { get; set; }
}