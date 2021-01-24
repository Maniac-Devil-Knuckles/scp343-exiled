using System;
using Interactables.Interobjects.DoorUtils;
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
        Dictionary<int, string> colorbadge { get; set; } = new Dictionary<int, string>();
        Dictionary<int, string> namebadge { get; set; } = new Dictionary<int, string>();
        public Players(SCP343 plugin) => this.plugin = plugin;
        public static List<int> Active343AndBadgeDict = new List<int>();
        public static Dictionary<int,bool> hecktime = new Dictionary<int,bool>();
        public static Dictionary<int, bool>  IsOpenAll = new Dictionary<int, bool>();

        public void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Player.Id))
            {
                ev.Lift.movingSpeed = 1f;
            }
            else ev.Lift.movingSpeed = plugin.Config.lift_moving_speed;
        }
        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            Active343AndBadgeDict.Clear();
            colorbadge.Clear();
            hecktime.Clear();
            IsOpenAll.Clear();
        }
        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            if(Active343AndBadgeDict.Count>0)
            {
                List<Player> mtf = new List<Player>();
                List<Player> classd = new List<Player>();
                List<Player> chaos = new List<Player>();
                List<Player> scps = new List<Player>();
                foreach (Player player in Player.List)
                {
                    if (player.Team == Team.MTF || player.Role == RoleType.Scientist) mtf.Add(player);
                    if (player.Role == RoleType.ClassD && !Active343AndBadgeDict.Contains(player.Id)) classd.Add(player);
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

        public void OnCommand(SendingRemoteAdminCommandEventArgs ev)
        {

            //Log.Info(ev.Name);
            if (ev.Name.ToLower() == "spawn343")
            {
                ev.IsAllowed = false;
                ev.Success = true;
                //Log.Info(ev.Arguments.Count);
                try {
                    if (ev.Arguments.Count < 1)
                    {
                        ev.ReplyMessage = "Usage command : \"spawn343 PlayerId\"";
                        return;
                    }
                    if (ev.Arguments.TryGet(0, out string str))
                    {
                        if (int.TryParse(str, out int PlayerId))
                        {
                            if (PlayerId < 2)
                            {
                                ev.ReplyMessage = "Usage command : \"spawn343 PlayerId\"";
                                return;
                            }
                            Player player = Player.Get(PlayerId);
                            //Log.Info(player.UserId);
                            if (player == null)
                            {
                                ev.ReplyMessage = "Incorrect PlayerId";
                                return;
                            }
                            player.SetRole(RoleType.ClassD, false, true);
                            Active343AndBadgeDict.Add(PlayerId);
                            if (plugin.Config.scp343_alert)
                            {
                                player.ClearBroadcasts();
                                player.Broadcast(15, plugin.Config.scp343_alerttext);
                            }
                            if (plugin.Config.scp343_console) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + plugin.Config.scp343_consoletext.Replace("343DOORTIME", plugin.Config.scp343_opendoortime.ToString()).Replace("343HECKTIME", plugin.Config.scp343_hecktime.ToString()) + "\n-----------------------------------------------------------", "green");
                            Timing.CallDelayed(1f, () =>
                            {
                                player.ClearInventory();
                                foreach (int item in plugin.Config.scp343_itemsatspawn) player.AddItem((ItemType)item);
                                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000);
                                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000, true);
                                hecktime.Add(PlayerId, true);
                                IsOpenAll.Add(PlayerId, false);
                            });
                            Timing.CallDelayed(plugin.Config.scp343_opendoortime, () =>
                            {
                                IsOpenAll.Remove(PlayerId);
                                IsOpenAll.Add(PlayerId, true);
                            });
                            
                            Timing.CallDelayed(plugin.Config.scp343_hecktime, () =>
                            {
                                hecktime.Remove(PlayerId);
                                hecktime.Add(PlayerId, false);
                            });
                            colorbadge.Add(PlayerId, player.ReferenceHub.serverRoles.NetworkMyColor);
                            namebadge.Add(PlayerId, player.ReferenceHub.serverRoles.NetworkMyText);
                            player.ReferenceHub.serverRoles.NetworkMyColor = "red";
                            player.ReferenceHub.serverRoles.NetworkMyText = "SCP-343";
                            API.scp343.Add(player);
                            ev.ReplyMessage = $"Made {player.Nickname} SCP-343";
                            return;
                        }
                    }
                    ev.ReplyMessage = "Usage command : \"spawn343 PlayerId\"";
                    return;
                } catch(Exception ex)
                {
                    ev.ReplyMessage = ex.Message;
                    Log.Info(ex);
                }
                }
        }

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if(ev.Name=="heck343")
            {
                ev.IsAllowed = false;
                if(Active343AndBadgeDict.Contains(ev.Player.Id))
                {
                    bool allowed = hecktime.TryGetValue(ev.Player.Id, out bool has);
                   if (allowed)
                    {
                        if (has)
                        {
                            ev.Player.SetRole(RoleType.ClassD);
                            ev.Player.DisableAllEffects();
                            if (colorbadge.TryGetValue(ev.Player.Id, out string color)) ev.Player.ReferenceHub.serverRoles.NetworkMyColor = color;
                            if (namebadge.TryGetValue(ev.Player.Id, out string name)) ev.Player.ReferenceHub.serverRoles.NetworkMyText = name;
                            colorbadge.Remove(ev.Player.Id);
                            namebadge.Remove(ev.Player.Id);
                            IsOpenAll.Remove(ev.Player.Id);
                            hecktime.Remove(ev.Player.Id);
                            Active343AndBadgeDict.Remove(ev.Player.Id);
                            API.scp343.Remove(ev.Player);
                            if (plugin.Config.scp343_alert) ev.Player.Broadcast(10, plugin.Config.scp343_alertbackd);
                            ev.ReturnMessage = plugin.Config.scp343_alertbackd;
                            return;
                        } else
                        {
                            ev.ReturnMessage = $"Error.....{plugin.Config.scp343_alertheckerrortime}";
                        }
                    } else
                    {
                        ev.ReturnMessage = $"Error.....{plugin.Config.scp343_alertheckerrortime}";
                    }
                } else
                {
                    ev.ReturnMessage = $"Error........{plugin.Config.scp343_alertheckerrornot343}";
                }
            }
        }

        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if(Active343AndBadgeDict.Contains(ev.Player.Id))
            {
                ev.IsAllowed = false;
                ev.Position = ev.Scp106.Position;
            }
        }

        public void OnStarting(StartingEventArgs ev)
        {
            if (ev.IsAuto||ev.Player==null) return;
            if (Active343AndBadgeDict.Contains(ev.Player.Id)&&plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnStopping(StoppingEventArgs ev)
        {
            if (ev.Player == null) return;
            if (Active343AndBadgeDict.Contains(ev.Player.Id) && plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Player.Id)&& plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }
        public void OnChangingLeverStatus(ChangingLeverStatusEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Player.Id) && plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Target.Id) || Active343AndBadgeDict.Contains(ev.Cuffer.Id)) ev.IsAllowed = false;
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev) 
        {
            bool isAllowed = IsOpenAll.TryGetValue(ev.Player.Id, out bool opened);
            if (!isAllowed) return;
            if (Active343AndBadgeDict.Contains(ev.Player.Id) && opened) ev.IsAllowed = true;
        }

        public void OnDied(DiedEventArgs ev)
        {
            Player player = ev.Target;
            if (Active343AndBadgeDict.Contains(player.Id))
            {
                Active343AndBadgeDict.Remove(player.Id);
                API.scp343.Remove(player);
            }

        }
        Random RNG = new Random();

        public void OnHurting(HurtingEventArgs ev)
        {
            if(Active343AndBadgeDict.Contains(ev.Target.Id))
            {
                if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
                {
                    ev.Amount = ev.Target.Health;
                    return;
                }else ev.Amount = 0f;
            }
            if (Active343AndBadgeDict.Contains(ev.Attacker.Id)) ev.Amount = 0;
        }
        public void OnRestartingRound()
        {
            Active343AndBadgeDict.Clear();
            IsOpenAll.Clear();
            hecktime.Clear();
            colorbadge.Clear();
            namebadge.Clear();  
        }
        public void OnRoundStarted()
        {
            Active343AndBadgeDict.Clear();
            IsOpenAll.Clear();
            hecktime.Clear();
            colorbadge.Clear();
            namebadge.Clear();
            if (!plugin.Config.IsEnabled)
            {
                plugin.OnDisabled();
                return;
            }
            int chance = Player.UserIdsCache.Count<2?10000:RNG.Next(1, 100);
            if (chance <= plugin.Config.scp343_spawnchance||plugin.Config.minplayers>Player.UserIdsCache.Count) return;
            List<Player> ClassDList = new List<Player>();
            foreach(Player play in Player.List)
            {
                if (play.Role == RoleType.ClassD) ClassDList.Add(play);
            }
            Player player = ClassDList[RNG.Next(ClassDList.Count)];
            spawn343(player);
            try
            {
                if (IsEnabledPluginAdvancedSubclassing)
                {
                    Subclass.API.RemoveClass(player);
                }
                
            } catch(Exception ex)
            {
                Log.Debug(ex, false);
            }
            
        }//
        void spawn343(Player player)
        {
            API.scp343.Add(player);
            Active343AndBadgeDict.Add(player.Id);
            player.ClearInventory();
            colorbadge.Add(player.Id, player.ReferenceHub.serverRoles.NetworkMyColor);
            namebadge.Add(player.Id, player.ReferenceHub.serverRoles.NetworkMyText);
            player.ReferenceHub.serverRoles.NetworkMyColor = "red";
            player.ReferenceHub.serverRoles.NetworkMyText = "SCP-343";
            if (plugin.Config.scp343_alert)
            {
                player.ClearBroadcasts();
                player.Broadcast(15, plugin.Config.scp343_alerttext);
            }
            if (plugin.Config.scp343_console) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + plugin.Config.scp343_consoletext.Replace("343DOORTIME", plugin.Config.scp343_opendoortime.ToString()).Replace("343HECKTIME", plugin.Config.scp343_hecktime.ToString()) + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(1f, () =>
            {
                player.ClearInventory();
                foreach (int item in plugin.Config.scp343_itemsatspawn) player.AddItem((ItemType)item);
                hecktime.Add(player.Id, true);
                IsOpenAll.Add(player.Id, false);
                Active343AndBadgeDict.Add(player.Id);
                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000);
                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000, true);
                player.Health = 100f;
            });
            Timing.CallDelayed(plugin.Config.scp343_opendoortime, () => {
                IsOpenAll.Remove(player.Id);
                IsOpenAll.Add(player.Id, true);
            });
            Timing.CallDelayed(plugin.Config.scp343_hecktime, () =>
            {
                hecktime.Remove(player.Id);
                hecktime.Add(player.Id, false);
            });
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.IsAllowed) return;
            bool allowed = IsOpenAll.TryGetValue(ev.Player.Id, out bool isOpened);
            if (!allowed) return;
            if (Active343AndBadgeDict.Contains(ev.Player.Id)&&isOpened)
            {
                ev.IsAllowed = true;
            }
        }
       
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if(Active343AndBadgeDict.Contains(ev.Player.Id))
            {
                API.scp343.Remove(ev.Player);
                Active343AndBadgeDict.Remove(ev.Player.Id);
                //Log.Info(namebadge[ev.Player.Id]);
                if (colorbadge.TryGetValue(ev.Player.Id,out string color)) ev.Player.ReferenceHub.serverRoles.NetworkMyColor=color;
                if(namebadge.TryGetValue(ev.Player.Id, out string name)) ev.Player.ReferenceHub.serverRoles.NetworkMyText = name;
                colorbadge.Remove(ev.Player.Id);
                namebadge.Remove(ev.Player.Id);
                IsOpenAll.Remove(ev.Player.Id);
                hecktime.Remove(ev.Player.Id);
            }
        }

        
        public void OnContaining(ContainingEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.ButtonPresser.Id)) ev.IsAllowed = false;
        }

        
        public void OnActivating(ActivatingEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Player.Id)) ev.IsAllowed = false;
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            if(Active343AndBadgeDict.Contains(ev.Player.Id))
            {
                ev.IsAllowed = false;
            }
        }

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Player.Id)&&IsOpenAll.ContainsKey(ev.Player.Id)) ev.IsAllowed = true;
        }
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.Player.Id)) ev.IsTriggerable = false;
        }
        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if(Active343AndBadgeDict.Contains(ev.Player.Id))
            {
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
                    foreach(int i in plugin.Config.scp343_converteditems)
                    {
                        if(i>=0)
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
