using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Models;

namespace BallInChair.Persistence
{
    public interface IPlayerService
    {
        ICollection<PlayerModel> GetAllPlayers();
        
        void CreatePlayer(Guid id, string name);
        PlayerModel GetPlayer(Guid id);
        
        void UpdatePlayerName(Guid id, string name);
    }

    public class InMemoryPlayerService : IPlayerService
    {
        private static readonly ICollection<PlayerModel> Players = new List<PlayerModel>();
        
        private readonly ILedgerService _ledgerService;
        
        public InMemoryPlayerService(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }
        
        public void CreatePlayer(Guid id, string name)
        {
            if(Players.Any(a => a.Id == id))
            {
                throw new Exception("Player with that ID already exists");
            }
            
            Players.Add(new PlayerModel
            {
               Id = id,
               Name = name 
            });
        }

        public ICollection<PlayerModel> GetAllPlayers()
        {
            return Players
                    .Select(a => GetPlayer(a.Id))
                    .ToList();
        }

        public PlayerModel GetPlayer(Guid id)
        {
            var player = Players.FirstOrDefault(a => a.Id == id);
            player.Balance = _ledgerService.GetBalance(id);
            return player;
        }

        public void UpdatePlayerName(Guid id, string name)
        {
            var player = GetPlayer(id);
            player.Name = name;
        }
    }
}