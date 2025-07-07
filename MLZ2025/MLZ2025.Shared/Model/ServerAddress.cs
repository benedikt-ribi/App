namespace MLZ2025.Core.Model;

public class ServerAddress
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string ZipCode { get; set; }
    public required DateTime Birthday { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
}
