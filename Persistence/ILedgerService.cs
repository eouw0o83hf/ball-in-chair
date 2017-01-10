using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Models;
using NodaTime;

namespace BallInChair.Persistence
{
    public interface ILedgerService
    {
        void MakeElection(Guid playerId, Guid roundId);
        void PurchaseCredits(Guid playerId, int creditCount);
        void AwardWinnings(Guid playerId, int winningAmount);

        int GetBalance(Guid playerId);
        ICollection<LedgerEntry> GetPlayerHistory(Guid playerId);
    }

    public class JsonBackedLedgerService : JsonBackedServiceBase<LedgerEntry>, ILedgerService
    {
        private const string LedgerJsonFile = "ledger.json";

        private readonly IClock _clock;

        public JsonBackedLedgerService(IClock clock, string directory) 
            : base(directory, LedgerJsonFile) 
        { 
            _clock = clock;
        }

        public void MakeElection(Guid playerId, Guid roundId) => AddEntry(playerId, -1, LedgerType.Election, roundId);
        public void PurchaseCredits(Guid playerId, int creditCount) => AddEntry(playerId, creditCount, LedgerType.CreditPurchase);
        public void AwardWinnings(Guid playerId, int winningAmount) => AddEntry(playerId, winningAmount, LedgerType.Winning);

        private void AddEntry(Guid playerId, int amount, LedgerType type, Guid? roundid = null)
        {
            SaveNewEntity(new LedgerEntry
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId,
                Amount = amount,
                Type = type,
                TransactionTime = _clock.GetCurrentInstant(),
                RoundId = roundid
            });
        }

        public int GetBalance(Guid playerId)
        {
            return GetPersistedData()
                    .Where(a => a.PlayerId == playerId)
                    .Sum(a => a.Amount);
        }

        public ICollection<LedgerEntry> GetPlayerHistory(Guid playerId)
        {
            return GetPersistedData()
                    .Where(a => a.PlayerId == playerId)
                    .OrderBy(a => a.TransactionTime)
                    .ToList();
        }
    }
}