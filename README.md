# CISpy

A plugin for SCP:SL that adds a chaos spy into every MTF wave.

Compatible with [SCP-035](https://github.com/Cyanox62/scp035/tree/exiled).

# Installation

**[EXILED](https://github.com/galaxy119/EXILED) must be installed for this to work.**

Place the "CISpy.dll" file in your Plugins folder.

# Configs

| Config        | Value Type |  Default          | Description  |
| :-------------: | :-----:| :-----:|:-----|
| cis_enabled | Boolean |True | Enables CiSpy. |
| cis_spawn_with_grenade | Boolean | False | If spies should be able to spawn with frag grenades. (When false, they spawn with a flashbang.) |
| cis_minimum_size | Integer | 6 | The minimum size of an MTF wave in order for there to be a chance for a CISpy to spawn. (Recommended minimum is  4) |
| cis_spawn_chance | Integer | 100 | The chance for a CISpy to spawn in an MTF spawn wave. (Keep as 100 unless you don't want the spy guaranteed with each wave, |
| cis_guard_chance | Integer | 15 | The chance for a facility guard to spawn as a spy at the start of the round. (Set to zero for no spy as guard) |
| cis_spy_roles | Integer List | 11, 13 | Roles that can spawn as a spy. |
