using GraphQL;
using GraphQL.Types;

namespace AdFormsAssignment.GraphQL
{
    /// <summary>
    /// GraphQL schema
    /// </summary>
    public class GraphQLSchema : Schema
    {
        /// <summary>
        /// GraphQL schema
        /// </summary>
        public GraphQLSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<Queries>();
        }
    }
}
