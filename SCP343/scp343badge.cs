using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;

namespace SCP343
{
    /// <summary>
    /// This class does :
    /// - keeps UserId, PlayerId, original RankName and original RankColor
    /// - checks if player can open doors and executing command .heck343
    /// </summary>
    public class scp343badge
    {
        /// <summary>
        /// This creates badge of <see cref="Player"/> as scp343.
        /// </summary>
        public scp343badge(Player player)
        {
            rankname = player.RankName;
            rankcolor = player.RankColor;
            Id = player.Id;
            UserId = player.UserId;
            this.player = player;
            scp343badgelist.Add(this);
        }
        public string UserId { get; }
        public int Id { get; }
        public string rankname { get; }
        public string rankcolor { get; }
        internal bool opendoor { get; set; } = false;
        internal bool heck { get; set; } = false;
        public bool canopendoor => opendoor;
        public bool canheck => heck;
        internal Player player { get; set; }
    }
    /// <summary>
    /// This list of badges of scp343 <see cref="Player"/>.
    /// </summary>
    public class scp343badgelist
    {
        private static bool RoundIsStarted => RoundSummary.RoundInProgress();
        private static Dictionary<int, scp343badge> badges { get; set; } = new Dictionary<int, scp343badge>();
        internal static List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            foreach (var badge in badges) players.Add(badge.Value.player);
            return players;
        }
        internal static IEnumerator<float> SetPlayers()
        {
            while(RoundIsStarted)
            {
                foreach(var player in GetPlayers())
                {
                    badges[player.Id].player = Player.Get(player.Id);
                    yield return Timing.WaitForSeconds(1f);
                }
            }
        }
        internal static void Add(scp343badge scp343) => badges.Add(scp343.Id,scp343);
        internal static void Remove(Player player) => badges.Remove(player.Id);
        internal static void Remove(int PlayerId) => badges.Remove(PlayerId);
        internal static void Clear() => badges.Clear();
        /// <summary>
        /// This returns if <see cref="Player"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(Player player) => badges.ContainsKey(player.Id);
        /// <summary>
        /// This returns if <see cref="Player.Id"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(int PlayerId) => badges.ContainsKey(PlayerId);
        /// <summary>
        /// Count of scp343
        /// </summary>
        public static int Count => badges.Count;
        /// <summary>
        /// Get Badge by <see cref="Player"/> and returns <seealso cref="scp343badge"/>
        /// </summary>
        public static scp343badge Get(Player player) => badges[player.Id];
        /// <summary>
        /// Get Badge by <see cref="Player.Id"/> and returns <seealso cref="scp343badge"/>
        /// </summary>
        public static scp343badge Get(int PlayerId) => badges[PlayerId];
    }
}
