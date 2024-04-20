// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Entity;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    public DateTime LastLogin { get; private set; }
    public bool IsDefaultAdmin { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; private set; } = "Default";
    public string UpdateBy { get; private set; } = "Default";

    public ApplicationUser()
    {

    }

    public ApplicationUser(string username, 
        string firstName, 
        string lastname, 
        string email, 
        bool isDefaultAdmin = false, 
        bool isActive = false)
    {
        UserName = username;
        FirstName = firstName;
        Lastname = lastname;
        Email = email;
        EmailConfirmed = false;
        PhoneNumberConfirmed = false;
        IsDefaultAdmin = isDefaultAdmin;
        IsActive = isActive;
    }

    public void SetCreatedBy(string username) => CreatedBy = username;
    public void SetUpdatedBy(string username) => UpdateBy = username;
    public void SetUpdationTime() => UpdatedAt = DateTime.UtcNow;
    public void SetLastLogin() => LastLogin = DateTime.UtcNow;
}
