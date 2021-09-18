using BlockSearch.Common.Enums;
using System;
using System.Collections.Generic;

namespace BlockSearch.Application.SearcherClients
{
    public class SearcherClientFactory : ISearcherClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private static Dictionary<CryptoType, Func<ISearcherClient>> searcherClients;

        public SearcherClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadSearcherClientDictionary();
        }

        private void LoadSearcherClientDictionary()
        {
            searcherClients = new Dictionary<CryptoType, Func<ISearcherClient>>();
            searcherClients.Add(CryptoType.Ethereum, () => (ISearcherClient)_serviceProvider.GetService(typeof(EthereumSearcherClient)));
        }

        public ISearcherClient GetSearcher(CryptoType cryptoType)
        {
            if (searcherClients.ContainsKey(cryptoType))
                return searcherClients[cryptoType]();

            throw new NotImplementedException();
        }
    }
}
