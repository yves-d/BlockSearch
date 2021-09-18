using BlockSearch.Application.Exceptions;
using BlockSearch.Common.Models;
using BlockSearch.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlockSearch.Application.SearcherClients
{
    public class EthereumSearcherClient : ISearcherClient
    {
        private const string GET_BLOCK_BY_NUMBER_METHOD = "eth_getBlockByNumber";

        private readonly HttpClient _client;

        public EthereumSearcherClient(HttpClient client, IOptions<EthereumSearcherOptions> options)
        {
            client.BaseAddress = new Uri($"{options.Value.BaseUri}/{options.Value.ProjectId}");
            _client = client;
        }
        
        public async Task<Block> GetBlock(int? blockNumber)
        {
            if (!blockNumber.HasValue)
                throw new InvalidInputException("Block number cannot be empty");

            return new Block();
        }
    }
}
