using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Models;
using NodaTime;

namespace BallInChair.Persistence
{
    public interface IRoundService
    {        
        RoundModel GetRound(Guid roundId);
        ICollection<RoundModel> GetAllRounds();

        void OpenNewRound(Guid roundId);
        Guid? GetOpenRoundId();

        void AddElectionToRound(Guid roundId, Guid playerId);
        void DeclareWinner(Guid roundId, Guid playerId);
    }

    public class InMemoryRoundService : IRoundService
    {
        private static readonly ICollection<RoundModel> Rounds = new List<RoundModel>();
        
        private readonly IPlayerService _playerService;
        private readonly ILedgerService _ledgerService;
        private readonly IClock _clock;
        
        public InMemoryRoundService(IPlayerService playerService, ILedgerService ledgerService, IClock clock)
        {
            _playerService = playerService;
            _ledgerService = ledgerService;
            _clock = clock;
        }

        public void AddElectionToRound(Guid roundId, Guid playerId)
        {
            var entities = ValidateIds(roundId, playerId);

            _ledgerService.MakeElection(playerId);
            entities.Item1.EntrantPlayerIds.Add(playerId);
        }

        public void DeclareWinner(Guid roundId, Guid playerId)
        {
            var entities = ValidateIds(roundId, playerId);

            entities.Item1.WinningPlayerId = playerId;
            entities.Item1.IsOpen = false;
            _ledgerService.AwardWinnings(playerId, entities.Item1.EntrantPlayerIds.Count);
        }

        private Tuple<RoundModel, PlayerModel> ValidateIds(Guid roundId, Guid playerId)
        {
            var player = _playerService.GetPlayer(playerId);
            if(player == null)
            {
                throw new ArgumentException("No player of that ID found", nameof(playerId));
            }

            var balance = _ledgerService.GetBalance(playerId);
            if(balance <= 0)
            {
                throw new InvalidOperationException("Player doesn't have any elections available");
            }

            var round = GetRound(roundId);
            if(!round.IsOpen)
            {
                throw new ArgumentException("That Round ID is not open", nameof(roundId));
            }

            return Tuple.Create(round, player);
        }

        public ICollection<RoundModel> GetAllRounds()
        {
            return Rounds.ToList();
        }

        public Guid? GetOpenRoundId()
        {
            return Rounds
                    .Where(a => a.IsOpen)
                    .Select(a => a.Id)
                    .Cast<Guid?>()
                    .FirstOrDefault();
        }

        public RoundModel GetRound(Guid roundId)
        {
            return Rounds.FirstOrDefault(a => a.Id == roundId);
        }

        public void OpenNewRound(Guid roundId)
        {
            if(Rounds.Any(a => a.Id == roundId))
            {
                throw new ArgumentException("That ID already exists", nameof(roundId));
            }

            Rounds.Add(new RoundModel
            {
                Id = roundId,
                RoundNumber = Rounds.Any() ? Rounds.Max(a => a.RoundNumber) + 1 : 1,
                IsOpen = true,
                Date = _clock.GetCurrentInstant().InZone(DateTimeZone.ForOffset(Offset.FromHours(-7))).Date,
                EntrantPlayerIds = new List<Guid>(),
                WinningPlayerId = null
            });
        }
    }
}