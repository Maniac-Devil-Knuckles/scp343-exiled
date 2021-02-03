using System;
using Exiled.API.Features;
using System.Collections.Generic;
using static SCP343.HandlersPl.Players;
namespace SCP343
{
    public static class ExtentionMethods
    {
        public static bool IsSCP343(this Player player)
        {
            return scp343badgelist.Contains(player);
        }
        public static scp343badge GetSCP343Badge(this Player player)
        {
            if (!player.IsSCP343()) return null;
            return scp343badgelist.Get(player.Id);
        }
    }
}
