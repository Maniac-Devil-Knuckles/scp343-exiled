using System;
using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(PlayableScps.Scp096),nameof(PlayableScps.Scp096.AddTarget))]
    public static class SCP096
    {
        public static bool Prefix(GameObject target)
        {
            return !Player.Get(target).IsSCP343();
        }
    }
}
