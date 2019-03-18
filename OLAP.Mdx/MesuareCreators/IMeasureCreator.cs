namespace OLAP.Mdx.MesuareCreators
{
    public interface IMeasureCreator
    {
        IMdxElement Create(string measure);
    }
}