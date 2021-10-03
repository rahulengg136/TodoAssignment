namespace AdFormsAssignment.Security
{
    /// <summary>
    /// Contract that deals with validation using JWT
    /// </summary>
    public interface IJwtAuth
    {
        /// <summary>
        /// Authentication method
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        string Authentication(string username, string password);
    }
}
