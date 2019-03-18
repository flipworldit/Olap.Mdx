using System.Collections.Generic;
using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx
{
    public interface IMdxBuilder
    {
        string CubeName { get; }
        IMdxElement[] ColumnsGet { get; }
        MdxRowElement RowsGet { get; }
        IMdxElement[] WhereGet { get; }
        IMdxElement[] WithGet { get; }
        string OrderBy_MeasureOrDimension { get; }
        string OrderBy_Dir { get; }

        IMdxBuilder Cube(string name);

        IMdxBuilder Columns(params IMdxElement[] columns);
        IMdxBuilder Columns(IEnumerable<IMdxElement> columns);


        IMdxBuilder Rows(params IMdxElement[] rows);
        IMdxBuilder Rows(IEnumerable<IMdxElement> rows);

        string Build();

        IMdxBuilder Where(IEnumerable<IMdxElement> @where);

        IMdxBuilder With(params IMdxElement[] mdxMembers);
        IMdxBuilder With(IEnumerable<IMdxElement> mdxMembers);

        IMdxBuilder OrderBy(string measureOrDimension, string dir);

        void RemoveLastRow();

    }
}