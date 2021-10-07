using AdFormAssignment.DAL.Entities;
using GraphQL.Types;

namespace AdFormsAssignment.GraphQL.Types
{
    /// <summary>
    /// GraphQL type : Label
    /// </summary>
    public class LabelType : ObjectGraphType<TblLabel>
    {
        /// <summary>
        /// GraphQL type : Label
        /// </summary>
        public LabelType()
        {
            Field(x => x.LabelId);
            Field(x => x.LabelName);
        }
    }
}
