using System;
using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;
using SCP008X;
using MEC;
namespace SCP343.Patches
{
    [HarmonyPatch(typeof(Scp008),nameof(Scp008.Awake))]
    public class scp008patch
    {
        public void Prefix(Scp008 scp008)
        {
            Player ply = Player.Get(scp008.gameObject);
            try
            {
                Timing.CallDelayed(5f, () =>
                {
                    Scp008 scp = ply.GameObject.GetComponent<Scp008>();
                    if (scp!=null) return;
                    if (ply.IsSCP343())
                    {
                        UnityEngine.Object.Destroy(scp008);
                        try
                        {
                            scp = ply.GameObject.GetComponent<Scp008>();
                          if(scp!=null) UnityEngine.Object.Destroy(scp);
                        }
                        catch
                        {
                        }
                    }
                });
            }
            catch
            {
            }
            Timing.CallDelayed(2f,()=>ply.Health=100);
        }
    }
}
