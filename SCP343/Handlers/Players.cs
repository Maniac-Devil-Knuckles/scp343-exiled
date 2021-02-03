using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;
using static SCP343.SCP343;
using MEC;
using System.Linq;
using Random = System.Random;
namespace SCP343.HandlersPl
{
    public class Players
    {
        private SCP343 plugin;
        //private bool IsRoundStarted => RoundSummary.RoundInProgress();
        public Players(SCP343 plugin) => this.plugin = plugin;
        private CoroutineHandle checkplayers;
        public void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.Lift.movingSpeed = 1f;
            }
            else ev.Lift.movingSpeed = plugin.Config.lift_moving_speed;
        }
        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List) if (player.IsSCP343()) KillSCP343(player); 
            scp343badgelist.Clear();
            Timing.KillCoroutines(checkplayers);
        }
        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            if (scp343badgelist.Count>0)
            {
                List<Player> mtf = new List<Player>();
                List<Player> classd = new List<Player>();
                List<Player> chaos = new List<Player>();
                List<Player> scps = new List<Player>();
                foreach (Player player in Player.List)
                {
                    if (player.Team == Team.MTF || player.Role == RoleType.Scientist) mtf.Add(player);
                    if (player.Role == RoleType.ClassD && !player.IsSCP343()) classd.Add(player);
                    if (player.Team == Team.SCP) scps.Add(player);
                    if (player.Role == RoleType.ChaosInsurgency) chaos.Add(player);
                }
                if (mtf.Count > 0 && classd.Count == 0 && scps.Count == 0 && chaos.Count == 0) ev.IsRoundEnded = true;
                else if (mtf.Count == 0 && classd.Count == 0 && scps.Count > 0) ev.IsRoundEnded = true;
                else if (mtf.Count > 0 && (classd.Count > 0 || chaos.Count > 0) && scps.Count == 0) ev.IsRoundEnded = false;
                else if (mtf.Count == 0 && classd.Count > 0 && scps.Count == 0) ev.IsRoundEnded = true;
                else if (mtf.Count > 0 && classd.Count == 0 && scps.Count == 0 && chaos.Count > 0) ev.IsRoundEnded = false;
                else if (mtf.Count == 0 && classd.Count == 0 && scps.Count == 0 && chaos.Count == 0) ev.IsRoundEnded = true;
            }
        }

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name.ToLower() == "heck343")
            {
                ev.IsAllowed = false;
                if (ev.Player.IsSCP343())
                {
                    if(!plugin.Config.scp343_heck)
                    {
                        ev.ReturnMessage = plugin.Config.scp343_heckerrordisable;
                        return;
                    }
                    bool allowed = ev.Player.GetSCP343Badge().canheck;
                    if (allowed)
                    {
                            ev.Player.SetRole(RoleType.ClassD);
                            ev.Player.DisableAllEffects();
                            KillSCP343(ev.Player);
                            if (plugin.Config.scp343_alert) ev.Player.Broadcast(10, plugin.Config.scp343_alertbackd);
                            ev.ReturnMessage = plugin.Config.scp343_alertbackd;
                            return;
                    }
                    else
                    {
                        ev.ReturnMessage = $"Error.....{plugin.Config.scp343_alertheckerrortime}";
                    }
                }
                else
                {
                    ev.ReturnMessage = $"Error........{plugin.Config.scp343_alertheckerrornot343}";
                }
            }
        }

        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.IsAllowed = false;
                ev.Position = ev.Scp106.Position;
            }
        }

        public void OnStarting(StartingEventArgs ev)
        {
            if (ev.IsAuto || ev.Player == null) return;
            if (ev.Player.IsSCP343() && !plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnStopping(StoppingEventArgs ev)
        {
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player.IsSCP343() && !plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }
        public void OnChangingLeverStatus(ChangingLeverStatusEventArgs ev)
        {
            if (ev.Player.IsSCP343() && !plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ev.Target.IsSCP343() || ev.Cuffer.IsSCP343()) ev.IsAllowed = false;
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player.IsSCP343() && ev.Player.GetSCP343Badge().canopendoor) ev.IsAllowed = plugin.Config.scp343_canopenanydoor;
        }

        public void OnDied(DiedEventArgs ev)
        {
            Player player = ev.Target;
            if (player.IsSCP343())
            {
                KillSCP343(player);
            }
        }
        public void KillSCP343(Player player)
        {
            if (!player.IsSCP343()) return;
            player.RankColor = player.GetSCP343Badge().rankcolor;
            player.RankName= player.GetSCP343Badge().rankname;
            if (player.Group.HiddenByDefault) player.BadgeHidden = true;
            scp343badgelist.Remove(player);
        }
        Random RNG = new Random();
        public void OnBlinking(BlinkingEventArgs ev)
        {
            ev.Targets.RemoveAll(e => !e.IsSCP343());
        }
        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Target.IsSCP343())
            {
                if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
                {
                    ev.Amount = ev.Target.Health;
                    return;
                }
                else ev.Amount = 0f;
            }
            if (ev.Attacker.IsSCP343()) ev.Amount = 0;
        }
        public void OnRestartingRound()
        {;
            foreach (Player pl in Player.List) if (pl.IsSCP343()) KillSCP343(pl);
            scp343badgelist.Clear();
            Timing.KillCoroutines(checkplayers);
        }
        public void OnRoundStarted()
        {
            scp343badgelist.Clear();
            if (!plugin.Config.IsEnabled)
            {
                plugin.OnDisabled();
                return;
            }
            int chance = Player.UserIdsCache.Count < 2 ? 10000 : RNG.Next(1, 100);
            int count = Player.List.Count();
            if (chance <= plugin.Config.scp343_spawnchance) return;
            if (plugin.Config.minplayers > count && count != 1) return;
            List<Player> ClassDList = new List<Player>();
            foreach (Player play in Player.List)
            {
                if (play.Role == RoleType.ClassD) ClassDList.Add(play);
            }

            checkplayers = Timing.RunCoroutine(scp343badgelist.SetPlayers());
            Player player = ClassDList[RNG.Next(ClassDList.Count)];
            Timing.CallDelayed(0.5f, () =>
            {
                spawn343(player);
                tryplugin(player);
            });

        }//
        public void tryplugin(Player player)
        {
            try
            {
                if (Exiled.Loader.Loader.Plugins.Any(e => e.Name == "Subclass" && e.Author == "Steven4547466" && e.Config.IsEnabled))
                {
                    Subclass.API.RemoveClass(player);
                }

            }
            catch (Exception ex)
            {
                Log.Debug(ex, plugin.Config.Debug);
            }
        }
        public scp343badge spawn343(Player player, bool scp0492 = false)
        {
            if (scp0492)
            {
                KillSCP343(player);
            }
            if (player.IsSCP343()) return player.GetSCP343Badge();
            if (player.BadgeHidden) player.BadgeHidden = false;
           scp343badge badge= new scp343badge(player);
            player.RankColor="red";
            player.RankName="SCP-343";
            if (plugin.Config.scp343_alert && !scp0492)
            {
                player.ClearBroadcasts();
                player.Broadcast(15, plugin.Config.scp343_alerttext);
            }
            if (plugin.Config.scp343_console && !scp0492) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + plugin.Config.scp343_consoletext.Replace("343DOORTIME", plugin.Config.scp343_opendoortime.ToString()).Replace("343HECKTIME", plugin.Config.scp343_hecktime.ToString()) + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000);
                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000, true);
                player.ClearInventory();
                if (!scp0492)
                {
                    foreach (int item in plugin.Config.scp343_itemsatspawn) player.AddItem((ItemType)item);
                }
                if(plugin.Config.scp343_heck) player.GetSCP343Badge().heck =true;
                player.Health = 100f;
            });
            if (plugin.Config.scp343_canopenanydoor) Timing.CallDelayed(plugin.Config.scp343_opendoortime, () => {
                player.GetSCP343Badge().opendoor = true;
            });
            if(plugin.Config.scp343_heck) Timing.CallDelayed(plugin.Config.scp343_hecktime, () =>
            {
                player.GetSCP343Badge().heck = false;
            });
            return badge;
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player.IsSCP343() && scp343badgelist.Get(ev.Player).canopendoor)
            {
                ev.IsAllowed = true;
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            List<ItemType> items = new List<ItemType>();
            foreach (var ite in ev.Player.Inventory.items)
            {
                Log.Debug("Items "+ite.id, plugin.Config.Debug);
                items.Add(ite.id);
            }
            if (ev.Player.IsSCP343() && ev.NewRole != RoleType.Scp0492)
            {
                KillSCP343(ev.Player);
            }
            else if (ev.Player.IsSCP343() && ev.NewRole == RoleType.Scp0492)
            {
                ev.NewRole = RoleType.ClassD;
                Vector3 pos = ev.Player.Position;
                Timing.CallDelayed(0.4f, () => { spawn343(ev.Player, true); });
                Timing.CallDelayed(1.1f, () =>
                {
                    Player player = Player.Get(ev.Player.Id);
                    player.Position = pos;
                    foreach (var item in items)
                    {
                        player.AddItem(item);
                    }
                });
            }
        }

        public void OnDestroyingEvent(DestroyingEventArgs ev)
        {
            if (ev.Player.Id == Server.Host.Id) return;
            if(ev.Player == null || !ev.Player.IsVerified || ev.Player.IPAddress == "127.0.0.WAN" || ev.Player.IPAddress == "127.0.0.1") return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
        }

        public void OnContaining(ContainingEventArgs ev)
        {
            if (ev.ButtonPresser.IsSCP343()) ev.IsAllowed = false;
        }

        public void OnEnraging(EnragingEventArgs ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.IsAllowed = false;
            }
        }
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (ev.Target.IsSCP343())
            {
                ev.AhpToAdd = 0;
                ev.EnrageTimeToAdd = 0;
                ev.IsAllowed = false;
            }
        }

        public void OnActivating(ActivatingEventArgs ev)
        {
            if (ev.Player.IsSCP343()) ev.IsAllowed = false;
        }

        /*public void OnEscaping(EscapingEventArgs ev)
        {
            if(ev.Player.IsSCP343())
            {
                ev.IsAllowed = plugin.Config.scp343_canescape;
            }
        }*/

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player.IsSCP343() && ev.Player.GetSCP343Badge().canopendoor) ev.IsAllowed = true;
        }
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.IsSCP343()) ev.IsTriggerable = false;
        }
        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Player.IsSCP343())
            {
                if(!plugin.Config.scp343_itemconverttoggle)
                {
                    ev.IsAllowed = false;
                    return;
                }
                int itemid = (int)ev.Pickup.ItemId;
                if (plugin.Config.scp343_itemdroplist.IndexOf(itemid) > 0)
                {
                    ev.IsAllowed = false;
                }
                else if (plugin.Config.scp343_itemstoconvert.IndexOf(itemid) > 0)
                {
                    if (!plugin.Config.scp343_itemconverttoggle)
                    {
                        ev.IsAllowed = false;
                        return;
                    }
                    foreach (int i in plugin.Config.scp343_converteditems)
                    {
                        if (i >= 0)
                        {
                            ev.IsAllowed = false;
                            ev.Pickup.Delete();
                            ItemType item = (ItemType)i;
                            ev.Player.AddItem(item);
                        }
                    }
                }
                else ev.IsAllowed = true;
            }
        }
    }
}
