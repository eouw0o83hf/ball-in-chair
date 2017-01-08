using System;

namespace BallInChair.Models
{
    public class PlayerModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int Balance { get; set; }
    }
}