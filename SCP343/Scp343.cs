using System;
using Exiled;
using Exiled.API;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events;
using Exiled.Events.Commands.Reload;
using SCP343.HandlersPl;
//using SCP343.Commands;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using Scps914 = Exiled.Events.Handlers.Scp914;
using SCP106 = Exiled.Events.Handlers.Scp106;
using WARHEAD = Exiled.Events.Handlers.Warhead;
using Exiled.API.Enums;
using HarmonyLib;
using System.Collections.Generic;

namespace SCP343
{
    public class API
    {
        public static List<Exiled.API.Features.Player> scp343 = new List<Exiled.API.Features.Player>();
    }

    public class SCP343 : Plugin<Config>
    {
		public Players players { get; private set; }
		public override string Name => "SCP-343";
        public override string Prefix => "SCP-343";
        public override string Author => "Maniac Devil Knuckles";
		public override Version Version { get; } = new Version(1, 3, 0);

        public override PluginPriority Priority => PluginPriority.Highest;

        public SCP343() { }
		public override void OnEnabled() //22>19
		{
			players = new Players(this);
            Log.Info("Enabling SCP343 by Maniac Devil Knuckles");
            Server.EndingRound += players.OnRoundEnding;
            Player.ChangingRole += players.OnChangingRole;
            Player.TriggeringTesla += players.OnTriggeringTesla;
            Server.RoundStarted += players.OnRoundStarted;
            Player.Died += players.OnDied;
            Player.Hurting += players.OnHurting;
            Player.Escaping += players.OnEscaping;
            Player.UnlockingGenerator += players.OnUnlockingGenerator;
            Scps914.Activating += players.OnActivating;
            SCP106.Containing += players.OnContaining;
            Player.PickingUpItem += players.OnPickingUpItem;
            Player.InteractingLocker += players.OnInteractingLocker;
            Player.Handcuffing += players.OnHandcuffing;
            Server.SendingConsoleCommand += players.OnSendingConsoleCommand;
            Player.InteractingDoor += players.OnInteractingDoor;
            WARHEAD.ChangingLeverStatus += players.OnChangingLeverStatus;
            WARHEAD.Starting += players.OnStarting;
            WARHEAD.Stopping += players.OnStopping;
            Player.ActivatingWarheadPanel += players.OnActivatingWarheadPanel;
            Player.EnteringPocketDimension += players.OnEnteringPocketDimension;
            Server.SendingRemoteAdminCommand += players.OnCommand;
            Player.InteractingElevator += players.OnInteractingElevator;
            Server.RoundEnded += players.OnRoundEnd;
            Server.RestartingRound += players.OnRestartingRound;
        }

		public override void OnDisabled()
		{
            Log.Info("Disabling SCP343 by Maniac Devil Knuckles");
            Player.ChangingRole -= players.OnChangingRole;
            Player.TriggeringTesla -= players.OnTriggeringTesla;
            Server.RoundStarted -= players.OnRoundStarted;
            Player.Died -= players.OnDied;
            Player.Hurting -= players.OnHurting;
            Player.Escaping -= players.OnEscaping;
            Player.UnlockingGenerator -= players.OnUnlockingGenerator;
            Scps914.Activating -= players.OnActivating;
            SCP106.Containing -= players.OnContaining;
            Player.PickingUpItem -= players.OnPickingUpItem;
            Player.InteractingLocker -= players.OnInteractingLocker;
            Player.Handcuffing -= players.OnHandcuffing;
            Server.SendingConsoleCommand -= players.OnSendingConsoleCommand;
            Player.InteractingDoor -= players.OnInteractingDoor;
            WARHEAD.ChangingLeverStatus -= players.OnChangingLeverStatus;
            WARHEAD.Starting -= players.OnStarting;
            WARHEAD.Stopping -= players.OnStopping;
            Player.ActivatingWarheadPanel -= players.OnActivatingWarheadPanel;
            Player.EnteringPocketDimension -= players.OnEnteringPocketDimension;
            players = null; ;
        }

		public override void OnReloaded() { }
	}
}
