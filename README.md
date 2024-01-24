Implementation of the Hnefatafl board game. 

Features:
* MinMax AI with alpha-betta pruning. Supports multiple difficulties.
* Online mode using Unity Multiplayer and Relay.
* Game saves.

The project uses Assablies to divide up the code into modules. Also uses Unity.TestFramwork for Unit testing.

Modules:
* Core - Base game rules and logic(No unity code)
* Game - Controller module to connect UI visualization with the game rules
* Visuals - UI for the game
* Utilities - Helpers for saving and other services.

