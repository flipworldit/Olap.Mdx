using System.Collections.Generic;
using System.Linq;
using SystemExt;
using LinqExtIn;

namespace OLAP.Mdx.MdxElements
{
    public class MdxWhereElement
        : IMdxElement
    {
        public UnionMdxElement Where;

        public MdxWhereElement Set(IEnumerable<IMdxElement> where)
        {
            Where = Where ?? new UnionMdxElement();

            AddChildren(where);
        
            return this;
        }

        public void Draw(MdxDrawContext dc)
        {
            if (Where == null || !Where.Measures.Any())
            {
                return;
            }

            dc.AppendLine("WHERE");

            Where.Draw(dc);
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>
            {
                Where 
            };
        }

        public void AddChildren(IEnumerable<IMdxElement> children)
        {
            if (children == null)
                return;

            var initWheres = Where.Measures
                .Where(el => el.GetType() == typeof(MdxValueElement)
                             || el.GetType() == typeof(TypedMdxElement))
                .ToArray();

            foreach (var child in children)
            {
                if (child.GetType() == typeof (MdxRangeElement))
                {
                    Where.Measures.Add(child);

                    continue;
                }

                var newWheres = GetMdxValueElements(child);

                var hie = newWheres[0].Name;

                var oldWheres = GetMdxValueElementsByHierarchyName(initWheres, hie);

                if (!oldWheres.Any())
                {
                    Where.Measures.Add(child);
             
                    continue;
                }

                var newWhereValues = newWheres
                    .Select(el => el.Value);

                var errorWheres = oldWheres
                    .Select(el => el.Value)
                    .Where(el => !newWhereValues.Contains(el))
                    .ToArray();

                if (errorWheres.Any())
                {
                    var errorValues = "(" + string.Join(", ", (IEnumerable<string>)errorWheres) + ")";

                    throw new EnableUserViewException(string.Format(
                        "Нарушение прав доступа. Недоступны значения {0}", errorValues));

                    //throw new Exception(string.Format(
                    //    "Нарушение прав доступа. Для иерархии {0} недоступны значения {1}", hie, errorValues));
                }

            }
        }

        private static List<MdxValueElement> GetMdxValueElements(IMdxElement initWhere)
        {
            var wheres = new List<MdxValueElement>();

            if (initWhere.GetType() == typeof(MdxValueElement))
            {
                wheres.Add((MdxValueElement)initWhere);

                return wheres;
            }

            if (initWhere.GetType() == typeof(TypedMdxElement))
            {
                var initWhereChildren = initWhere.GetChildren()
                    .Where(el => el.GetType() == typeof(MdxValueElement))
                    .ToArray();

                wheres.AddRange(initWhereChildren.Cast<MdxValueElement>());
            }
            return wheres;
        }

        private static List<MdxValueElement> GetMdxValueElementsByHierarchyName(IEnumerable<IMdxElement> children, string name)
        {
            var wheres = new List<MdxValueElement>();

            foreach (var child in children)
            {
                if (child.GetType() == typeof(MdxValueElement))
                {
                    if (((MdxValueElement)child).Name == name)
                    {
                        wheres.Add((MdxValueElement)child);

                        return wheres;
                    }
                }

                if (child.GetType() == typeof(TypedMdxElement))
                {
                    var initWhereChildren = child.GetChildren()
                        .Where(el => el.GetType() == typeof(MdxValueElement))
                        .Cast<MdxValueElement>()
                        .ToArray();

                    var byName = initWhereChildren
                        .Where(el => el.Name == name);

                    wheres.AddRange(byName);
                }
            }

            return wheres;
        }
    }
}