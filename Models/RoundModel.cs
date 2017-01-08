using NodaTime;
using System;
using System.Collections.Generic;

namespace BallInChair.Models
{
    public class RoundModel
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public LocalDate Date { get; set; }

        public List<Guid> EntrantPlayerIds { get; set; }
        public Guid? WinningPlayerId { get; set; }
    }
}