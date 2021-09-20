using BlockSearch.Application.CryptoService;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Exceptions;
using BlockSearch.Common.Models;
using BlockSearch.Infrastructure.Logger;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlockSearch.Application
{
    public class BlockSearchService : IBlockSearchService
    {
        private readonly ILoggerAdapter<IBlockSearchService> _logger;
        private readonly ICryptoServiceFactory _cryptoServiceFactory;

        public BlockSearchService(ILoggerAdapter<IBlockSearchService> logger, ICryptoServiceFactory cryptoServiceFactory)
        {
            _logger = logger;
            _cryptoServiceFactory = cryptoServiceFactory;
        }

        public async Task<Block> GetAddressTransactionsInBlock(CryptoType? cryptoType, int? blockNumber, string address)
        {
            try
            {
                ValidateInput(cryptoType, blockNumber);
                var cryptoService = _cryptoServiceFactory.GetCryptoService(cryptoType.Value);
                var block = await cryptoService.GetBlockByBlockNumber(blockNumber.Value);
                return FilterBlockTransactionsByAddress(block, address);
            }
            catch(Exception ex)
            {
                if(ExceptionShouldBeLogged(ex))
                    _logger.LogError(ex.Message);
                
                throw;
            }
        }

        private void ValidateInput(CryptoType? cryptoType, int? blockNumber)
        {
            if (!cryptoType.HasValue)
                throw new InvalidInputException($"Missing input - cryptoType");

            if (!blockNumber.HasValue)
                throw new InvalidInputException("Missing input - blockNumber");
        }

        private Block FilterBlockTransactionsByAddress(Block block, string address)
        {
            if (string.IsNullOrEmpty(address))
                return block;

            var filteredTransactions = block.Transactions.Where(transaction =>
                    transaction.From == address
                    || transaction.To == address).ToList();

            block.Transactions = filteredTransactions;
            block.Address = address;
            
            return block;
        }

        private bool ExceptionShouldBeLogged(Exception ex)
        {
            return
                !(ex is BlockNotFoundException)
                && !(ex is InvalidInputException);
        }
    }
}
