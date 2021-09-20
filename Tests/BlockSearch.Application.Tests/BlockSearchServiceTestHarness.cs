using BlockSearch.Application.Exceptions;
using BlockSearch.Application.SearcherClients;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Logger;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockSearch.Application.Tests
{
    public class BlockSearchServiceTestHarness
    {
        // searcher client
        private IBlockSearchService _blockSearchService;

        // injectables
        public ILoggerAdapter<IBlockSearchService> _logger { get; private set; }
        private ISearcherClientFactory _searcherClientFactory;

        // test variables
        private CryptoType? _cryptoType;
        private int? _blockNumber;
        private string _address;
        private ISearcherClient _searcherClient;

        public Common.Models.Block _block { get; private set; }

        public BlockSearchServiceTestHarness()
        {
            _logger = Substitute.For<ILoggerAdapter<IBlockSearchService>>();
            _searcherClient = Substitute.For<ISearcherClient>();
            _searcherClientFactory = Substitute.For<ISearcherClientFactory>();
            InitialiseValidTestVariables();
            InitialiseValidBlockFromSearchClient();
        }

        #region SETUP

        private void InitialiseValidBlockFromSearchClient()
        {
            _block = GetBlockWithOneTransaction();
        }

        private void InitialiseValidTestVariables()
        {
            _blockNumber = 0;
            _cryptoType = CryptoType.Ethereum;
            _address = "0xea674fdde714fd979de3edf0f56aa9716b898ec8";
        }

        #endregion

        #region ARRANGE

        public BlockSearchServiceTestHarness WithEmptyCryptoType()
        {
            _cryptoType = null;
            return this;
        }

        public BlockSearchServiceTestHarness WithEmptyBlockNumber()
        {
            _blockNumber = null;
            return this;
        }

        public BlockSearchServiceTestHarness WithSpecifiedAddressPresentInFromFieldForTwoTransactions()
        {
            _block = GetBlockWithFourTransactionsWithSpecifiedAddressInFromFieldOfTwo();

            return this;
        }

        public BlockSearchServiceTestHarness WithSpecifiedAddressPresentInToFieldForTwoTransactions()
        {
            _block = GetBlockWithFourTransactionsWithSpecifiedAddressInToFieldOfTwo();
            return this;
        }

        public BlockSearchServiceTestHarness WithSpecifiedAddressNotMatchingAnyTransactions()
        {
            _block = GetBlockWithFourTransactionsWithNoMatchingAddresses();
            return this;
        }

        public BlockSearchServiceTestHarness WithSearcherClientFactoryThrowingClientNotImplementedException()
        {
            _searcherClientFactory.When(factory => factory.GetSearcher(Arg.Any<CryptoType>()))
                .Do(factory => { throw new ClientNotImplementedException("Searcher Client not implemented - Bitcoin"); });

            return this;
        }

        public BlockSearchServiceTestHarness WithSearcherClientFactoryThrowingInitialisationFailureException()
        {
            _searcherClientFactory.When(factory => factory.GetSearcher(Arg.Any<CryptoType>()))
                .Do(factory => { throw new InitialisationFailureException("Failed to initialise NethereumClient"); });

            return this;
        }

        public BlockSearchServiceTestHarness WithSearcherClientThrowingBlockNotFoundException()
        {
            _searcherClient.When(client => client.GetBlockByBlockNumber(Arg.Any<int>()))
                .Do(client => { throw new BlockNotFoundException("Block with that number was not found."); });

            return this;
        }

        public BlockSearchServiceTestHarness WithSearcherClientThrowingGeneralException()
        {
            _searcherClient.When(client => client.GetBlockByBlockNumber(Arg.Any<int>()))
                .Do(client => { throw new Exception("Unknown exception"); });

            return this;
        }

        public BlockSearchServiceTestHarness Build()
        {
            _searcherClient.GetBlockByBlockNumber(Arg.Any<int>()).Returns(_block);
            _searcherClientFactory.GetSearcher(Arg.Any<CryptoType>()).Returns(_searcherClient);
            _blockSearchService = new BlockSearchService(_logger, _searcherClientFactory);
            return this;
        }

        #region HELPERS

        private Common.Models.Block GetBlockWithOneTransaction()
        {
            return new Common.Models.Block()
            {
                Transactions = new List<Common.Models.Transaction>()
                {
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xea674fdde714fd979de3edf0f56aa9716b898ec8",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    }
                }
            };
        }

        private Common.Models.Block GetBlockWithFourTransactionsWithSpecifiedAddressInFromFieldOfTwo()
        {
            return new Common.Models.Block()
            {
                Transactions = new List<Common.Models.Transaction>()
                {
                    // matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = _address,
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = _address,
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xea674fdde714fd979de3edf0f56aa9716b898ec9",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xea674fdde714fd979de3edf0f56aa9716b898ec9",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    }
                }
            };
        }

        private Common.Models.Block GetBlockWithFourTransactionsWithSpecifiedAddressInToFieldOfTwo()
        {
            return new Common.Models.Block()
            {
                Transactions = new List<Common.Models.Transaction>()
                {
                    // matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = _address,
                        Value = (0.01m).ToString()
                    },
                    // matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = _address,
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    }
                }
            };
        }

        private Common.Models.Block GetBlockWithFourTransactionsWithNoMatchingAddresses()
        {
            return new Common.Models.Block()
            {
                Transactions = new List<Common.Models.Transaction>()
                {
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    },
                    // not matching
                    new Common.Models.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.Value.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    }
                }
            };
        }

        #endregion

        #endregion

        #region ACT

        public Task<Common.Models.Block> Execute_GetAddressTransactionsInBlock()
        {
            return _blockSearchService.GetAddressTransactionsInBlock(_cryptoType, _blockNumber, _address);
        }

        #endregion
    }
}
