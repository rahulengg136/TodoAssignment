namespace AdFormsAssignment.DTO.Common
{
    /// <summary>
    /// Entity to return in case of few common scenarios
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Message
        /// </summary>
        public virtual string Message { get; set; }
    }
    /// <summary>
    /// Entity to return in case of bad request
    /// </summary>
    public class BadRequestInfo : ResponseModel
    {
    }
    /// <summary>
    /// Entity to return in case of unauthorized access
    /// </summary>
    public class UnauthorizedInfo : ResponseModel
    {
    }
    /// <summary>
    /// Entity to return in user is validated and need token
    /// </summary>
    public class ValidatedUserToken : ResponseModel
    {
    }
}
