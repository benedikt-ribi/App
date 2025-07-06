using SQLite;

namespace MLZ2025.Shared.Model;

[Table("Address")]
public class DatabaseAddress
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string ZipCode { get; set; } = "";

    public DateTime Birthday { get; set; } = DateTime.Today;

    public string Email { get; set; } = "";

    public string Phone { get; set; } = "";
}
