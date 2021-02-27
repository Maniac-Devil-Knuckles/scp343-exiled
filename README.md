# [SCP-343](http://www.scp-wiki.net/scp-343)
This is a server plugin for [SCP:SL.](https://store.steampowered.com/app/700330/SCP_Secret_Laboratory)

## Install Instructions.
Put SCP343.dll under the release tab into %appdata%\EXILED\Plugins\ on Windows, or into ~/.config/EXILED/Plugins/ on Linux

# Config Options.
| Config Option              | Value Type      | Default Value | Description |
|   :---:                    |     :---:       |    :---:      |    :---:    |
| IsEnabled                  | Boolean         | true         |  Will loading this plugin on the server or not |
| scp343_spawnchance         | Float           | 10            | Percent chance for SPC-343 to spawn at the start of the round. |
| scp343_opendoortime        | Integer         | 60            | How many seconds after roundstart till SCP-343 can open any door in the game (Like door bypass).               |
| scp343_nuke_interact       | Boolean         | false         | Should SCP-343 beable to interact with the nuke?               |
| scp343_itemconverttoggle   | Boolean         | false         | Should SPC-343 convert items?                                  |
| scp343_itemdroplist        | Integer List    | 0,1,2,3,4,5,6,7,8,9,10,11,14,17,19,22,27,28,29 | What items SCP-343 drops instead of picking up.|
| scp343_itemstoconvert      | Integer List    | 13,16,20,21,23,24,25,26,30 | What items SCP-343 converts. |
| scp343_converteditems      | Integer List    | 14            | What a item should be converted to.       |
| scp343_console             | Boolean         | true          | When 343 spawns should that person be given information about 343 in console      |
| scp343_consoletext         | String          | You are SCP-343, a passive SCP.\n(To be clear this isn't the correct wiki version SCP-343) \nAfter 343DOORTIME seconds you can open any door in the game \nAny weapon/grenade you pick up is morphed into a flashlight.\nYou are NOT counted towards ending the round (Example the round will end if its all NTF and you) \nYou cannot die to anything but lure (106 femur crusher), decontamination, crushed (jumping off at t intersections at heavy) and the nuke.\nYou can use the command .heck343 to spawn as a normal D-Class within the first 343HECKTIME seconds of the round.            | What 343 is shown if scp343_console is true.       |
| scp343_heck                | Boolean         | True          | Should players be allowed to use the .heck343 client command to respawn themselves as d-class within scp343_hecktime seconds of round start.     |
| scp343_hecktime            | Integer         | 30            | How long people should beable to respawn themselves as d-class.     |
| scp343_alert               | Boolean         | true          | When 343 spawns should that person will be broadcast    |
| scp343_alerttext           | String          | You're SCP-343! Check your console for more information about SCP-343   | What 343 is shown if scp343_alert is true.       |
| scp343_alertbackd          | String          | You already not scp-343 | What 343 is shown if scp343 will back to usual class d|
| scp343_alertheckerrortime  | String          | Time is left... | What 343 is shown if scp343 will back to usual class d and time is left|
| scp343_alertheckerrornot343| String          | Wait. You are not SCP-343 |  What 343 is shown if not scp343 trying to back to usual class d |
| minplayers                 | Integer         | 1              | Minimum players for spawning scp343 |
| scp343_itemsatspawn        | Integer List    | 15             | What give scp-343 on spawn |
| lift_moving_speed          | float           | 6.5            | Moving Speed lift for all players |

| Command(s)                 | Value Type      | Description                              |
|   :---:                    |     :---:       |    :---:                                 |
| spawn343                   | PlayerID        | Spawn SCP-343 from PlayerID (Number next to name with M menu). Example = "spawn343 2" |

| Permission(s)     | Description                              |
|   :---:           |    :---:                                 |
| scp343.spawn      | This permissions required for spawning as scp343 from command spawn343 |
## Lore Friendly Description 
SCP-343 is a passive immortal D-Class Personnel. He spawns with one Flashlight and any weapon he picks up is morphed to prevent violence. He seeks to help out who he deems worthy. 
## Technical Description  

To be clear this isn't the correct wiki version SCP-343, its just a passive SCP inspired by my experiences of people being Tutorial running around messing with people.

Technically speaking hes a D-Class with godmode enabled or HP with the config option and spawns with the D-Class. After a X minute period set by the server he can open every door in the game. Also to make sure he is passive every weapon he picks up or spawns with is converted to a MedKit or something the server config can change. So people can know who he is, their rank is set to a red "SCP-343" and if they die or are set to a different class their rank name and color are reset to what it was orginally.
SCP-343 doesn't affect who wins.
