using BlockSearch.Application;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Exceptions;
using BlockSearch.Common.Models;
using BlockSearch.MVC.Controllers;
using BlockSearch.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace BlockSearch.MVC.Tests
{
    public class TransactionSearchControllerTestHarness
    {
        // controller
        private TransactionSearchController _controller;

        // injectables
        private IBlockSearchService _blockSearchService;

        // test variables
        private TransactionSearchModel _requestModel;
        private TransactionSearchModel _responseModel;

        private CryptoType? _cryptoType;
        private int? _blockNumber;
        private string _address;

        public const string UNKNOWN_ERROR_MESSAGE = "An unknown error has occurred.";
        public const string BLOCK_NOT_FOUND_MESSAGE = "Block with that number was not found.";
        public const string INVALID_INPUT_MESSAGE = "Missing input - Block Number";

        public Block _block { get; private set; }

        public TransactionSearchControllerTestHarness()
        {
            _blockSearchService = Substitute.For<IBlockSearchService>();

            InitialiseValidTestVariables();
            InitialiseValidTransactionSearchModelRequest();
            InitialiseValidBlockFromBlockSearchService();
        }

        #region SETUP

        private void InitialiseValidTestVariables()
        {
            _blockNumber = 1;
            _cryptoType = CryptoType.Ethereum;
            _address = "0xea674fdde714fd979de3edf0f56aa9716b898ec8";
        }

        private void InitialiseValidTransactionSearchModelRequest()
        {
            _requestModel = new TransactionSearchModel()
            {
                BlockNumber = _blockNumber,
                Crypto = _cryptoType,
                Address = _address
            };
        }

        private void InitialiseValidBlockFromBlockSearchService()
        {
            _block = GetBlockWithOneTransaction();
            _blockSearchService.GetAddressTransactionsInBlock(Arg.Any<CryptoType?>(), Arg.Any<int?>(), Arg.Any<string>())
                .Returns(_block);
        }

        #endregion

        #region ARRANGE

        public TransactionSearchControllerTestHarness WithEmptyCryptoType()
        {
            _cryptoType = null;
            return this;
        }

        public TransactionSearchControllerTestHarness WithBlockSearchServiceThrowingServiceNotImplementedException()
        {
            _blockSearchService.When(service => service.GetAddressTransactionsInBlock(
                    Arg.Any<CryptoType>(),
                    Arg.Any<int?>(),
                    Arg.Any<string>()))
                .Do(service => { throw new ServiceNotImplementedException("Crypto Service not implemented - Bitcoin"); });

            return this;
        }

        public TransactionSearchControllerTestHarness WithBlockSearchServiceThrowingBlockNotFoundException()
        {
            _blockSearchService.When(service => service.GetAddressTransactionsInBlock(
                    Arg.Any<CryptoType>(),
                    Arg.Any<int?>(),
                    Arg.Any<string>()))
                .Do(service => { throw new BlockNotFoundException(BLOCK_NOT_FOUND_MESSAGE); });

            return this;
        }

        public TransactionSearchControllerTestHarness WithBlockSearchServiceThrowingInvalidInputException()
        {
            _blockSearchService.When(service => service.GetAddressTransactionsInBlock(
                    Arg.Any<CryptoType>(),
                    Arg.Any<int?>(),
                    Arg.Any<string>()))
                .Do(service => { throw new InvalidInputException(INVALID_INPUT_MESSAGE); });

            return this;
        }

        public TransactionSearchControllerTestHarness WithBlockSearchServiceThrowingInitialisationFailureException()
        {
            _blockSearchService.When(service => service.GetAddressTransactionsInBlock(
                    Arg.Any<CryptoType>(),
                    Arg.Any<int?>(),
                    Arg.Any<string>()))
                .Do(service => { throw new InitialisationFailureException("Failed to initialise NethereumClient"); });

            return this;
        }

        public TransactionSearchControllerTestHarness WithBlockSearchServiceThrowingGeneralException()
        {
            _blockSearchService.When(service => service.GetAddressTransactionsInBlock(
                    Arg.Any<CryptoType>(),
                    Arg.Any<int?>(),
                    Arg.Any<string>()))
                .Do(service => { throw new Exception("Unknown exception"); });

            return this;
        }

        public TransactionSearchControllerTestHarness Build()
        {
            _controller = new TransactionSearchController(_blockSearchService);
            return this;
        }

        #region HELPERS

        private Block GetBlockWithOneTransaction()
        {
            return new Block()
            {
                Address = _address,
                Crypto = _cryptoType.Value,
                Hash = "",
                Number = _blockNumber.ToString(),
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        BlockHash = "0x8acd2cfcad505faf70e96ef3db426e7f3a2fef4dad02d69be4766aa9ddffb426",
                        BlockNumber = _blockNumber.ToString(),
                        Gas = "0",
                        Hash = "0x6b567b7a2513ecad92bb6217d597b106abfff8911d85d12a274e7566c8ccc159",
                        From = _address,
                        To = "0xb172299614d6601f4a88a3e67498f5b37ed5ff45",
                        Value = (0.01m).ToString()
                    }
                }
            };
        }

        #endregion

        #endregion

        #region ACT

        public TransactionSearchModel Execute_IndexPost()
        {
            var actionResult = _controller.Index(_requestModel).Result;
            return (TransactionSearchModel)((ViewResult)actionResult).Model;
        }

        #endregion
    }
}
