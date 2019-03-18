using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using OLAP.Mdx.MdxElements;
using LinqExtIn;

namespace OLAP.Mdx
{
    public class MdxBuilder : IMdxBuilder
    {
        /// <summary>
        /// MdxBuilder создавать только через декоратор
        /// </summary>
        public MdxBuilder()
        {
            _orderByDecoder = new OrderByParam(this);
        }

        private string _cube;

        public enum ValueFilter
        {
            Positive,
            Negative,
            All
        };

        private MdxDrawContext _dc;

        private MdxColumnElement _columns;
        private MdxRowElement _rows;
        private MdxWhereElement _where;
        private MdxWithElement _with;

        private readonly OrderByParam _orderByDecoder;

        public string CubeName
        {
            get { return _cube; }
        }

        public IMdxElement[] ColumnsGet
        {
            get
            {
                return _columns != null ? _columns.GetChildren().ToArray() : null;
            }
        }

        public MdxRowElement RowsGet
        {
            get
            {
                if (_rows != null)
                {
                    return _rows;
                }

                return null;
            }
        }

        public IMdxElement[] WhereGet
        {
            get
            {
                if (_where != null)
                {
                    var tmp = from t in _where.GetChildren()
                        from t1 in t.GetChildren()
                        select t1;
                    return tmp.ToArray();
                }

                return null;

            }
        }

        public IMdxElement[] WithGet
        {
            get
            {
                return _with != null ? _with.GetChildren().ToArray() : null;
            }
        }

        public IMdxBuilder Cube(string name)
        {
            _cube = name;

            return this;
        }

        public IMdxBuilder Columns(params IMdxElement[] columns)
        {
            _columns = _columns ?? new MdxColumnElement();

            _columns.Set(columns);

            return this;
        }

        public IMdxBuilder Columns(IEnumerable<IMdxElement> columns)
        {
            _columns = _columns ?? new MdxColumnElement();

            _columns.Set(columns);

            return this;
        }

        public IMdxBuilder Rows(params IMdxElement[] rows)
        {
            _rows = _rows ?? new MdxRowElement();

            _rows.Set(rows);

            return this;
        }

        public IMdxBuilder Rows(IEnumerable<IMdxElement> rows)
        {
            _rows = _rows ?? new MdxRowElement();

            _rows.Set(rows);

            return this;
        }

        public string OrderBy_MeasureOrDimension
        {
            get
            {
                return _orderByDecoder.MeasureOrDimension;
            }
        }

        public string OrderBy_Dir
        {
            get
            {
                return _orderByDecoder.Dir;
            }
        } 

        public string Build()
        {
            if (_where != null)
            {
                PreBuild();
            }

            _dc = new MdxDrawContext();

            if (_with != null)
            {
                _with.Draw(_dc);
            }

            _dc.AppendLine("SELECT");

            if (_columns != null)
            {
                _columns.Draw(_dc);
            }
            else
            {
                _dc.AppendLine("{}");
                _dc.AppendLine("ON COLUMNS,");
            }

            if (_columns != null && _rows != null&&!_rows.IsEmpty())
            {
                _dc.AppendLine(",");
            }
            else
            {
                _dc.EndOfLine();
            }

            if (_rows != null)
                _rows.Draw(_dc);

            DrawFrom();

            if (_where != null && _where.GetChildren().Any())
            {
                _dc.EndOfLine();

                _where.Draw(_dc);
            }

            return _dc.ToString();
        }

        private void PreBuild()
        {
            var deleteFromWhere = new List<IMdxElement>();

            var whereHierarhiesLists = GetWhereValues(_where);

            var hierarhiesList = GetHierarhiesList();

            foreach (var hierarchy in hierarhiesList)
            {
                CheckHierarchyForCurrentMember(hierarchy);

                List<IMdxElement> whereHierarhiesList;

                if (hierarchy.Name == null)
                {
                    throw new Exception("Значение иерархии пустое в списке '" +
                                        string.Join("',", hierarhiesList.Select(el => el.Name)) + "'");
                }

                if (!whereHierarhiesLists.TryGetValue(hierarchy.Name, out whereHierarhiesList))
                {
                    continue;
                }

                var replaceValue = GetReplaceValue(whereHierarhiesList);

                if (hierarchy.parentMdx != null)
                {
                    hierarchy.parentMdx.ReplaceChild(hierarchy.Name, replaceValue);
                }

                deleteFromWhere.AddRange(whereHierarhiesList);
            }

            _where.Remove(deleteFromWhere);
        }

        private static void CheckHierarchyForCurrentMember(MdxHierarchy hierarchyElement)
        {
            if (hierarchyElement._currentMember)
            {
                throw new Exception(String.Format(
                    "Ошибка в подстановке значения из условия. Значение CurrentMember не предусмотрено для замены. Иерархия - {0}",
                    hierarchyElement.Name));
            }
        }

        private static TypedMdxElement GetReplaceValue(List<IMdxElement> whereHierarhiesList)
        {
            var replaceValue = new TypedMdxElement();

            foreach (var whereHierarhies in whereHierarhiesList)
            {
                replaceValue._measures.Add(whereHierarhies);
            }
            return replaceValue;
        }

        private List<MdxHierarchy> GetHierarhiesList()
        {
            var hierarhiesList = new List<MdxHierarchy>();

            if (_rows != null)
            {
                hierarhiesList = GetHierarhies(_rows);
            }

            return hierarhiesList;
        }


        private static List<MdxHierarchy> GetHierarhies(IMdxElement rows)
        {
            var allHierarchies = LinqExtendedDescendans.DescendansAllLayerBFS(
                rows,
                element => element is MdxHierarchy,
                element => element.GetChildren())
                .Cast<MdxHierarchy>()
                .ToList();

            return allHierarchies;
        }
        //TODO: сделать internal

        private static Dictionary<string, List<IMdxElement>> GetWhereValues(IMdxElement whereMdx)
        {
            var allValues = LinqExtendedDescendans.DescendansAllLayerBFS(
                whereMdx,
                element => element.GetType() == typeof(MdxValueElement),
                element => element.GetChildren());

            var valuesByNames = allValues
                .GroupBy(el => ((MdxValueElement)el).Name)
                .ToDictionary(elements => elements.Key, elements => elements.ToList());

            return valuesByNames;
        }

        private void DrawFrom()
        {
            _dc.Append("FROM ");
            _dc.Append(_cube);
        }

        public IMdxBuilder Where(IEnumerable<IMdxElement> where)
        {
            if(where.Any())
            {
                _where = _where ?? new MdxWhereElement();

                _where.Set(where);
            }

            return this;
        }

        public IMdxBuilder With(params IMdxElement[] mdxMembers)
        {
            _with = _with ?? new MdxWithElement();

            _with.Set(mdxMembers);

            return this;
        }

        public IMdxBuilder With(IEnumerable<IMdxElement> mdxMembers)
        {
            if (mdxMembers == null)
            {
                return null;
            }

            _with = _with ?? new MdxWithElement();

            _with.Set(mdxMembers);

            return this;
        }

        public IMdxBuilder OrderBy(string measureOrDimension, string dir)
        {
            _rows.OrderBy(measureOrDimension, dir);

            return this;
        }

        public void RemoveLastRow()
        {
            if (_rows == null)
            {
                return;
            }

            var removeDimensions = _rows.RemoveLast();

            if (removeDimensions != null
                && removeDimensions.Length > 0
                && removeDimensions[0] is MdxValueElement)
            {
                var typedMdxElement = new TypedMdxElement(removeDimensions);

                _where.AddChildren(new[] { typedMdxElement });
            }
        }

        public class OrderByParam
        {
            private readonly MdxBuilder _builder;

            internal OrderByParam(MdxBuilder builder)
            {
                _builder = builder;
            }

            public string MeasureOrDimension
            {
                get
                {
                    if (_builder._rows != null)
                    {
                        var childrens = _builder._rows.GetChildren();

                        if (childrens.Count() == 1)
                        {
                            var firstChilde = _builder._rows.GetChildren().ElementAt(0);

                            if (firstChilde is MdxOrderBy)
                            {
                                return ((MdxOrderBy) firstChilde)._measureOrDimension;
                            }
                        }
                    }

                    return null;
                }
            }

            public string Dir
            {
                get
                {
                    if (_builder._rows != null)
                    {
                        var childrens = _builder._rows.GetChildren();

                        if (childrens.Count() == 1)
                        {
                            var firstChilde = _builder._rows.GetChildren().ElementAt(0);

                            if (firstChilde is MdxOrderBy)
                            {
                                return ((MdxOrderBy)firstChilde)._dir;
                            }
                        }
                    }

                    return null;
                }
            }
        }
    }
}
