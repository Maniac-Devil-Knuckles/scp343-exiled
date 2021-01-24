﻿
using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace SCP343.Patches
{
	[HarmonyPatch(typeof(Scp939PlayerScript), nameof(Scp939PlayerScript.CallCmdShoot))]
	class Scp939Attack
	{
		public static void Postfix(Scp939PlayerScript __instance, GameObject target)
		{
			Player player = Player.Get(target);
			if (API.scp343.Contains(player))
			{
				player.ReferenceHub.playerEffectsController.DisableEffect<CustomPlayerEffects.Amnesia>();
			}
		}
	}
}