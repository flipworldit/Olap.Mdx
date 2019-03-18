using System.Collections.Generic;
using System.Linq;
using LinqExtIn;
using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MdxElementsExt
{
    public static class Mesuares
    {
        public static MdxDeliveryMesuare Return
        {
            get { return DeliveryByType("Возврат"); }
        }

        public static MdxDeliveryMesuare Delivery
        {
            get { return DeliveryByType("Реализация"); }
        }

        private static MdxDeliveryMesuare DeliveryByType(string type)
        {
            var mdxValueElement = DeliveryType()
                .Value(type);

            return new MdxDeliveryMesuare(mdxValueElement);
        }

        private static MdxHierarchy DeliveryType()
        {
            return new MdxHierarchy("[DeliveryType].[Name]");
        }

        public static MdxMember DeliverySumPrice
        {
            get
            {
                return new MdxMember("summaKRub",
                    new MdxSum(
                        Delivery,
                        Divider.SumPriceDiv1000
                        ));
            }
        }

        public static MdxMember DeliveryWeight
        {
            get
            {
                return new MdxMember("weightT",
                    new MdxSum(
                        Delivery,
                        Divider.WeightDiv1000
                        ));

            }
        }

        public static MdxMember ReturnAccum(MdxRangeElement monthOffset)
        {
            return new MdxMember("returnAccumKRub",
                new MdxSum(
                    new UnionMdxElement(
                        monthOffset,
                        Return),
                    new MdxSubtraction(
                        new MdxEmptyElement(),
                        Divider.SumPriceDiv1000
                        )
                    ));
        }

        public static MdxMember DeliveryAccum(MdxRangeElement monthOffset)
        {
            return new MdxMember("delAccumKRub",
                new MdxSum(
                    new UnionMdxElement(
                        monthOffset,
                        Delivery),
                    Divider.SumPriceDiv1000
                    ));
        }

        public static MdxMember GrossProfitSum()
        {
            return new MdxMember("summaGross",
                Divider.GrossProfitDiv1000);
        }

        public static MdxMeasureElement Sum()
        {
            return new MdxMeasureElement("sum");
        }
        
    }

    public class MdxDeliveryMesuare : IMdxElement
    {
        private readonly MdxValueElement _mdxValueElement;

        public MdxDeliveryMesuare(
            MdxValueElement mdxValueElement)
        {
            _mdxValueElement = mdxValueElement;
        }

        public void Draw(MdxDrawContext dc)
        {
            _mdxValueElement.Draw(dc);
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxValueElement.GetChildren();
        }

        public MdxSum SumPrice
        {
            get { return new MdxSum(this, "SumPrice"); }
        }

        public MdxSum SumPriceAll
        {
            get { return new MdxSum(this, "SumPriceAll"); }
        }

        public MdxSum Weight
        {
            get { return new MdxSum(this, "Weight"); }
        }

        public MdxSum WeightAll
        {
            get { return new MdxSum(this, "WeightAll"); }
        }


        public MdxSum SumPriceDiv1000
        {
            get
            {
                return
                    new MdxSum(this,
                        Divider.SumPriceDiv1000);
            }
        }
    }
}
