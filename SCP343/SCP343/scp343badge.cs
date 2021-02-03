using System;
using Exiled.API.Features;
namespace SCP343
{
    public class scp343badge
    {
        public scp343badge(Player player)
        {
            rankname = player.RankName;
            rankcolor = player.RankColor;
            Id = player.Id;
            UserId = player.UserId;
        }
        public string UserId { get; }
        public int Id { get; }
        public string rankname { get; }
        public string rankcolor { get; }
        public bool opendoor { get; set; } = false;
        public bool heck { get; set; } = false;
    }
}
