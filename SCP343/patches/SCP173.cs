using System;
using Exiled.API.Features;
using UnityEngine;
using HarmonyLib;
using System.Reflection.Emit;
using System.Collections.Generic;
using static HarmonyLib.AccessTools;
using NorthwoodLib.Pools;
using System.Linq;

namespace SCP343.Patches
{
   /* //[HarmonyPatch(typeof(Scp173PlayerScript),nameof(Scp173PlayerScript.LookFor173))]
    static class Scp173LookFor173Patch
    {
		public static bool Postfix(Scp173PlayerScript _instance,GameObject scp, bool angleCheck)
        {
            Log.Info("is working?");
            Player player = Player.Get(scp);
            if (player.Team == Team.SCP) return true;
            if (player.Id != API.GetSCP343().Id) return false;
            return true;
            //return SCP343.instance.Config.scp343_canstopscp173;
        }
	}*/
}
