using BlockSearch.Application.CryptoService;
using BlockSearch.Application.ExternalClients;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockSearch.Application.Tests
{
    public class EthereumSearchClientTestHarness
    {
        // searcher client
        private ICryptoService _cryptoService;

        // injectables
        IEthereumClient _ethereumClient;

        // test variables
        private int _blockNumber;

        public EthereumSearchClientTestHarness()
        {
            _ethereumClient = Substitute.For<IEthereumClient>();
            InitialiseValidStubBlock();
            _blockNumber = 0;
        }

        #region SETUP

        private void InitialiseValidStubBlock()
        {
            BlockWithTransactions validBlock = new BlockWithTransactions()
            {
                BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                Number = new HexBigInteger("0xca3b19"),
                Transactions = new List<Nethereum.RPC.Eth.DTOs.Transaction>()
                {
                    new Nethereum.RPC.Eth.DTOs.Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = new HexBigInteger("0xca3b19"),
                        Gas = new HexBigInteger("0x3d090"),
                        TransactionHash = "0x0",
                        From = "0xea674fdde714fd979de3edf0f56aa9716b898ec8",
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = new HexBigInteger("0x2226c661547bb3")
                    }
                }.ToArray()
            };

            _ethereumClient.GetBlockWithTransactionsByNumberAsync(Arg.Any<int>()).Returns(validBlock);
        }

        #endregion

        #region ARRANGE

        public EthereumSearchClientTestHarness WithBlockNotFound()
        {
            _blockNumber = 1;
            _ethereumClient.GetBlockWithTransactionsByNumberAsync(Arg.Any<int>()).Returns((BlockWithTransactions)null);

            return this;
        }

        public EthereumSearchClientTestHarness Build()
        {
            _cryptoService = new EthereumService(_ethereumClient);

            return this;
        }

        #endregion

        #region ACT

        public Task<Common.Models.Block> Execute_GetBlockByBlockNumber()
        {
            return _cryptoService.GetBlockByBlockNumber(_blockNumber);
        }

        #endregion
    }
}
