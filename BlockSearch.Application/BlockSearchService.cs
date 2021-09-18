using BlockSearch.Application.SearcherClients;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Logger;
using BlockSearch.Common.Models;
using System;

namespace BlockSearch.Application
{
    public class BlockSearchService : IBlockSearchService
    {
        private readonly ILoggerAdapter<IBlockSearchService> _logger;
        private readonly ISearcherClientFactory _searcherClientFactory;

        public BlockSearchService(ILoggerAdapter<IBlockSearchService> logger, ISearcherClientFactory searcherClientFactory)
        {
            _logger = logger;
            _searcherClientFactory = searcherClientFactory;
        }
        
        public Block GetEthereumAddressTransactionsInBlock(CryptoType cryptoType, int blockNumber, string address)
        {
            throw new NotImplementedException();
        }
    }
}
