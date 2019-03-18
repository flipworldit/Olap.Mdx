using System.Collections.Generic;
using OLAP.Mdx;

namespace AuixilaryxMdxBuilder
{
    public class MdxBuilderArrayCreator
    {
       public static IEnumerable<string> ToMdxQueriesForExcel(IMdxBuilder builder)
        {
            var isEmpty = false;

            while (!isEmpty)
            {
                isEmpty = builder.RowsGet.IsEmpty(); 
                
                var build = builder.Build();

                builder.RemoveLastRow();

                yield return build;
            }
        }
    }
}