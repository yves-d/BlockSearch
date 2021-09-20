using BlockSearch.Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace BlockSearch.MVC.Tests
{
    [TestClass]
    public class TransactionSearchControllerTests
    {
        private TransactionSearchControllerTestHarness _testHarness;

        [TestMethod]
        public void When_Input_TransactionSearchModel_Is_Valid_A_Populated_TransactionSearchModel_With_Transactions_Should_Be_Returned()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.ShouldNotBe(null);
            model.Transactions.Any().ShouldBeTrue();
        }

        [TestMethod]
        public void When_BlockSearchService_Throws_An_Exception_Then_TransactionSearchModel_Should_Reset_CryptoType_To_Ethereum()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .WithBlockSearchServiceThrowingGeneralException()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.Crypto.ShouldBe(CryptoType.Ethereum);
        }

        [TestMethod]
        public void When_BlockSearchService_Throws_BlockNotFoundException_Then_TransactionSearchModel_Should_Contain_BlockNotFoundMessage()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .WithBlockSearchServiceThrowingBlockNotFoundException()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.ErrorMessage.ShouldBe(TransactionSearchControllerTestHarness.BLOCK_NOT_FOUND_MESSAGE);
        }

        [TestMethod]
        public void When_BlockSearchService_Throws_InvalidInputException_Then_TransactionSearchModel_Should_Contain_InvalidInputMessage()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .WithBlockSearchServiceThrowingInvalidInputException()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.ErrorMessage.ShouldBe(TransactionSearchControllerTestHarness.INVALID_INPUT_MESSAGE);
        }

        [TestMethod]
        public void When_BlockSearchService_Throws_ServiceNotImplementedException_Then_TransactionSearchModel_Should_Contain_Uknown_Error_Message()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .WithBlockSearchServiceThrowingServiceNotImplementedException()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.ErrorMessage.ShouldBe(TransactionSearchControllerTestHarness.UNKNOWN_ERROR_MESSAGE);
        }

        [TestMethod]
        public void When_BlockSearchService_Throws_InitialisationFailureException_Then_TransactionSearchModel_Should_Contain_Uknown_Error_Message()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .WithBlockSearchServiceThrowingInitialisationFailureException()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.ErrorMessage.ShouldBe(TransactionSearchControllerTestHarness.UNKNOWN_ERROR_MESSAGE);
        }

        [TestMethod]
        public void When_BlockSearchService_Throws_General_Exception_Then_TransactionSearchModel_Should_Contain_Uknown_Error_Message()
        {
            // arrange
            _testHarness = new TransactionSearchControllerTestHarness()
                .WithBlockSearchServiceThrowingGeneralException()
                .Build();

            // act
            var model = _testHarness.Execute_IndexPost();

            // assert
            model.ErrorMessage.ShouldBe(TransactionSearchControllerTestHarness.UNKNOWN_ERROR_MESSAGE);
        }
    }
}
