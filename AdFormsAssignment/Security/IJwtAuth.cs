namespace AdFormsAssignment.Security
{
    public interface IJwtAuth
    {
        string Authentication(string username, string password);
    }
}
