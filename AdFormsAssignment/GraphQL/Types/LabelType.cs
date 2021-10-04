using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormsAssignment.GraphQL
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
