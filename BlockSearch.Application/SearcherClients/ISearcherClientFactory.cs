using BlockSearch.Common.Enums;

namespace BlockSearch.Application.SearcherClients
{
    public interface ISearcherClientFactory
    {
        ISearcherClient GetSearcher(CryptoType cryptoType);
    }
}
