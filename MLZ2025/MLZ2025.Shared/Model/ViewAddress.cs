using System.ComponentModel.DataAnnotations;

namespace MLZ2025.Shared.Model;

public class ViewAddress
{
    public static ViewAddress FromDatabaseAddress(DatabaseAddress address)
    {
        return new ViewAddress
        {
            FirstName = address.FirstName,
            LastName = address.LastName,
            ZipCode = address.ZipCode,
            Birthday = address.Birthday,
            Email = address.Email,
            Phone = address.Phone
        };
    }

    public static DatabaseAddress ToDatabaseAddress(ViewAddress address)
    {
        return new DatabaseAddress
        {
            FirstName = address.FirstName,
            LastName = address.LastName,
            ZipCode = address.ZipCode,
            Birthday = address.Birthday,
            Email = address.Email,
            Phone = address.Phone
        };
    }

    [Required, StringLength(50)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(50)]
    public string LastName { get; set; } = "";

    [Required, StringLength(10)]
    public string ZipCode { get; set; } = "";

    [Required]
    public DateTime Birthday { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, Phone]
    public string Phone { get; set; } = "";
}
