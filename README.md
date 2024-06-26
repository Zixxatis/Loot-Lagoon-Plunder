<img src="https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/.github/banner.png" alt="banner">

<p align="center">
   <img src="https://img.shields.io/badge/Engine-Unity%202022.3.21f1-blueviolet?style=&logo=unity" alt="Engine">
   <img src="https://img.shields.io/badge/Platform-Windows%20-darkblue?style=&logo=windows" alt="Platform">
   <img src="https://img.shields.io/badge/Release%20Date-13.04.2024-red" alt="Release Date">
   <img src="https://img.shields.io/badge/Version-1.2.2-blue" alt="Game Version">
   <img src="https://img.shields.io/github/license/Zixxatis/Loot-Lagoon-Plunder" alt="License">
</p>

## Table of Contents
* [About](#About)
* [Features](#features)
	* [Project Systems](#project-systems)
	* [Design Patterns Used](#design-patterns-used)
	* [Custom Editor Scripts](#custom-editor-scripts)
	* [Extensions](#extensions)
- [Screenshots](#Screenshots)
- [Acknowledgments](#Acknowledgments)
- [License](#License)
- [Contact](#Contact)

## About
"Loot Lagoon Plunder" is a pixel rogue-lite 2D platformer where players uncover hidden treasure chests, navigate treacherous levels and upgrade their character's abilities. Keep gems and upgrades after each defeat, ensuring a fresh, rewarding experience as you capture loot and conquer new islands.

Play WebGL version or download .exe for Windows from [itch.io](https://zixxatis.itch.io/loot-lagoon-plunder)
	or download the .exe for Windows directly from [Releases](https://github.com/Zixxatis/Loot-Lagoon-Plunder/releases)

### Built With
- [Newtonsoft Json](https://www.newtonsoft.com/json) - advanced JSON serialization and deserialization.
- [Cinemachine](https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/index.html) - advanced camera for Unity.
- [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html) - a newer, more flexible system for controlling Unity content.
- [Zenject](https://github.com/modesttree/zenject) - a Dependency Injection container for Unity.
- [Dependencies Hunter](https://github.com/AlexeyPerov/Unity-Dependencies-Hunter) - tool for finding asset references in a Unity project.

## Features
### Project Systems
- [Saving System](https://github.com/Zixxatis/Loot-Lagoon-Plunder/tree/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Saving%20System) - A saving system that allows data to be read and stored in a JSON format. It can be used with any class that implements [DataView](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Saving%20System/DataView.cs).
- [Localization System](https://github.com/Zixxatis/Loot-Lagoon-Plunder/tree/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Localization%20System) - A system that allows to localize a TMP text component in Editor and in the runtime.
- Custom struct for counting [Colored Gems](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Economics/ColoredGems.cs), including:
	- Custom indexing & IEnumerator
	- Operator overrides
	- Override of base ToString, Equals, GetHashCode

### Design Patterns Used
- [Strategy](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Scripts/Characters/Character%20Components/Enemy%20Components/EnemyBrain.cs) - Used to change enemy [tasks](https://github.com/Zixxatis/Loot-Lagoon-Plunder/tree/main/Assets/Scripts/Characters/Enemy%20Tasks).
- [State Machine](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Scripts/Characters/State%20Machine/StateMachine.cs) - Used to change behavior and set character animation to match current behavior.
- [Factory](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Scripts/Objects/Collectables/LootFactory.cs) - Used to properly instantiate loot objects.
- Object Pooling - Used for [Shell Projectiles](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Scripts/Characters/Character%20Components/Enemy%20Components/Shell%20Shooter/ShellShooterModule.cs) and [Damage Notification](https://github.com/Zixxatis/Loot-Lagoon-Plunder/tree/main/Assets/Scripts/HUD%20%26%20UI/Damage%20Pop-ups) by hiding and resetting objects instead of instantiating and destroying them.

### Custom Editor Scripts
- [Various Drawers](https://github.com/Zixxatis/Loot-Lagoon-Plunder/tree/main/Assets/Editor/Inspector%20GUI) for custom Components
- [Localization Editor](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Editor/Windows/LocalizationEditor.cs) - An editor window, that helps to maintain the localization file and find any references.
- [Localized Text](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Editor/Inspector%20GUI/TextInputter.cs) Component drawer - Allows to preview localized text in the Editor mode.
- [GameObject Creator](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/Assets/Editor/GameObjects%20Creation/GameObjectCreator.cs) - A simple factory for creating custom objects from the context menu in Edit mode.

### Extensions
- [General Extensions](https://github.com/Zixxatis/Hundred-Cells/tree/main/Assets/Scripts/%5BExtensions%20%26%20Misc%5D/Extensions) - A collection of various extensions for general C# & Unity classes.

## Screenshots
<p align="center">
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_1.png" alt="screenshot1" width="400"/>
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_2.png" alt="screenshot2" width="400"/>
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_3.png" alt="screenshot3" width="400"/>
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_4.png" alt="screenshot4" width="400"/>
</p>

## Acknowledgments
- Asset Packs
	- [Treasure Hunters](https://pixelfrog-assets.itch.io/treasure-hunters)
	- [Pirate Bomb](https://pixelfrog-assets.itch.io/pirate-bomb)
	- [Skill Icons Set](https://quintino-pixels.itch.io/free-pixel-art-skill-icons-pack)
	- [Pixel Art Training Dummy Sprites](https://elthen.itch.io/2d-pixel-art-training-dummy)

## License
This project is open source and available under the [Apache-2.0 License](https://github.com/Zixxatis/Loot-Lagoon-Plunder/blob/main/LICENSE).

## Contact
Created by [@Zixxatis](https://github.com/Zixxatis/) - feel free to contact me!