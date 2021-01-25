using System;
using Exiled.API.Features;
namespace SCP343
{
    public static class ExtentionMethods
    {
        internal static bool IsSCP343(this Player player)
        {
            return HandlersPl.Players.Active343AndBadgeDict.Contains(player.Id);
        }
    }
}
