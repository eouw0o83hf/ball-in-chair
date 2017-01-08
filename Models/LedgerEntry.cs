using NodaTime;
using System;

namespace BallInChair.Models
{
    public class LedgerEntry
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public int Amount { get; set; }

        public LedgerType Type { get; set; }

        public Instant TransactionTime { get; set; }
    }

    public enum LedgerType
    {
        BootstrapBackfill,
        CreditPurchase,
        Election,
        Winning,
        CashOut
    }
}