
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
using SCP096 = Exiled.Events.Handlers.Scp096;
using Exiled.API.Enums;
using HarmonyLib;
using System.Collections.Generic;
using Exiled.Loader;
using System.Linq;
using System.ComponentModel;

namespace SCP343
{
    public class API
    {
        /// <summary>
        /// Get random SCP343 on the server <see cref="Exiled.API.Features.Player"/>.
        /// </summary>
        public static Exiled.API.Features.Player GetSCP343()
        {
                Exiled.API.Features.Player l = null;
                foreach (Exiled.API.Features.Player p in Exiled.API.Features.Player.List)
                {
                    if (p.IsSCP343()) l = p;
                }
                return l;
        }
        /// <summary>
        /// Get ALL list scp343 on the server and returns list of <see cref="Exiled.API.Features.Player>"/>.
        /// </summary>
        public static List<Exiled.API.Features.Player> scp343list
        {
            get
            {
                List<Exiled.API.Features.Player> players = new List<Exiled.API.Features.Player>();
                foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) if (player.IsSCP343()) players.Add(player);
                return players;
            }
        }
        /// <summary>
        /// This spawns player as scp343 and returns <see cref="scp343badge"/>
        /// </summary>
        public static scp343badge Spawn343(Exiled.API.Features.Player player)
        {
            scp343badge scp343badge =SCP343.players.spawn343(player);
            SCP343.players.tryplugin(player);
            return scp343badge;
        }
        /// <summary>
        /// This returns List of <see cref="scp343badge"/>
        /// </summary>
        public static Dictionary<int,scp343badge> Scp343BadgesList
        {
            get
            {
                Dictionary<int, scp343badge> badges = new Dictionary<int, scp343badge>();
                foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) if (player.IsSCP343()) badges.Add(player.Id, player.GetSCP343Badge());
                return badges;
            }
        }
        /// <summary>
        /// This kills scp343
        /// </summary>
        public static void Kill343(Exiled.API.Features.Player player) => SCP343.players.KillSCP343(player);
    }

    public class SCP343 : Plugin<Config>
    {
		public static Players players { get; private set; }
        
        public override string Name => "SCP-343";
        public override string Prefix => "SCP-343";
        public override string Author => "Maniac Devil Knuckles";
		public override Version Version { get; } = new Version(1, 3, 0);
        public override Version RequiredExiledVersion => new Version(2,1,30);
        public override PluginPriority Priority => PluginPriority.Highest;
        public static SCP343 instance;
        public Harmony harmony { get; set; } = null;
        private int i { get; set; } = 0;
        public SCP343() { }
        //public static bool IsEnabledPluginAdvancedSubclassing => Loader.Plugins.Any(p => p.Name == "Subclass" && p.Config.IsEnabled);
        public override void OnEnabled() //22>19
		{
            instance = this;
            try
            {
                harmony = new Harmony("knuckles.scp343\nVersion" + i++);
                harmony.PatchAll();
                Log.Info("cool");
            } catch(Exception ex)
            {
                Log.Info("error\n\n\n\n\n\n\n\\n\n");
                Log.Info(ex.Message);//
            }
			players = new Players(this);
            Log.Info("Enabling SCP343 by Maniac Devil Knuckles");
            Server.EndingRound += players.OnRoundEnding;
            Player.ChangingRole += players.OnChangingRole;
            Player.TriggeringTesla += players.OnTriggeringTesla;
            Server.RoundStarted += players.OnRoundStarted;
            Player.Died += players.OnDied;
            Player.Hurting += players.OnHurting;
            //Player.Escaping += players.OnEscaping;
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
            //Server.SendingRemoteAdminCommand += players.OnCommand;
            Player.InteractingElevator += players.OnInteractingElevator;
            Server.RoundEnded += players.OnRoundEnd;
            Server.RestartingRound += players.OnRestartingRound;
            SCP096.AddingTarget += players.OnAddingTarget;
            SCP096.Enraging += players.OnEnraging;
            Exiled.Events.Handlers.Scp173.Blinking+=players.OnBlinking;
        }

        public override void OnDisabled()
		{
            Log.Info("Disabling SCP343 by Maniac Devil Knuckles");
            harmony.UnpatchAll();
            harmony = null;
            Player.ChangingRole -= players.OnChangingRole;
            Player.TriggeringTesla -= players.OnTriggeringTesla;
            Server.RoundStarted -= players.OnRoundStarted;
            Player.Died -= players.OnDied;
            Player.Hurting -= players.OnHurting;
            Player.Destroying += players.OnDestroyingEvent;
            //Player.Escaping -= players.OnEscaping;
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
