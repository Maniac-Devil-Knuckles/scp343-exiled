using System;
namespace SCP343
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {

        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("Will log some error debug or debug some test features")]
        public bool Debug { get; set; } = false;

        [Description("scp343 can escape?")]
        public bool scp343_canescape { get; set; } = false;

        [Description("scp343 can open doors?")]
        public bool scp343_canopenanydoor { get; set; } = true;
        //[Description("Can scp343 stop scp173?")]
        //public bool scp343_canstopscp173 { get; set; } = false;

        [Description("What broadcasted who become scp343")]
        public string scp343_alerttext { get; set; } = "You're SCP-343! Check your console for more information about SCP-343.";

        [Description("Will or will not broadcast")]
        public bool scp343_alert { get; set; } = true;
        [Description("What 343 is shown if scp343_broadcast is true.")]
        public string scp343_consoletext { get; set; } = "You are SCP-343, a passive SCP.\n(To be clear this isn't the correct wiki version SCP-343) \nAfter 343DOORTIME seconds you can open any door in the game \nAny weapon/grenade you pick up is morphed into a flashlight.\nYou are NOT counted towards ending the round (Example the round will end if its all NTF and you) \nYou cannot die to anything but lure (106 femur crusher), decontamination, crushed (jumping off at t intersections at heavy) and the nuke.\nYou can use the command .heck343 to spawn as a normal D-Class within the first 343HECKTIME seconds of the round.";
        [Description("What 343 is shown if scp343 will back to usual class d")]
        public string scp343_alertbackd { get; set; } = "You already not scp-343";
        public string scp343_alertheckerrortime { get; set; } = "Time is left...";
        public string scp343_alertheckerrornot343 { get; set; } = "Wait. You are not SCP-343";
        [Description("When 343 spawns should that person be given information about 343")]
        public bool scp343_console { get; set; } = true;
        [Description("Should players be allowed to use the .heck343 client command to respawn themselves as d-class within scp343_hecktime seconds of round start.")]
        public bool scp343_heck { get; set; } = true;
        [Description("How long people should beable to respawn themselves as d-class.")]
        public int scp343_hecktime { get; set; } = 30;
        [Description("If scp343_heck is false, what should send in console")]
        public string scp343_heckerrordisable { get; set; } = ".heck343 is disabled by config";
        [Description("Should SPC-343 beable to interact with the nuke.")]
        public bool scp343_nuke_interact { get; set; } = true;
        [Description("How long in seconds till SPC-343 can open any door.")]
        public int scp343_opendoortime { get; set; } = 30;
        [Description("Should SPC-343 convert items?")]
        public bool scp343_itemconverttoggle { get; set; } = true;
        [Description("Percent chance for SPC-343 to spawn at the start of the round.")]
        public float scp343_spawnchance { get; set; } = 30f;
        [Description("What items SCP-343 drops instead of picking up.")]
        public int[] scp343_itemdroplist { get; set; } = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 19, 12, 19, 22, 27, 28, 29, 32 };
        [Description("What items SCP-343 converts.")]
        public int[] scp343_itemstoconvert { get; set; } = new int[] { 10, 13, 14, 16, 20, 21, 23, 24, 25, 26, 30, 35 };
        [Description("What a item should be converted to.")]
        public int[] scp343_converteditems { get; set; } = new int[] { 14 };
        [Description("Minimum players for ")]
        public int minplayers { get; set; } = 1;
        [Description("What give scp-343 on spawn")]
        public int[] scp343_itemsatspawn { get; set; } = new int[] { 15 };
        [Description("Moving Speed lift for all players")]
        public float lift_moving_speed { get; set; } = 6.5f;
    }
}
