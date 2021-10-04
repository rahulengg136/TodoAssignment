using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
