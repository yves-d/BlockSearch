using BlockSearch.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System.Linq;

namespace BlockSearch.Application.Tests
{
    [TestClass]
    public class BlockSearchServiceTests
    {
        BlockSearchServiceTestHarness _testHarness;

        [TestMethod]
        public void When_Corresponding_Block_Exists_GetAddressTransactionsInBlock_Should_Return_A_Block_With_Transactions()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .Build();

            // act
            var block = _testHarness.Execute_GetAddressTransactionsInBlock().Result;

            // act & assert
            block.ShouldNotBeNull();
            block.Transactions.Any().ShouldBeTrue();
        }

        [TestMethod]
        public void When_2_Of_4_Transactions_Contain_Matching_Address_In_From_Field_GetAddressTransactionsInBlock_Should_Return_Block_With_2_Transactions()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithSpecifiedAddressPresentInFromFieldForTwoTransactions()
                .Build();

            // act
            var block = _testHarness.Execute_GetAddressTransactionsInBlock().Result;

            // act & assert
            block.ShouldNotBeNull();
            block.Transactions.Count().ShouldBe(2);
        }

        [TestMethod]
        public void When_2_Of_4_Transactions_Contain_Matching_Address_In_To_Field_GetAddressTransactionsInBlock_Should_Return_Block_With_2_Transactions()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithSpecifiedAddressPresentInToFieldForTwoTransactions()
                .Build();

            // act
            var block = _testHarness.Execute_GetAddressTransactionsInBlock().Result;

            // act & assert
            block.ShouldNotBeNull();
            block.Transactions.Count().ShouldBe(2);
        }

        [TestMethod]
        public void When_No_Transactions_Contain_Matching_Address_GetAddressTransactionsInBlock_Should_Return_Block_With_0_Transactions()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithSpecifiedAddressNotMatchingAnyTransactions()
                .Build();

            // act
            var block = _testHarness.Execute_GetAddressTransactionsInBlock().Result;

            // act & assert
            block.ShouldNotBeNull();
            block.Transactions.Count().ShouldBe(0);
        }

        [TestMethod]
        public void When_Passed_BlockNumber_Is_Empty_GetAddressTransactionsInBlock_Should_Throw_InvalidInputException_And_Not_Log()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithEmptyBlockNumber()
                .Build();

            // act & assert
            Should.ThrowAsync<InvalidInputException>(() => _testHarness.Execute_GetAddressTransactionsInBlock())
                .Result.Message.ShouldBe("Missing input - blockNumber");
            _testHarness._logger.Received(0).LogError("Missing input - Block Number");
        }

        [TestMethod]
        public void When_Passed_CryptoType_Is_Empty_GetAddressTransactionsInBlock_Should_Throw_InvalidInputException_And_Not_Log()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithEmptyCryptoType()
                .Build();

            // act & assert
            Should.ThrowAsync<InvalidInputException>(() => _testHarness.Execute_GetAddressTransactionsInBlock())
                .Result.Message.ShouldBe("Missing input - cryptoType");
            _testHarness._logger.Received(0).LogError("Missing input - Crypto Type");
        }

        [TestMethod]
        public void When_CryptoService_Throws_BlockNotFoundException_GetAddressTransactionsInBlock_Should_Throw_And_Not_Log()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithCryptoServiceThrowingBlockNotFoundException()
                .Build();

            // act & assert
            Should.ThrowAsync<BlockNotFoundException>(() => _testHarness.Execute_GetAddressTransactionsInBlock());
            _testHarness._logger.Received(0).LogError("Block with that number was not found.");
        }

        [TestMethod]
        public void When_CryptoServiceFactory_Throws_ClientNotImplementedException_GetAddressTransactionsInBlock_Should_Throw_And_Log()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithCryptoServiceFactoryThrowingServiceNotImplementedException()
                .Build();

            // act & assert
            Should.ThrowAsync<ServiceNotImplementedException>(() => _testHarness.Execute_GetAddressTransactionsInBlock());
            _testHarness._logger.Received(1).LogError("Crypto Service not implemented - Bitcoin");
        }

        [TestMethod]
        public void When_CryptoServiceFactory_Throws_InitialisationFailureException_GetAddressTransactionsInBlock_Should_Throw_And_Log()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithCryptoServiceFactoryThrowingInitialisationFailureException()
                .Build();

            // act & assert
            Should.ThrowAsync<InitialisationFailureException>(() => _testHarness.Execute_GetAddressTransactionsInBlock());
            _testHarness._logger.Received(1).LogError("Failed to initialise NethereumClient");
        }

        [TestMethod]
        public void When_CryptoServiceFactory_Throws_General_Exception_GetAddressTransactionsInBlock_Should_Throw_And_Log()
        {
            // arrange
            _testHarness = new BlockSearchServiceTestHarness()
                .WithCryptoServiceThrowingGeneralException()
                .Build();

            // act & assert
            Should.ThrowAsync<ServiceNotImplementedException>(() => _testHarness.Execute_GetAddressTransactionsInBlock());
            _testHarness._logger.Received(1).LogError("Unknown exception");
        }
    }
}
