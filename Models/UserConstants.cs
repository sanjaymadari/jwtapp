namespace JwtApp.Models;

public class UserConstants
{
    public static List<UserModel> Users = new List<UserModel>
    {
        new UserModel() { Username = "sanjaymadari", EmailAddres = "jason.admin@gmail.com", Password = "sanju@27",
        GivenName = "Jason", Surname = "Bryant", Role = "Administrator" },
        new UserModel() { Username = "jason_user", EmailAddres = "snajay@gmail.com", Password = "MyPass_w0rd1",
        GivenName = "Jason1", Surname = "Bryant1", Role = "Seller" },

    };
}

