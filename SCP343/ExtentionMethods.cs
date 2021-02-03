using System;
using Exiled.API.Features;
using System.Collections.Generic;
using static SCP343.HandlersPl.Players;
namespace SCP343
{
    public static class ExtentionMethods
    {
        internal static bool IsSCP343(this Player player)
        {
            if (SCP343.players.scp343badge == null) return false;
            return SCP343.players.scp343badge.UserId == player.UserId;
        }
    }
}
