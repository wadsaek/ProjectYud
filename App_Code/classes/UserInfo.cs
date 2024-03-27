/// <summary>
/// Summary description for UserInfo
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PfpAdress { get; set; }
    public UserInfo(int id, string usern, string mail, string pfp)
    {
        PfpAdress = pfp;
        Id = id;
        UserName = usern;
        Email = mail;
    }
    private UserInfo()
    {
        Id = -1;
        UserName = "";
        Email = "";
    }
    public static UserInfo Empty = new UserInfo();
}