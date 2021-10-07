using GraphQL;
using GraphQL.Types;

namespace AdFormsAssignment.GraphQL
{
    /// <summary>
    /// GraphQL schema
    /// </summary>
    public class GraphQlSchema : Schema
    {
        /// <summary>
        /// GraphQL schema
        /// </summary>
        public GraphQlSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<Queries>();
        }
    }
}
