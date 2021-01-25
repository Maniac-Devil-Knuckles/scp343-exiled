using System;
using Exiled.API.Features;
using HarmonyLib;
using Mirror;

namespace SCP343.Patches
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRegisterEscape))]
	class EscapePatch
	{
		public static bool Prefix(CharacterClassManager __instance)
		{
			if (!Player.Get(((NetworkBehaviour)__instance).gameObject).IsSCP343()) return false;
			return SCP343.instance.Config.scp343_canescape;
		}
	}
}	