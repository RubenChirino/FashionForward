using System;
namespace FashionForward.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Active { get; set; }
    public int Verify { get; set; }

    public User(int id, string email, string password, int active, int verify)
    {
        Id = id;
        Email = email;
        Password = password;
        Active = active;
        Verify = verify;
    }

    public User(int id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}