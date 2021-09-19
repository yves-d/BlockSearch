using BlockSearch.Application.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace BlockSearch.Application.Tests
{
    [TestClass]
    public class EthereumSearcherClientTests
    {
        private EthereumSearchClientTestHarness _testHarness;

        [TestMethod]
        public void When_Passed_BlockNumber_Has_No_Corresponding_Block_GetBlock_Should_Return_Empty_Block_With_Empty_Transaction_List()
        {
            // arrange
            _testHarness = new EthereumSearchClientTestHarness()
                .WithBlockNotFound()
                .Build();

            // act & assert
            Should.ThrowAsync<BlockNotFoundException>(() => _testHarness.Execute_GetBlock());
        }

        [TestMethod]
        public void When_Passed_BlockNumber_Has_Value_GetBlock_Should_Return_A_Block()
        {
            // arrange
            _testHarness = new EthereumSearchClientTestHarness()
                .Build();

            // act
            var blockResult = _testHarness.Execute_GetBlock().Result;

            // assert
            blockResult.ShouldNotBeNull();
        }

        [TestMethod]
        public void When_Passed_BlockNumber_Has_Transactions_GetBlock_Should_Return_Block_With_Populated_Transaction_List()
        {
            // arrange
            _testHarness = new EthereumSearchClientTestHarness()
                .Build();

            // act
            var blockResult = _testHarness.Execute_GetBlock().Result;

            // assert
            blockResult.Transactions.ShouldNotBeEmpty();
        }
    }
}
