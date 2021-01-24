using System;
using Exiled.API.Features;
using HarmonyLib;
using Mirror;

namespace SCP343.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRegisterEscape))]
	class EscapePatch
	{
		public static bool Prefix(CharacterClassManager __instance) => !HandlersPl.Players.Active343AndBadgeDict.Contains(Player.Get(((NetworkBehaviour)__instance).gameObject).Id);
	}
}