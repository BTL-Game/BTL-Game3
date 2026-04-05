# Infinite Flyer

A 2D endless side-scroller built for **Assignment 3 (Game Programming - SEM252)**.
The player controls a baby dragon, avoids obstacles, collects items, and survives as long as possible while the world scrolls infinitely.

## Project Info
This project was developed for `Game Programming - SEM252` as `Assignment 3 - INFINITE FLYER`, using `Unity 6 (6000.3.8f1)`. It is a Flappy Bird-style endless flyer where players progress through three rotating maps, each featuring its own boss encounter, along with collectible-based buff and debuff mechanics that affect gameplay. Source code is available at `https://github.com/BTL-Game/BTL-Game3`, and the executable build will be provided at `<EXE_DOWNLOAD_LINK_PLACEHOLDER>`.

## Controls
- `Space` or `Left Mouse`: flap / jump
- `Esc`: pause/resume
- `C`: toggle ghost form (only in Ghost map)
- `Space` or `Left Mouse` in Story scene: advance story slides

## MVP Coverage
- Infinite horizontal scrolling with parallax background
- Frame-based player animation (falling/idle + flapping state)
- Procedural obstacle and collectible spawning
- Collision-based game over (obstacles/borders) and score increase on collectible pickup
- Off-screen cleanup by destroying spawned objects when they pass the left dead zone

## Implemented Extensions
- Dynamic obstacles (`MutantPillar`) with vertical sinus movement
- Gravity shift collectible effect
- Difficulty scaling over time (global game speed increases gradually)
- Boss phase system
- Map switching via portal (Volcanic / Ice / Ghost map logic)
- Cold/Sanity survival mechanics with warning and fail states

## Technical Note: Parallax Scrolling
Parallax is handled by `ParallaxBackground` components assigned to **far / mid / near** layers.
Each layer has its own `scrollSpeed`, and every frame updates texture UV offset using game time:

`offset += scrollSpeed * (gameSpeed / 10f) * deltaTime`

This means:
- all layers scroll continuously and loop seamlessly through texture offset
- layers move at different relative speeds to create depth
- parallax automatically speeds up with gameplay difficulty because it is tied to `GameManager.gameSpeed`

## How To Run
### Option 1: Run executable (recommended)
1. Extract and run: `<GAME_EXE_NAME_PLACEHOLDER>.exe`

### Option 2: Run from source in Unity
1. Open project in Unity `6000.3.8f1` (or compatible)
2. Open scene: `Assets/Scenes/MainMenuScene.unity`
3. Press Play

## Assets & Copyright Attribution

### Audio
License note: verify and keep all final credit lines based on the original license pages before submission.

- **Background music**
  - Source page: https://pixabay.com/music/adventure-red-flame-over-the-horizon-2-299064
  - Direct file: https://cdn.pixabay.com/audio/2025/12/25/audio_c07afcb282.mp3
  - Title: `Red Flame Over the Horizon 2`
  - Source page background 2: https://pixabay.com/music/happy-childrens-tunes-arcade-games-ghost-hunt-480820/

- **Flap SFX (candidate A)**
  - Direct file: https://cdn.pixabay.com/audio/2025/06/23/audio_af5829174e.mp3



- **Hit / Boss appearance SFX**
  - Direct file: https://cdn.pixabay.com/audio/2022/03/24/audio_72089b8fdb.mp3


- **Attack SFX (variant 1)**
  - Direct file: https://cdn.pixabay.com/audio/2025/06/23/audio_9d407b5127.mp3

- **Death Sound Map Ghost**
  - Death Sound: https://pixabay.com/sound-effects/film-special-effects-pixel-death-66829/




### Images / Sprites / UI / Fonts
- Art assets source list: `GEMINI`

## Team
- Team name: `Group 7`
- Members:
  - `Nguyen Hoai Nam` - `2352776`
  - `Tran Dang Khoa` - `2352590`

## Acknowledgment
Inspired by the infinite-flyer gameplay style popularized by games such as Flappy Bird and Jetpack Joyride.
