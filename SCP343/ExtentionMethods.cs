using System;
using Exiled.API.Features;
namespace SCP343
{
    public static class ExtentionMethods
    {
        internal static bool IsSCP343(this Player player)
        {
            return HandlersPl.Players.Active343.Contains(player.Id);
        }
        internal static string GetBadgeName (this Player player)
        {
            return player.ReferenceHub.serverRoles.NetworkMyText;
        }
        internal static void SetBadgeName(this Player player,string name)
        {
           player.ReferenceHub.serverRoles.NetworkMyText=name;
        }
        internal static string GetBadgeColor(this Player player)
        {
            return player.ReferenceHub.serverRoles.NetworkMyColor;
        }
        internal static void SetBadgeColor(this Player player, string color)
        {
            player.ReferenceHub.serverRoles.NetworkMyColor = color;
        }
    }
}
