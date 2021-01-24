using System;
using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(PlayableScps.Scp096),nameof(PlayableScps.Scp096.AddTarget))]
    public static class SCP096
    {
        public static bool Postfix(PlayableScps.Scp096 scp096, GameObject target)
        {
            return !HandlersPl.Players.Active343AndBadgeDict.Contains(Player.Get(target).Id);
        }
    }
}
