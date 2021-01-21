using System;
namespace SCP343
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {

        //[Deskription("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;
        //[Deskription("!(Indicates whether the plugin is enabled or not)")]
        public bool scp343_disable { get; set; } = false;


        //[Deskription("What broadcasted who become scp343")]
        public string scp343_alerttext { get; set; } = "You're SCP-343! Check your console for more information about SCP-343.";

        //[Deskription("Will or will not broadcast")]
        public bool scp343_alert { get; set; } = true;
        //[Deskription("What 343 is shown if scp343_broadcast is true.")]
        public string scp343_consoletext { get; set; } = "You are SCP-343, a passive SCP.\n(To be clear this isn't the correct wiki version SCP-343) \nAfter 343DOORTIME seconds you can open any door in the game \nAny weapon/grenade you pick up is morphed into a flashlight.\nYou are NOT counted towards ending the round (Example the round will end if its all NTF and you) \nYou cannot die to anything but lure (106 femur crusher), decontamination, crushed (jumping off at t intersections at heavy) and the nuke.\nYou can use the command .heck343 to spawn as a normal D-Class within the first 343HECKTIME seconds of the round.";
        //[Deskription("What 343 is shown if scp343 will back to usual class d")]
        public string scp343_alertbackd { get; set; } = "You already not scp-343";
        public string scp343_alertheckerrortime { get; set; } = "Time is left...";
        public string scp343_alertheckerrornot343 { get; set; } = "Wait. You are not SCP-343";
        //[Deskription("When 343 spawns should that person be given information about 343")]
        public bool scp343_console { get; set; } = true;
        //[Deskription("Should players be allowed to use the .heck343 client command to respawn themselves as d-class within scp343_hecktime seconds of round start.")]
        public bool scp343_heck { get; set; } = true;
        //[Deskription("How long people should beable to respawn themselves as d-class.")]
        public int scp343_hecktime { get; set; } = 30;
        //[Deskription("Should SPC-343 beable to interact with the nuke.")]
        public bool scp343_nuke_interact { get; set; } = true;
        //[Deskription("How long in seconds till SPC-343 can open any door.")]
        public int scp343_opendoortime { get; set; } = 30;
        //[Deskription("Should SPC-343 convert items?")]
        public bool scp343_itemconverttoggle { get; set; } = true;
        //[Deskription("Percent chance for SPC-343 to spawn at the start of the round.")]
        public float scp343_spawnchance { get; set; } = 30f;
        //[Deskription("What items SCP-343 drops instead of picking up.")]
        public int[] scp343_itemdroplist { get; set; } = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 19, 12, 19, 22, 27, 28, 29, 32 };
        //[Deskription("What items SCP-343 converts.")]
        public int[] scp343_itemstoconvert { get; set; } = new int[] { 10, 13, 14, 16, 20, 21, 23, 24, 25, 26, 30, 35 };
        //[Deskription("What a item should be converted to.")]
        public int[] scp343_converteditems { get; set; } = new int[] { 14 };
        //[Description("Minimum players for ")]
        public int minplayers { get; set; } = 1;
        //[Description("What give scp-343 on spawn")]
        public int[] scp343_itemsatspawn { get; set; } = new int[] { 15 };
        //[Description("Moving Speed lift")]
        public float moving_speed { get; set; } = 8.5f;
    }
}
