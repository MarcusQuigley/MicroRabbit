using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService) {
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransferLog>>> GetTransfersAsync() {
            var transfers = await _transferService.GetTransfersAsync();
            return Ok(transfers);
        }
    }

    //video 10 Event handler
}
