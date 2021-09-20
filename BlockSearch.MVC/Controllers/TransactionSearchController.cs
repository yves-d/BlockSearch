using BlockSearch.Application;
using BlockSearch.Application.Exceptions;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Models;
using BlockSearch.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlockSearch.MVC.Controllers
{
    public class TransactionSearchController : Controller
    {
        private IBlockSearchService _blockSearchService;

        public TransactionSearchController(IBlockSearchService blockSearchService)
        {
            _blockSearchService = blockSearchService;
        }

        // GET: TransactionSearchController
        public ActionResult Index()
        {
            var searchModel = new TransactionSearchModel()
            {
                Crypto = CryptoType.Ethereum // defaulting to Eth
            };
            return View(searchModel);
        }

        // POST: TransactionSearchController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(TransactionSearchModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Crypto = CryptoType.Ethereum;
                return View(model);
            }

            try
            {
                var block = await _blockSearchService.GetAddressTransactionsInBlock(model.Crypto, model.BlockNumber, model.Address);
                return View(BuildSuccesfulResponse(block));
            }
            catch(Exception ex)
            {
                if (CanShowExceptionMessageToUser(ex))
                    return View(BuildErrorResponse(ex.Message));
                
                return View(BuildErrorResponse("An unknown error has occurred."));
            }
        }

        private TransactionSearchModel BuildSuccesfulResponse(Block block)
        {
            return new TransactionSearchModel()
            {
                BlockNumber = int.Parse(block.Number),
                Address = block.Address,
                Crypto = block.Crypto,
                Transactions = block.Transactions.Select(x => new TransactionModel()
                {
                    BlockHash = x.BlockHash,
                    BlockNumber = x.BlockNumber,
                    From = x.From,
                    To = x.To,
                    Hash = x.Hash,
                    Gas = x.Gas,
                    Value = x.Value
                }).ToList()
            };
        }

        private TransactionSearchModel BuildErrorResponse(string message)
        {
            return new TransactionSearchModel()
            {
                Crypto = CryptoType.Ethereum,
                ErrorMessage = message
            };
        }

        private bool CanShowExceptionMessageToUser(Exception ex)
        {
            return
                (ex is BlockNotFoundException)
                || (ex is InvalidInputException);
        }
    }
}
