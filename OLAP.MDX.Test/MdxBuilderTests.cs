using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemExt;
using AuixilaryxMdxBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OLAP.Mdx;
using OLAP.Mdx.Common;
using OLAP.Mdx.MdxElements;
using Assert = NUnit.Framework.Assert;


namespace Dashboard.OLAP.Mdx.Tests
{
    [TestClass]
    public class MdxBuilderTests
    {
        [TestMethod]
        [TestCategory("CI")]
        public void BuildManyMeasureQueryTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .Columns(new TypedMdxElement("Amount",
                    "Price",
                    "Weight"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[Weight]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void SubsetTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .Columns(new TypedMdxElement("Weight"))
                .Rows(new MdxSubset(new TypedMdxElement(new MdxHierarchy("Contacts")), 0, 100));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Weight]
    }
ON COLUMNS,
SubSet(
    {
        Contacts
    }, 0, 100)
ON ROWS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("hasChild",
                        new MdxDistinctCount(new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Name]").Children())))
                .Columns(new TypedMdxElement(
                    "Amount",
                    "Price",
                    "hasChild"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[hasChild] AS
        DistinctCount ([Ent Contacts].[Ent MainManagers - Name].children)
SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[hasChild]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }


        [TestMethod]
        [TestCategory("CI")]
        public void BuildWithTest2()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("hasChild",
                        new MdxDistinctCount(new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Name]").Children())))
                .Columns(new TypedMdxElement("Amount",
                    "Price",
                    "hasChild"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[hasChild] AS
        DistinctCount ([Ent Contacts].[Ent MainManagers - Name].children)
SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[hasChild]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildPeriodsToDateWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("PeriodsToDate",
                        new MdxSum(
                            new MdxPeriodsToDate(
                                new MdxHierarchy("[Dates].[Календарная].[Cal Month]"),
                                new MdxHierarchy("[Dates].[Календарная]").CurrentMember()),
                            "Weight")))
                .Columns(new TypedMdxElement("PeriodsToDate"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[PeriodsToDate] AS
        Sum (PeriodsToDate ([Dates].[Календарная].[Cal Month], [Dates].[Календарная].CurrentMember), [Measures].[Weight])
SELECT
    {
        [Measures].[PeriodsToDate]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildSumWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("SumReturns",
                        new MdxSum(
                            new MdxHierarchy("[DeliveryType].[Name].&[Возврат]"),
                            "SumPrice"))
                )
                .Columns(new TypedMdxElement("SumReturns"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[SumReturns] AS
        Sum ([DeliveryType].[Name].&[Возврат], [Measures].[SumPrice])
SELECT
    {
        [Measures].[SumReturns]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildPrirostWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("SumPrirost",
                        new MdxPrirost(
                            new MdxHierarchy("[Dates].[Date]").CurrentMember(),
                            new MdxMeasureElement("SumPrice")))
                )
                .Columns(new TypedMdxElement("SumPrirost"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[SumPrirost] AS
        [Measures].[SumPrice] - Sum ([Dates].[Date].CurrentMember.PREVMEMBER, [Measures].[SumPrice])
SELECT
    {
        [Measures].[SumPrirost]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildPrirostPercentWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("SumPrirostPercent",
                        new MdxPrirostPercent(
                            new MdxHierarchy("[Dates].[Date]").CurrentMember(),
                            new MdxMeasureElement("SumPrice")))
                )
                .Columns(new TypedMdxElement("SumPrirostPercent"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[SumPrirostPercent] AS
        Divide ([Measures].[SumPrice] - Sum ([Dates].[Date].CurrentMember.PREVMEMBER, [Measures].[SumPrice]), [Measures].[SumPrice], 0)
SELECT
    {
        [Measures].[SumPrirostPercent]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildMovAvgWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("AvgMov",
                        new MdxMovAvg(
                            new MdxHierarchy("[Dates].[Date]").CurrentMember(),
                            new MdxMeasureElement("Weight"),
                            29))
                )
                .Columns(new TypedMdxElement("AvgMov"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[AvgMov] AS
        Sum (
            [Dates].[Date].CurrentMember.Lag(28) :
            [Dates].[Date].CurrentMember.Lag(0),
            [Measures].[Weight]
        )/29
SELECT
    {
        [Measures].[AvgMov]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

       
        [TestMethod]
        [TestCategory("CI")]
        public void BuildManyWithTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember("PeriodsToDate",
                        new MdxSum(
                            new MdxPeriodsToDate(
                                new MdxHierarchy("[Dates].[Календарная].[Cal Month]"),
                                new MdxHierarchy("[Dates].[Календарная]").CurrentMember()),
                            "Weight")),
                    new MdxMember("hasChild",
                        new MdxDistinctCount(
                            new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Name]").Children())),
                    new MdxMember("SumReturns",
                        new MdxSum(
                            new MdxHierarchy("[DeliveryType].[Name].&[Возврат]"),
                            "SumPrice")),
                    new MdxMember("SumPrirost",
                        new MdxPrirost(
                            new MdxHierarchy("[Dates].[Date]").CurrentMember(),
                            new MdxMeasureElement("SumPrice"))),
                    new MdxMember("SumPrirostPercent",
                        new MdxPrirostPercent(
                            new MdxHierarchy("[Dates].[Date]").CurrentMember(),
                            new MdxMeasureElement("SumPrice"))),
                    new MdxMember("AvgMov",
                        new MdxMovAvg(
                            new MdxHierarchy("[Dates].[Date]").CurrentMember(),
                            new MdxMeasureElement("Weight"),
                            29)))
                .Columns(new TypedMdxElement("Amount",
                    "SumPrice",
                    "SumReturns",
                    "SumPrirost",
                    "SumPrirostPercent",
                    "AvgMov",
                    "PeriodsToDate",
                    "hasChild"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[PeriodsToDate] AS
        Sum (PeriodsToDate ([Dates].[Календарная].[Cal Month], [Dates].[Календарная].CurrentMember), [Measures].[Weight])
    MEMBER [Measures].[hasChild] AS
        DistinctCount ([Ent Contacts].[Ent MainManagers - Name].children)
    MEMBER [Measures].[SumReturns] AS
        Sum ([DeliveryType].[Name].&[Возврат], [Measures].[SumPrice])
    MEMBER [Measures].[SumPrirost] AS
        [Measures].[SumPrice] - Sum ([Dates].[Date].CurrentMember.PREVMEMBER, [Measures].[SumPrice])
    MEMBER [Measures].[SumPrirostPercent] AS
        Divide ([Measures].[SumPrice] - Sum ([Dates].[Date].CurrentMember.PREVMEMBER, [Measures].[SumPrice]), [Measures].[SumPrice], 0)
    MEMBER [Measures].[AvgMov] AS
        Sum (
            [Dates].[Date].CurrentMember.Lag(28) :
            [Dates].[Date].CurrentMember.Lag(0),
            [Measures].[Weight]
        )/29
SELECT
    {
        [Measures].[Amount],
        [Measures].[SumPrice],
        [Measures].[SumReturns],
        [Measures].[SumPrirost],
        [Measures].[SumPrirostPercent],
        [Measures].[AvgMov],
        [Measures].[PeriodsToDate],
        [Measures].[hasChild]
    }
ON COLUMNS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }


        [TestMethod]
        [TestCategory("CI")]
        public void BuildReportByPeriodsQueryTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            const string fieldMember = "field";
            const string fieldAllMember = "fieldAll";
            const string measure = "Weight";
            const string periodType = "[Dates].[Date]";
            const string begin = "2016-06-20T00:00:00";
            const string end = "2016-06-24T00:00:00";
            const string selectGroup = "[Ent Contacts].[Ent FirstLevels - Name]";

            builder
                .Cube("Dashboard")
                .With(
                    new MdxMember(fieldMember, measure),
                    new MdxMember(fieldAllMember,
                        new MdxSum(
                            new MdxRangeElement(
                                new MdxHierarchy(periodType).Value(begin),
                                new MdxHierarchy(periodType).Value(end)),
                            fieldMember)))
                .Columns(
                    new TypedMdxElement(
                        new UnionMdxElement(
                            new MdxHierarchy(periodType),
                            new MdxMeasureElement(fieldAllMember)),
                        new UnionMdxElement(
                            new MdxRangeElement(
                                new MdxHierarchy(periodType).Value(begin),
                                new MdxHierarchy(periodType).Value(end)),
                            new MdxMeasureElement(fieldMember))))
                .Rows(
                    new MdxNonEmpty(
                        new TypedMdxElement(
                            new MdxHierarchy(selectGroup).Children(),
                            new MdxHierarchy(selectGroup)
                        )));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"WITH
    MEMBER [Measures].[field] AS
        [Measures].[Weight]
    MEMBER [Measures].[fieldAll] AS
        Sum ([Dates].[Date].&[2016-06-20T00:00:00]:[Dates].[Date].&[2016-06-24T00:00:00], [Measures].[field])
SELECT
    {
                (
            [Dates].[Date],
            [Measures].[fieldAll]
        ),
                (
            [Dates].[Date].&[2016-06-20T00:00:00]:[Dates].[Date].&[2016-06-24T00:00:00],
            [Measures].[field]
        )
    }
ON COLUMNS,
NON EMPTY (
    {
        [Ent Contacts].[Ent FirstLevels - Name].children,
        [Ent Contacts].[Ent FirstLevels - Name]
    })
ON ROWS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildManyMeasureAndRowsQueryTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .Columns(
                    new TypedMdxElement("Amount",
                        "Price",
                        "Weight"))
                .Rows(new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]").Children());

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[Weight]
    }
ON COLUMNS,
[Ent Contacts].[Ent MainManagers - Id].children
ON ROWS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildManyMeasureAndTwoRowsQueryTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .Columns(
                    new TypedMdxElement("Amount",
                        "Price",
                        "Weight"))
                .Rows(new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]").Children(),
                    new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]"));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[Weight]
    }
ON COLUMNS,
{
    [Ent Contacts].[Ent MainManagers - Id].children,
    [Ent Contacts].[Ent MainManagers - Id]
}
ON ROWS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void BuildManyMeasureAndTwoRowsNonEmptyQueryTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .Columns(
                    new TypedMdxElement(
                        "Amount",
                        "Price",
                        "Weight"))
                .Rows(new MdxNonEmpty(
                    new TypedMdxElement(
                        new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]").Children(),
                        new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]"))));

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[Weight]
    }
ON COLUMNS,
NON EMPTY (
    {
        [Ent Contacts].[Ent MainManagers - Id].children,
        [Ent Contacts].[Ent MainManagers - Id]
    })
ON ROWS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void NonEmptyHierarchyTest()
        {
            var builder = new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]")
                .NotEmpty();

            var dc = new MdxDrawContext();

            builder.Draw(dc);
            var mdxQuery = dc.ToString();

            var expectedQuery = @"NON EMPTY (
[Ent Contacts].[Ent MainManagers - Id])";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void HierarchyValueTest()
        {
            var builder = new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]")
                .Value("175");

            var dc = new MdxDrawContext();

            builder.Draw(dc);
            var mdxQuery = dc.ToString();

            var expectedQuery = "[Ent Contacts].[Ent MainManagers - Id].&[175]";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void HierarchyValueLagTest()
        {
            var builder = new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]")
                .Value("175")
                .Lag(2);

            var dc = new MdxDrawContext();

            builder.Draw(dc);
            var mdxQuery = dc.ToString();

            var expectedQuery = "[Ent Contacts].[Ent MainManagers - Id].&[175].Lag(2)";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void HierarchyRangeTest()
        {
            var builder = new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]")
                .Range("175", "190");

            var dc = new MdxDrawContext();

            builder.Draw(dc);
            var mdxQuery = dc.ToString();

            var expectedQuery =
                "[Ent Contacts].[Ent MainManagers - Id].&[175]:[Ent Contacts].[Ent MainManagers - Id].&[190]";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void WhereTest()
        {
            var builder = new MdxWhereElement()
                .Set(
                    new List<IMdxElement>
                    {
                        new MdxHierarchy("[Ent FirstLevels - Name]")
                            .Value("Мотовилов Андрей Александрович"),
                        new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                            .Value("1. КИСЛОМОЛОЧНАЯ группа")
                    }
                );

            var dc = new MdxDrawContext();

            builder.Draw(dc);
            var mdxQuery = dc.ToString();

            var expectedQuery =
                @"WHERE
(
    [Ent FirstLevels - Name].&[Мотовилов Андрей Александрович],
    [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа]
)";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void OrderTest()
        {
            var builder = TestMdxBuilderFactory.CreateBuilder();

            builder
                .Cube("Dashboard")
                .Rows( //new MdxNonEmpty(
                    new MdxHierarchy("[Delivery].[Delivery Key]")
                        .Children()
                    //)
                )
                .OrderBy("[Delivery].[Date]", "asc")
                ;

            var columns = new TypedMdxElement("Weight");

            builder
                .Columns(columns);

            var mdxQuery = builder.Build();

            Debug.Write(mdxQuery);

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Weight]
    }
ON COLUMNS,
NON EMPTY(
[Delivery].[Delivery Key].children
,
    ORDER
    (
        ([Delivery].[Date].children),
        [Delivery].[Date].CurrentMember.Properties(""Key""),
        asc
    )
)
ON ROWS
FROM Dashboard";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void IntegrationTest()
        {
            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    .Columns(
                        new TypedMdxElement("Amount",
                            "Price",
                            "Weight"))
                    .Rows(
                        new UnionMdxElement(
                            new MdxHierarchy("[Ent Contacts].[Ent FirstLevels - Id]")
                                .Value("175"),
                            new MdxHierarchy("[Ent Contacts].[Ent MainManagers - Id]")
                                .Children()
                                .NotEmpty()))
                    .Where(
                        new List<IMdxElement>
                        {
                            new MdxHierarchy("[Ent Contacts].[Подчинения].[Ent FirstLevels - Name]")
                                .Value("Мотовилов Андрей Александрович"),
                            new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                .Value("1. КИСЛОМОЛОЧНАЯ группа"),
                            new MdxHierarchy("[Ent Goods].[Ent BKG - Name]")
                                .Value("ММЛ ЗЕЛЕНЫЕ"),
                            new MdxHierarchy("[Dates].[Date]")
                                .Range("2014-08-23T00:00:00", "2014-08-31T00:00:00")
                        });

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount],
        [Measures].[Price],
        [Measures].[Weight]
    }
ON COLUMNS,
(
    [Ent Contacts].[Ent FirstLevels - Id].&[175],
    NON EMPTY     (
[Ent Contacts].[Ent MainManagers - Id].children    )
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Contacts].[Подчинения].[Ent FirstLevels - Name].&[Мотовилов Андрей Александрович],
    [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void WhereAndRowsTest()
        {
            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    .Columns(
                        new TypedMdxElement(
                            new MdxMeasureElement("Amount")))
                    .Rows(
                        new UnionMdxElement(
                            new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                .Children()))
                    .Where(
                        new List<IMdxElement>
                        {
                            new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                .Value("1. КИСЛОМОЛОЧНАЯ группа"),
                            new MdxHierarchy("[Ent Goods].[Ent BKG - Name]")
                                .Value("ММЛ ЗЕЛЕНЫЕ"),
                            new MdxHierarchy("[Dates].[Date]")
                                .Range("2014-08-23T00:00:00", "2014-08-31T00:00:00")
                        });

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
(
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа]
    }
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void MultyWhereAndRowsTest()
        {
            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    .Columns(
                        new TypedMdxElement(
                            new MdxMeasureElement("Amount")))
                    .Rows(
                        new UnionMdxElement(
                            new MdxHierarchy("[Ent Goods].[Ent SubGroup NMK - Name]")
                                .Children(),
                            new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                .Children()))
                    .Where(
                        new List<IMdxElement>
                        {
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("1. КИСЛОМОЛОЧНАЯ группа"),
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("2. МОЛОЧНАЯ группа")),

                            new MdxHierarchy("[Ent Goods].[Ent BKG - Name]")
                                .Value("ММЛ ЗЕЛЕНЫЕ"),
                            new MdxHierarchy("[Dates].[Date]")
                                .Range("2014-08-23T00:00:00", "2014-08-31T00:00:00"),


                        });

            var mdxQuery = builder.Build();

            var expectedQuery =
                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
(
    [Ent Goods].[Ent SubGroup NMK - Name].children,
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    }
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)";

            Assert.AreEqual(expectedQuery, mdxQuery, "Запрос по измерениям должен совпадать с образцом");
        }

        [TestMethod]
        [TestCategory("CI")]
        public void TestQuery()
        {
            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    //.Columns()
                    .Rows(
                        new MdxNonEmptyJoined(
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Contacts].[Ent Regions - Name]").Children()
                            )));

            Debug.WriteLine(builder.Build());
        }

        [TestMethod]
        [TestCategory("CI")]
        public void TestDashboardAnalizeConcretePeriod()
        {
            var expectedQuery = @"WITH
    MEMBER [Measures].[field] AS
        [Measures].[DeliveryCount]
    MEMBER [Measures].[prevMonthYearPeiod] AS
                (
            [Dates].[Cal Month].&[2016]&[10],
            [Measures].[field]
        )
    MEMBER [Measures].[prevYearPeriod] AS
                (
            [Dates].[Cal Month].&[2016]&[11],
            [Measures].[field]
        )
    MEMBER [Measures].[prevMonthPeiod] AS
                (
            [Dates].[Cal Month].&[2017]&[10],
            [Measures].[field]
        )
    MEMBER [Measures].[currentPeiod] AS
                (
            [Dates].[Cal Month].&[2017]&[11],
            [Measures].[field]
        )
    MEMBER [Measures].[prognoz] AS
        Divide (Sum ([Dates].[Date].&[2017-10-31T00:00:00]:[Dates].[Date].&[2017-10-02T00:00:00], [Measures].[field]), 30, 0) * 30
    MEMBER [Measures].[prevYearDynamic] AS
        IIF (IsEmpty([Measures].[prognoz]), Null, (Divide ([Measures].[prognoz], [Measures].[prevYearPeriod], 0) - 1))
    MEMBER [Measures].[prevMonthDynamic] AS
        IIF (IsEmpty([Measures].[prognoz]), Null, (Divide ([Measures].[prognoz], [Measures].[prevMonthPeiod], 0) - 1))
SELECT
    {
        [Measures].[prevMonthYearPeiod],
        [Measures].[prevYearPeriod],
        [Measures].[prevMonthPeiod],
        [Measures].[currentPeiod],
        [Measures].[prognoz],
        [Measures].[prevYearDynamic],
        [Measures].[prevMonthDynamic]
    }
ON COLUMNS,
NON EMPTY (
[Ent Contacts].[Ent FirstLevels - Name].children)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Goods Type Name].&[Продукция Сатурн]
)";

            string measure = "DeliveryCount";
            string whereString = "[Ent Goods].[Goods Type Name]";
            string whereValue = "Продукция Сатурн";

            var period = PeriodType.CalendarMonth;
            var stringDate = "01.11.2017";
            string fieldMember = "field";
            string prevMonthYearPeriodName = "prevMonthYearPeiod";
            string prevYearPeriodName = "prevYearPeriod";
            string prevMonthPeriodName = "prevMonthPeiod";
            string currentPeiodName = "currentPeiod";
            string prognozName = "prognoz";
            string prevYearDynamicName = "prevYearDynamic";
            string prevMonthDynamicName = "prevMonthDynamic";

            DateTime dateNowValue = DateTime.Parse(stringDate);

            DateTime endDatePrognoz = dateNowValue.AddDays(-1);
            DateTime startDatePrognoz = endDatePrognoz.AddDays(-29);

            switch (period)
            {
                case PeriodType.CalendarMonth:
                case PeriodType.Day:
                    dateNowValue = DateTime.Parse(stringDate);

                    break;

                case PeriodType.ProductionMonth:
                    dateNowValue = DateTimeExt.ParseProductionMonth(stringDate);
                    break;
            }

            var dateNowPrevMonth = dateNowValue.AddMonths(-1);
            var datePrevYear = dateNowValue.AddYears(-1);
            var datePrevMonthYear = dateNowValue.AddYears(-1).AddMonths(-1);

            var dateNowMdx = DateMdxHelper.GetMdxDateValue[period](dateNowValue);
            var dateNowPrevMonthMdx = DateMdxHelper.GetMdxDateValue[period](dateNowPrevMonth);
            var datePrevYearMdx = DateMdxHelper.GetMdxDateValue[period](datePrevYear);
            var datePrevMonthYearMdx = DateMdxHelper.GetMdxDateValue[period](datePrevMonthYear);

            var endDatePrognozMdx = DateMdxHelper.GetMdxDateValue[PeriodType.Day](endDatePrognoz);
            var startDatePrognozMdx = DateMdxHelper.GetMdxDateValue[PeriodType.Day](startDatePrognoz);

            var periodType = "[Dates].[Cal Month]"; //SaturnMeasures.DatesHierarchiesMap[period];
            var periodTypeDay = "[Dates].[Date]"; //SaturnMeasures.DatesHierarchiesMap[PeriodType.Day];


            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    .With(
                        new MdxMember(fieldMember, measure),
                        new MdxMember(prevMonthYearPeriodName,
                            new UnionMdxElement(
                                new MdxHierarchy(periodType).Value(datePrevMonthYearMdx),
                                new MdxMeasureElement(fieldMember)
                            )
                        ),
                        new MdxMember(prevYearPeriodName,
                            new UnionMdxElement(
                                new MdxHierarchy(periodType).Value(datePrevYearMdx),
                                new MdxMeasureElement(fieldMember)
                            )
                        ),
                        new MdxMember(prevMonthPeriodName,
                            new UnionMdxElement(
                                new MdxHierarchy(periodType).Value(dateNowPrevMonthMdx),
                                new MdxMeasureElement(fieldMember)
                            )
                        ),
                        new MdxMember(currentPeiodName,
                            new UnionMdxElement(
                                new MdxHierarchy(periodType).Value(dateNowMdx),
                                new MdxMeasureElement(fieldMember)
                            )
                        ),
                        new MdxMember(prognozName,
                            new MdxMultiply(
                                new MdxDivideNumber(
                                    new MdxSum(
                                        new MdxRangeElement(
                                            new MdxHierarchy(periodTypeDay).Value(endDatePrognozMdx),
                                            new MdxHierarchy(periodTypeDay).Value(startDatePrognozMdx)
                                        ),
                                        fieldMember
                                    ),
                                    30
                                )
                                ,
                                new MdxNumber(30)
                            )

                        ),
                        new MdxMember(prevYearDynamicName,
                            new MdxIif(
                                new MdxIsEmpty(
                                    new MdxMeasureElement(prognozName)
                                ),
                                new MdxNull(),
                                new MdxSubtraction(
                                    new MdxDivide(
                                        new MdxMeasureElement(prognozName),
                                        new MdxMeasureElement(prevYearPeriodName)
                                    ),
                                    new MdxNumber(1)
                                )
                            )
                        ),
                        new MdxMember(prevMonthDynamicName,
                            new MdxIif(
                                new MdxIsEmpty(
                                    new MdxMeasureElement(prognozName)
                                ),
                                new MdxNull(),
                                new MdxSubtraction(
                                    new MdxDivide(
                                        new MdxMeasureElement(prognozName),
                                        new MdxMeasureElement(prevMonthPeriodName)
                                    ),
                                    new MdxNumber(1)
                                )
                            )
                        )
                    )
                    .Columns(
                        new TypedMdxElement(
                            prevMonthYearPeriodName,
                            prevYearPeriodName,
                            prevMonthPeriodName,
                            currentPeiodName,
                            prognozName,
                            prevYearDynamicName,
                            prevMonthDynamicName
                        )
                    )
                    .Rows(
                        new MdxNonEmpty(
                            new MdxHierarchy("[Ent Contacts].[Ent FirstLevels - Name]")
                                .Children()
                        )
                    )
                    .Where(
                        new List<IMdxElement>
                        {
                            new MdxHierarchy(whereString)
                                .Value(whereValue),


                        });

            var newMdxQuery = builder.Build();

            Assert.AreEqual(expectedQuery, newMdxQuery, "Запрос по анализу периодов должен совпадать с образцом");
        }


        [TestMethod]
        [TestCategory("CI")]
        public void GetRowHierarchiesTest()
        {
            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Rows(
                        new MdxNonEmpty(
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Children()),
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent SubGroup NMK - Name]")
                                    .Children()),
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Children()),
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Товары]")
                                    .Children())
                        )).Where(
                        new List<IMdxElement>
                        {
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("1. КИСЛОМОЛОЧНАЯ группа"),
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("2. МОЛОЧНАЯ группа")),

                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Value("Коровка"),
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Value("Бабулины продукты")),

                            new MdxHierarchy("[Ent Goods].[Ent BKG - Name]")
                                .Value("ММЛ ЗЕЛЕНЫЕ"),
                            new MdxHierarchy("[Dates].[Date]")
                                .Range("2014-08-23T00:00:00", "2014-08-31T00:00:00")
                        });

            builder.Build();

            var mdxRows = builder.RowsGet.FindHierarchies().Reverse();

                foreach (var hierarchy in mdxRows)
                {
                    var dc = new MdxDrawContext();

                    hierarchy.Draw(dc);

                    Console.WriteLine("    "+dc);
                }

                Console.WriteLine("}");
            
        }

        [TestMethod]
        [TestCategory("CI")]
        public void RemoveLastRow_Test()
        {
            #region expected

            var expectedMdxQuery = new[]
            {
                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
(
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    },
    [Ent Goods].[Ent SubGroup NMK - Name].children,
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    },
    [Ent Goods].[Товары].children
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)",

                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
(
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    },
    [Ent Goods].[Ent SubGroup NMK - Name].children,
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    }
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)",

                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
(
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    },
    [Ent Goods].[Ent SubGroup NMK - Name].children
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00],
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    }
)",

                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
(
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    }
)
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00],
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    }
)",

                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00],
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    },
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    }
)"
            };

            #endregion

            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    .Columns(
                        new TypedMdxElement(
                            new MdxMeasureElement("Amount")))
                    .Rows(
                        new UnionMdxElement(
                            new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                .Children(),
                            new MdxHierarchy("[Ent Goods].[Ent SubGroup NMK - Name]")
                                .Children(),
                            new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                .Children(),
                            new MdxHierarchy("[Ent Goods].[Товары]")
                                .Children()
                        ))
                    .Where(
                        new List<IMdxElement>
                        {
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("1. КИСЛОМОЛОЧНАЯ группа"),
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("2. МОЛОЧНАЯ группа")),

                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Value("Коровка"),
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Value("Бабулины продукты")),

                            new MdxHierarchy("[Ent Goods].[Ent BKG - Name]")
                                .Value("ММЛ ЗЕЛЕНЫЕ"),
                            new MdxHierarchy("[Dates].[Date]")
                                .Range("2014-08-23T00:00:00", "2014-08-31T00:00:00")
                        });



            var querys = MdxBuilderArrayCreator.ToMdxQueriesForExcel(builder).ToList();

            Assert.AreEqual(expectedMdxQuery.Length, querys.Count, "Должен возвращаться набор из {0} запросов",
                expectedMdxQuery.Length);

            for (var i = 0; i < expectedMdxQuery.Length; i++)
            {
                Assert.AreEqual(expectedMdxQuery[i].Replace("\r", ""), querys[i].Replace("\r", ""), "Запрос {0} должен быть вида\n{1}",
                    i + 1, expectedMdxQuery[i]);
            }
        }

        [TestMethod]
        [TestCategory("CI")]
        public void RemoveLastRow_NonEmptyElement_Test()
        {
            #region expected

            var expectedMdxQuery = new[]
            {
                @"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
NON EMPTY (
    {
                {
            [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
            [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
        }
    },    {
        [Ent Goods].[Ent SubGroup NMK - Name].children
    },    {
                {
            [Ent Goods].[Ent Brends - Name].&[Коровка],
            [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
        }
    },    {
        [Ent Goods].[Товары].children
    })
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)",

@"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
NON EMPTY (
    {
                {
            [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
            [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
        }
    },    {
        [Ent Goods].[Ent SubGroup NMK - Name].children
    },    {
                {
            [Ent Goods].[Ent Brends - Name].&[Коровка],
            [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
        }
    })
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00]
)",

@"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
NON EMPTY (
    {
                {
            [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
            [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
        }
    },    {
        [Ent Goods].[Ent SubGroup NMK - Name].children
    })
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00],
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    }
)",

@"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS,
NON EMPTY (
    {
                {
            [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
            [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
        }
    })
ON ROWS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00],
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    }
)",

@"SELECT
    {
        [Measures].[Amount]
    }
ON COLUMNS
FROM Dashboard
WHERE
(
    [Ent Goods].[Ent BKG - Name].&[ММЛ ЗЕЛЕНЫЕ],
    [Dates].[Date].&[2014-08-23T00:00:00]:[Dates].[Date].&[2014-08-31T00:00:00],
        {
        [Ent Goods].[Ent Brends - Name].&[Коровка],
        [Ent Goods].[Ent Brends - Name].&[Бабулины продукты]
    },
        {
        [Ent Goods].[Ent Group NMK - Name].&[1. КИСЛОМОЛОЧНАЯ группа],
        [Ent Goods].[Ent Group NMK - Name].&[2. МОЛОЧНАЯ группа]
    }
)"
            };

            #endregion

            var builder =
                TestMdxBuilderFactory.CreateBuilder()
                    .Cube("Dashboard")
                    .Columns(
                        new TypedMdxElement(
                            new MdxMeasureElement("Amount")))
                    .Rows(
                        new MdxNonEmpty(
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Children()),
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent SubGroup NMK - Name]")
                                    .Children()),
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Children()),
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Товары]")
                                    .Children())
                        ))
                    .Where(
                        new List<IMdxElement>
                        {
                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("1. КИСЛОМОЛОЧНАЯ группа"),
                                new MdxHierarchy("[Ent Goods].[Ent Group NMK - Name]")
                                    .Value("2. МОЛОЧНАЯ группа")),

                            new TypedMdxElement(
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Value("Коровка"),
                                new MdxHierarchy("[Ent Goods].[Ent Brends - Name]")
                                    .Value("Бабулины продукты")),

                            new MdxHierarchy("[Ent Goods].[Ent BKG - Name]")
                                .Value("ММЛ ЗЕЛЕНЫЕ"),
                            new MdxHierarchy("[Dates].[Date]")
                                .Range("2014-08-23T00:00:00", "2014-08-31T00:00:00")
                        });



            var querys = MdxBuilderArrayCreator.ToMdxQueriesForExcel(builder).ToList();

            Assert.AreEqual(expectedMdxQuery.Length, querys.Count, "Должен возвращаться набор из {0} запросов",
                expectedMdxQuery.Length);

            for (var i = 0; i < expectedMdxQuery.Length; i++)
            {
                Assert.AreEqual(expectedMdxQuery[i].Replace("\r", ""), querys[i].Replace("\r", ""), "Запрос {0} должен быть вида\n{1}",
                    i + 1, expectedMdxQuery[i]);
            }
        }

        public object mdxQuery { get; set; }
    }

}