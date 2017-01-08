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
            return Players.ToList();
        }

        public PlayerModel GetPlayer(Guid id)
        {
            return Players.FirstOrDefault(a => a.Id == id);
        }

        public void UpdatePlayerName(Guid id, string name)
        {
            var player = GetPlayer(id);
            player.Name = name;
        }
    }
}