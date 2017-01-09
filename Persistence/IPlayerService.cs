using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Models;
using Newtonsoft.Json;

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
            return Players
                    .Select(a => GetPlayer(a.Id))
                    .ToList();
        }

        public PlayerModel GetPlayer(Guid id)
        {
            var player = Players.FirstOrDefault(a => a.Id == id);
            return player;
        }

        public void UpdatePlayerName(Guid id, string name)
        {
            var player = GetPlayer(id);
            player.Name = name;
        }
    }

    public class JsonBackedPlayerService : JsonBackedServiceBase<PlayerModel>, IPlayerService
    {       
        private const string PlayerJsonFile = "players.json";
         
        public JsonBackedPlayerService(string persistenceDirectory)
            : base(persistenceDirectory, PlayerJsonFile) { }
        
        public void CreatePlayer(Guid id, string name)
        {
            var existingPlayers = GetAllPlayers();
            if(existingPlayers.Any(a => a.Id == id))
            {
                throw new ArgumentException("Player with that ID already exists", nameof(id));
            }

            name = name.Trim();
            if(existingPlayers.Any(a => a.Name == name))
            {
                throw new ArgumentException("Player with that name already exists", nameof(name));
            }

            SaveNewEntity(new PlayerModel
            {
                Id = id,
                Name = name
            });
        }

        public ICollection<PlayerModel> GetAllPlayers()
        {
            return GetPersistedData().ToList();
        }

        public PlayerModel GetPlayer(Guid id)
        {
            return GetAllPlayers().SingleOrDefault(a => a.Id == id);
        }

        public void UpdatePlayerName(Guid id, string name)
        {
            var player = GetPlayer(id);
            player.Name = name;
            UpdateEntity(player, a => a.Id == id);
        }
    }
}