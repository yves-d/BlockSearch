using BlockSearch.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace BlockSearch.Application.Tests
{
    [TestClass]
    public class EthereumSearcherClientTests
    {
        private EthereumSearchClientTestHarness _testHarness;

        [TestMethod]
        public void When_Passed_BlockNumber_Has_No_Corresponding_Block_GetBlockByBlockNumber_Should_Return_Empty_Block_With_Empty_Transaction_List()
        {
            // arrange
            _testHarness = new EthereumSearchClientTestHarness()
                .WithBlockNotFound()
                .Build();

            // act & assert
            Should.ThrowAsync<BlockNotFoundException>(() => _testHarness.Execute_GetBlockByBlockNumber());
        }

        [TestMethod]
        public void When_Passed_BlockNumber_Has_Value_GetBlockByBlockNumber_Should_Return_A_Block()
        {
            // arrange
            _testHarness = new EthereumSearchClientTestHarness()
                .Build();

            // act
            var blockResult = _testHarness.Execute_GetBlockByBlockNumber().Result;

            // assert
            blockResult.ShouldNotBeNull();
        }

        [TestMethod]
        public void When_Passed_BlockNumber_Has_Transactions_GetBlockByBlockNumber_Should_Return_Block_With_Populated_Transaction_List()
        {
            // arrange
            _testHarness = new EthereumSearchClientTestHarness()
                .Build();

            // act
            var blockResult = _testHarness.Execute_GetBlockByBlockNumber().Result;

            // assert
            blockResult.Transactions.ShouldNotBeEmpty();
        }
    }
}
