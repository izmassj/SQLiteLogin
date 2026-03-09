using System;
using SQLite4Unity3d;

[Serializable]
[Table("Users")]
public class User
{
    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }

    [Unique, NotNull]
    public string Username { get; set; }

    [NotNull]
    public string Password { get; set; }

    [NotNull]
    public string CreatedDate { get; set; }
}
