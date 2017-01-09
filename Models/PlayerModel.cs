using System;
using Newtonsoft.Json;

namespace BallInChair.Models
{
    public class PlayerModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }
}