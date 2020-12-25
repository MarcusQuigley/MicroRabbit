using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _context;

        public TransferRepository(TransferDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TransferLog>> GetTransfersAsync( ) {

            return await _context.TransferLogs.ToListAsync();
        }
    }
}
