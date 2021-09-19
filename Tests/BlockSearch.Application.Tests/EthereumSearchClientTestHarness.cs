using BlockSearch.Application.SearcherClients;
using BlockSearch.Application.Tests.Helpers;
using BlockSearch.Common.Models;
using BlockSearch.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlockSearch.Application.Tests
{
    public class EthereumSearchClientTestHarness
    {
        // searcher client
        private ISearcherClient _searcherClient;

        // injectables
        private HttpClient _httpClient;
        private MockHttpMessageHandler _mockMessageHandler;
        IOptions<EthereumSearcherOptions> _options;

        // test variables
        private int? _blockNumber;
        private string _httpClientJsonResponse;
        private HttpStatusCode _httpClientStatusCodeResponse;

        public EthereumSearchClientTestHarness()
        {
            InitialiseValidOptions();
            _blockNumber = 0;
            _httpClientJsonResponse = "";
            _httpClientStatusCodeResponse = HttpStatusCode.OK;
        }

        #region ARRANGE

        private void InitialiseValidOptions()
        {
            var ethereumSearcherOptions = new EthereumSearcherOptions()
            {
                BaseUri = "http://validbaseuri.com/",
                ProjectId = "123"
            };
            _options = Substitute.For<IOptions<EthereumSearcherOptions>>();
            _options.Value.Returns(ethereumSearcherOptions);
        }

        public EthereumSearchClientTestHarness WithNullBlockNumber()
        {
            _blockNumber = null;
            return this;
        }

        public EthereumSearchClientTestHarness WithBlockNotFound()
        {
            _blockNumber = 1;
            _httpClientJsonResponse = JsonConvert.SerializeObject(GetEmptyBlock());
            _httpClientStatusCodeResponse = HttpStatusCode.NotFound;

            return this;
        }

        public EthereumSearchClientTestHarness WithBlockNumberContainingOneTransaction()
        {
            _blockNumber = 2;
            _httpClientJsonResponse = JsonConvert.SerializeObject(GetBlockWithOneTransaction());
            _httpClientStatusCodeResponse = HttpStatusCode.OK;

            return this;
        }

        public EthereumSearchClientTestHarness Build()
        {
            _mockMessageHandler = new MockHttpMessageHandler(_httpClientJsonResponse, _httpClientStatusCodeResponse);
            _httpClient = new HttpClient(_mockMessageHandler);
            _searcherClient = new EthereumSearcherClient(_options);

            return this;
        }

        #region HELPERS

        private Block GetEmptyBlock()
        {
            return new Block();
        }

        private Block GetBlockWithOneTransaction()
        {
            return new Block()
            { 
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        BlockHash = "BlockHash",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "Hash",
                        From = "FromAddress",
                        To = "ToAddress",
                        Value = (0.01m).ToString()
                    }
                }
            };
        }

        #endregion

        #endregion

        #region ACT

        public Task<Block> Execute_GetBlock()
        {
            return _searcherClient.GetBlockByBlockNumber(_blockNumber.Value);
        }

        #endregion
    }
}
