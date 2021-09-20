using BlockSearch.ExternalClients.CryptoClients;
using BlockSearch.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Nethereum.RPC.Eth.DTOs;
using NSubstitute;
using System.Threading.Tasks;

namespace BlockSearch.ExternalClients.Tests
{
    public class NethereumClientTestHarness
    {
        // searcher client
        private IEthereumClient _ethereumClient;

        // injectables
        IOptions<NethereumClientOptions> _options;

        // test variables
        private const int BLOCK_NUMBER = 1;

        public NethereumClientTestHarness()
        {
            _ethereumClient = Substitute.For<IEthereumClient>();
            InitialiseValidOptions();
        }
        
        #region SETUP

        private void InitialiseValidOptions()
        {
            var ethereumSearcherOptions = new NethereumClientOptions()
            {
                BaseUri = "http://validbaseuri.com/",
                ProjectId = "123"
            };
            _options = Substitute.For<IOptions<NethereumClientOptions>>();
            _options.Value.Returns(ethereumSearcherOptions);
        }

        #endregion

        #region ARRANGE

        public NethereumClientTestHarness WithEmptyBaseUri()
        {
            var ethereumSearcherOptions = new NethereumClientOptions()
            {
                BaseUri = "",
                ProjectId = "123"
            };
            _options = Substitute.For<IOptions<NethereumClientOptions>>();
            _options.Value.Returns(ethereumSearcherOptions);

            return this;
        }

        public NethereumClientTestHarness WithEmptyProjectId()
        {
            var ethereumSearcherOptions = new NethereumClientOptions()
            {
                BaseUri = "http://validbaseuri.com/",
                ProjectId = ""
            };
            _options = Substitute.For<IOptions<NethereumClientOptions>>();
            _options.Value.Returns(ethereumSearcherOptions);

            return this;
        }

        public NethereumClientTestHarness Build()
        {
            _ethereumClient = new NethereumClient(_options);

            return this;
        }

        #endregion

        #region ACT

        public Task<BlockWithTransactions> Execute_GetBlockWithTransactionsByNumberAsync()
        {
            return _ethereumClient.GetBlockWithTransactionsByNumberAsync(BLOCK_NUMBER);
        }

        #endregion
    }
}
