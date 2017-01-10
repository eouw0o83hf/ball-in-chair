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

    public class JsonBackedRoundService : JsonBackedServiceBase<RoundModel>, IRoundService
    {
        private const string RoundsJsonFile = "rounds.json";
        
        private readonly IPlayerService _playerService;
        private readonly ILedgerService _ledgerService;
        private readonly IClock _clock;
        
        public JsonBackedRoundService(IPlayerService playerService, ILedgerService ledgerService, IClock clock, string directory)
            : base(directory, RoundsJsonFile)
        {
            _playerService = playerService;
            _ledgerService = ledgerService;
            _clock = clock;
        }

        public void AddElectionToRound(Guid roundId, Guid playerId)
        {
            var entities = ValidateIds(roundId, playerId);

            _ledgerService.MakeElection(playerId, roundId);

            entities.Item1.EntrantPlayerIds.Add(playerId);            
            UpdateEntity(entities.Item1, a => a.Id == roundId);
        }

        public void DeclareWinner(Guid roundId, Guid playerId)
        {
            var entities = ValidateIds(roundId, playerId);

            _ledgerService.AwardWinnings(playerId, entities.Item1.EntrantPlayerIds.Count);

            entities.Item1.WinningPlayerId = playerId;
            entities.Item1.IsOpen = false;
            UpdateEntity(entities.Item1, a => a.Id == roundId);
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
            return GetPersistedData().ToList();
        }

        public Guid? GetOpenRoundId()
        {
            return GetPersistedData()
                    .Where(a => a.IsOpen)
                    .Select(a => a.Id)
                    .Cast<Guid?>()
                    .FirstOrDefault();
        }

        public RoundModel GetRound(Guid roundId)
        {
            return GetPersistedData().FirstOrDefault(a => a.Id == roundId);
        }

        public void OpenNewRound(Guid roundId)
        {
            var rounds = GetAllRounds();
            if(rounds.Any(a => a.Id == roundId))
            {
                throw new ArgumentException("That ID already exists", nameof(roundId));
            }

            SaveNewEntity(new RoundModel
            {
                Id = roundId,
                RoundNumber = rounds.Any() ? rounds.Max(a => a.RoundNumber) + 1 : 1,
                IsOpen = true,
                Date = _clock.GetCurrentInstant().InZone(DateTimeZone.ForOffset(Offset.FromHours(-7))).Date,
                EntrantPlayerIds = new List<Guid>(),
                WinningPlayerId = null
            });
        }
    }
}