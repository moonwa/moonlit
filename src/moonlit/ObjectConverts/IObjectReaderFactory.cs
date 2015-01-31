namespace Moonlit.ObjectConverts
{
    public interface IObjectReaderFactory
    {
        IObjectReader CreateReader(object obj);
    }
}