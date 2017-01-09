using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Models;
using NodaTime;

namespace BallInChair.Persistence
{
    public interface ILedgerService
    {
        void MakeElection(Guid playerId);
        void PurchaseCredits(Guid playerId, int creditCount);
        void AwardWinnings(Guid playerId, int winningAmount);

        int GetBalance(Guid playerId);
        ICollection<LedgerEntry> GetPlayerHistory(Guid playerId);
    }

    public class InMemoryLedgerService : ILedgerService
    {
        private static readonly ICollection<LedgerEntry> Ledger = new List<LedgerEntry>();

        private readonly IClock _clock;

        public InMemoryLedgerService(IClock clock)
        {
            _clock = clock;
        }

        public void MakeElection(Guid playerId) => AddEntry(playerId, -1, LedgerType.Election);
        public void PurchaseCredits(Guid playerId, int creditCount) => AddEntry(playerId, creditCount, LedgerType.CreditPurchase);
        public void AwardWinnings(Guid playerId, int winningAmount) => AddEntry(playerId, winningAmount, LedgerType.Winning);

        private void AddEntry(Guid playerId, int amount, LedgerType type)
        {
            Ledger.Add(new LedgerEntry
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId,
                Amount = amount,
                Type = type,
                TransactionTime = _clock.GetCurrentInstant()
            });
        }

        public int GetBalance(Guid playerId)
        {
            return Ledger
                    .Where(a => a.PlayerId == playerId)
                    .Sum(a => a.Amount);
        }

        public ICollection<LedgerEntry> GetPlayerHistory(Guid playerId)
        {
            return Ledger
                    .Where(a => a.PlayerId == playerId)
                    .ToList();
        }
    }
}