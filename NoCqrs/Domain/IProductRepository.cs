namespace NoCqrs.Domain
{
    public interface IProductRepository
    {
        void Add(Product product);

        Product WithCode(string code);
    }
}