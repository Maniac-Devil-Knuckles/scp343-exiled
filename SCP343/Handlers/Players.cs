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
using RemoteAdmin;

namespace SCP343.HandlersPl
{
    public class Players
    {
        private SCP343 plugin;
        private bool IsRoundStarted => RoundSummary.RoundInProgress();
        Dictionary<int, string> colorbadge { get; set; } = new Dictionary<int, string>();
        Dictionary<int, string> namebadge { get; set; } = new Dictionary<int, string>();
        public Players(SCP343 plugin) => this.plugin = plugin;
        public static List<int> Active343AndBadgeDict = new List<int>();
        public static Dictionary<int,bool> hecktime = new Dictionary<int,bool>();
        public static Dictionary<int, bool>  IsOpenAll = new Dictionary<int, bool>();

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
        /*public void OnCommand(SendingRemoteAdminCommandEventArgs ev)
        {

            //Log.Info(ev.Name);
            if (aliases.IndexOf(ev.Name.ToLower())>-1)
            {
                ev.IsAllowed = false;
                ev.Success = true;
                //Log.Info(ev.Arguments.Count);
                try {
                    if (ev.Arguments.Count < 1)
                    {
                        ev.ReplyMessage = $"Usage command : \"{ev.Name.ToLower()} PlayerId\"";
                        return;
                    }
                    if (ev.Arguments.TryGet(0, out string str))
                    {
                        if (int.TryParse(str, out int PlayerId))
                        {
                            if (PlayerId < 2)
                            {
                                ev.ReplyMessage = $"Usage command : \"{ev.Name.ToLower()} PlayerId\"";
                                return;
                            }
                            Player player = Player.Get(PlayerId);
                            //Log.Info(player.UserId);
                            if (player == null)
                            {
                                ev.ReplyMessage = "Incorrect PlayerId";
                                return;
                            }
                            if(player.IsSCP343())
                            {
                                ev.ReplyMessage = "This player already scp343";
                                return;
                            }
                            player.SetRole(RoleType.ClassD, false, true);
                            Timing.CallDelayed(0.5f, () =>
                            {
                                spawn343(player);
                                tryplugin(player);
                            });
                            ev.ReplyMessage = $"Made {player.Nickname} SCP-343";
                            return;
                        }
                    }
                    ev.ReplyMessage = $"Usage command : \"{ev.Name.ToLower()} PlayerId\""; ;
                    return;
                } catch(Exception ex)
                {
                    ev.ReplyMessage = ex.Message;
                    Log.Info(ex);
                }
                } 
        }//*/

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if(ev.Name=="heck343")
            {
                ev.IsAllowed = false;
                if(ev.Player.IsSCP343())
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
            if(ev.Player.IsSCP343())
            {
                ev.IsAllowed = false;
                ev.Position = ev.Scp106.Position;
            }
        }

        public void OnStarting(StartingEventArgs ev)
        {
            if (ev.IsAuto||ev.Player==null) return;
            if (ev.Player.IsSCP343()&&!plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnStopping(StoppingEventArgs ev)
        {
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
        }

        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player.IsSCP343()&& !plugin.Config.scp343_nuke_interact) ev.IsAllowed = false;
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
            bool isAllowed = IsOpenAll.TryGetValue(ev.Player.Id, out bool opened);
            if (!isAllowed) return;
            if (ev.Player.IsSCP343() && opened) ev.IsAllowed = true;
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
            int count = Player.List.Count();
            if (chance <= plugin.Config.scp343_spawnchance) return;
            if (plugin.Config.minplayers > count&&count!=1) return;
            List<Player> ClassDList = new List<Player>();
            foreach(Player play in Player.List)
            {
                if (play.Role == RoleType.ClassD) ClassDList.Add(play);
            }
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
                if (Exiled.Loader.Loader.Plugins.Any(e=>e.Name== "Subclass"&&e.Author== "Steven4547466"&&e.Config.IsEnabled))
                {
                    Subclass.API.RemoveClass(player);
                }

            }
            catch (Exception ex)
            {
                Log.Debug(ex, false);
            }
        }
        public void spawn343(Player player, bool scp0492 = false)
        {
            if(scp0492)
            {
                API.scp343.Remove(player);
                Active343AndBadgeDict.Remove(player.Id);
                //Log.Info(namebadge[ev.Player.Id]);
                if (colorbadge.TryGetValue(player.Id, out string color)) player.ReferenceHub.serverRoles.NetworkMyColor = color;
                if (namebadge.TryGetValue(player.Id, out string name)) player.ReferenceHub.serverRoles.NetworkMyText = name;
                colorbadge.Remove(player.Id);
                namebadge.Remove(player.Id);
                if(plugin.Config.scp343_canopenanydoor) IsOpenAll.Remove(player.Id);
                hecktime.Remove(player.Id);
            }
            if(player.BadgeHidden) player.BadgeHidden = false;
            API.scp343.Add(player);
            Active343AndBadgeDict.Add(player.Id);
            colorbadge.Add(player.Id, player.ReferenceHub.serverRoles.NetworkMyColor);
            namebadge.Add(player.Id, player.ReferenceHub.serverRoles.NetworkMyText);
            player.ReferenceHub.serverRoles.NetworkMyColor = "red";
            player.ReferenceHub.serverRoles.NetworkMyText = "SCP-343";
            if (plugin.Config.scp343_alert&&!scp0492)
            {
                player.ClearBroadcasts();
                player.Broadcast(15, plugin.Config.scp343_alerttext);
            }
            if (plugin.Config.scp343_console&&!scp0492) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + plugin.Config.scp343_consoletext.Replace("343DOORTIME", plugin.Config.scp343_opendoortime.ToString()).Replace("343HECKTIME", plugin.Config.scp343_hecktime.ToString()) + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000);
                player.EnableEffect(Exiled.API.Enums.EffectType.Scp207, 10000000000, true);
                player.ClearInventory();
                if (!scp0492)
                {
                    foreach (int item in plugin.Config.scp343_itemsatspawn) player.AddItem((ItemType)item);
                }
                hecktime.Add(player.Id, true);
                if (plugin.Config.scp343_canopenanydoor) IsOpenAll.Add(player.Id, false);
                player.Health = 100f;
            });
            if (plugin.Config.scp343_canopenanydoor) Timing.CallDelayed(plugin.Config.scp343_opendoortime, () => {
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
            if (ev.Player.IsSCP343()&&isOpened)
            {
                ev.IsAllowed = true;
            }
        }
       
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            List<ItemType> items = new List<ItemType>();
            foreach(var ite in ev.Player.Inventory.items)
            {
                Log.Debug(ite.id,false);
                items.Add(ite.id);
            }
            if (ev.Player.IsSCP343()&&ev.NewRole!=RoleType.Scp0492)
            {
                API.scp343.Remove(ev.Player);
                Active343AndBadgeDict.Remove(ev.Player.Id);
                //Log.Info(namebadge[ev.Player.Id]);
                if (colorbadge.TryGetValue(ev.Player.Id,out string color)) ev.Player.ReferenceHub.serverRoles.NetworkMyColor=color;
                if(namebadge.TryGetValue(ev.Player.Id, out string name)) ev.Player.ReferenceHub.serverRoles.NetworkMyText = name;
                if (ev.Player.Group.HiddenByDefault) ev.Player.BadgeHidden = true;
                colorbadge.Remove(ev.Player.Id);
                namebadge.Remove(ev.Player.Id);
                IsOpenAll.Remove(ev.Player.Id);
                hecktime.Remove(ev.Player.Id);
            } else if(ev.Player.IsSCP343() && ev.NewRole == RoleType.Scp0492)
            {
                ev.NewRole = RoleType.ClassD;
                Vector3 pos = ev.Player.Position;
                Timing.CallDelayed(0.4f, () => { spawn343(ev.Player,true); });
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

        
        public void OnContaining(ContainingEventArgs ev)
        {
            if (Active343AndBadgeDict.Contains(ev.ButtonPresser.Id)) ev.IsAllowed = false;
        }

        public void OnEnraging(EnragingEventArgs ev)
        {
            if(ev.Player.IsSCP343())
            {
                ev.IsAllowed = false;
            }
        }
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if(Active343AndBadgeDict.Contains(ev.Target.Id))
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

        public void OnEscaping(EscapingEventArgs ev)
        {
            if(ev.Player.IsSCP343())
            {
                ev.IsAllowed = plugin.Config.scp343_canescape;
            }
        }

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player.IsSCP343()&&IsOpenAll.ContainsKey(ev.Player.Id)) ev.IsAllowed = true;
        }
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.IsSCP343()) ev.IsTriggerable = false;
        }
        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if(ev.Player.IsSCP343())
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
