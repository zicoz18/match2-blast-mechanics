# Match-2 Blast

A 9x9 tap-to-match puzzle game in Unity, playable in the browser: eleven levels, obstacles,
bombs, rockets, special-item combos, per-level objectives and saved progress.

The project has two distinct halves, and the repository is laid out so you can tell them
apart.

|             | What it is                                                           | Where it is defined        |
| ----------- | -------------------------------------------------------------------- | -------------------------- |
| **Phase 1** | A studio's engineering case study, completed as specified            | [`CASE.md`](CASE.md)       |
| **Phase 2** | Turning that mechanics sandbox into a game someone can actually play | [`ROADMAP.md`](ROADMAP.md) |

## Provenance

Built on an MIT-licensed match-2 blast starter project (original copyright Peak Games — see
`LICENSE.md`). The starter provides the board/cell/item architecture, flood-fill match
finding, the fall & fill simulation, the service locator, Level 0, and all art assets.

`CASE.md` is the original assignment as issued, in Turkish. Public copies of the same
materials are available at
[MertBalkan/peak-unithon-matching-game](https://github.com/MertBalkan/peak-unithon-matching-game)
and [BeraDemirhan/Unithon-Peak-2022](https://github.com/BeraDemirhan/Unithon-Peak-2022).

**Written by me:** everything under `Assets/Scripts/UI/` and `Assets/Scripts/Game/Combos/`,
plus `SpecialItem`, `BalloonItem`, `ColorBalloonItem`, `BombItem`, `HorizontalRocketItem`,
`VerticalRocketItem`, `Combo`, `ComboManager`, `MatchNeighbourFinder`, `HintManager`,
`LevelProgress`, `LevelFlow`, `LevelProgressManager`, `Goal`, `SceneNames`, the
`ComboType` / `SpecialType` / `GoalType` enums, and `LevelData_5` through `LevelData_10`.

**Starter files I modified:** `Board`, `Cell`, `Item`, `ItemFactory`, `CrateItem`,
`CubeItem`, `Level`, `LevelData`, `LevelDataFactory`, `ImageLibrary`, `ParticleManager`,
`ServiceProvider`, `FallAndFillManager`, `MatchFinder`, `ScreenManager`, `TouchManager`.

## Phase 1 — the case study

`CASE.md` defines the starting point and sets four tasks. A fifth and sixth arrived as a
surprise round partway through the original event.

| Level | Mechanic                                                                            |
| ----- | ----------------------------------------------------------------------------------- |
| 1     | `CrateItem` — fixed in place, two layers, destroyed by adjacent matches             |
| 2     | `BalloonItem` — built from scratch, pops from adjacent matches, 10% fill rate       |
| 3     | `ColorBalloonItem` — pops only from matches of its own colour                       |
| 4     | `BombItem` — 3x3 blast, chain reactions, created from 7+ matches, with player hints |
| 5     | Rocket items — full row/column blasts, created from 5–6 matches                     |
| 6     | Combos — rocket+rocket, bomb+rocket, bomb+bomb, with combo hint particles           |

Levels 5 and 6 have no written brief in the available materials. Their requirements were
reconstructed from the provided sprites, level layouts and enum definitions — including
judgement calls on the blast shape of each combo type.

## Phase 2 — making it a game

The case produces a mechanics sandbox: one scene, one level, chosen from a dropdown in the
editor, with no way to win or lose. `ROADMAP.md` is the set of assignments I wrote for
myself to close that gap, worked through in order.

|     | Feature                                                                                                         |
| --- | --------------------------------------------------------------------------------------------------------------- |
| F1  | Input that works outside the editor — the build used a compile-time branch that made it unplayable in a browser |
| F2  | Per-level move limits and goals, declared as data                                                               |
| F3  | Tracking moves and goal progress at runtime                                                                     |
| F4  | Win and lose resolution, evaluated only on a settled board                                                      |
| F5  | A heads-up display: moves, goal icons and counts                                                                |
| F6  | A main menu, level progression and saved progress                                                               |
| F7  | Result panels with continue and retry                                                                           |
| F8  | A WebGL build and public hosting                                                                                |

## Design notes

**A single vocabulary for item events.** Rather than one overloaded `Execute` method, items
respond to four distinct events — `TryMatchExecute` (I was matched), `TryNeighbourExecute`
(a match happened beside me), `TryBlastExecute` (I was caught in an explosion) and
`Activate` (detonate). Adding a new item type means overriding the events it cares about
and nothing else.

**Generic damage model.** `Damage()` / `OnDeath()` / `OnNonLethalDamage()` on the base
`Item` let multi-layer items (crates) and single-hit items (balloons) share one code path,
and let bombs and rockets damage anything without knowing what it is.

**`SpecialItem` as a template method.** Bombs and both rockets share one activation
algorithm with a single variation point (`GetBlastArea()`). Chain reactions terminate
because each special removes itself before propagating — the board itself acts as the
visited set.

**Hints derived from board state.** `HintManager` measures every group on the board and
lets each item decide how to express its own hint (`SetHint`), so the manager needs no
knowledge of item types. The creation thresholds live in one place, so a hint can never
disagree with what a match will actually produce. The pass runs every frame: gating it on a
settled board was cheaper but left hints visibly wrong for the length of a cascade. To
afford that, the pass allocates nothing — `MatchFinder` fills a caller-supplied list, and
the already-counted set is a reusable 9x9 array of flags.

**One definition of a settled board.** Falling items are the thing that makes the board
lie: mid-cascade, the cell under your finger is not the cell the item will land in, and a
goal counter is not yet final. `Board.IsMoving()` answers that question once, and both the
tap handler and the win/lose check consult it — taps are ignored while anything is in
motion, and progression is evaluated only on a still board that has changed since the last
check.

**Progress as a static over `PlayerPrefs`.** `LevelProgress` deliberately avoids a
`DontDestroyOnLoad` singleton: the value is one integer already persisted to disk, so an
in-memory copy would only be a second source of truth able to go stale.

## Running it

Unity **6000.5.1f1** (originally authored for 2021.3.5f1 and upgraded).

Open the project and press Play from `Assets/Scenes/MainScene.unity`. The play button reads
your saved progress; completing a level advances it, and it survives closing the editor.

To jump straight to a specific level while developing, open `Assets/Scenes/LevelScene.unity`,
select the **Level** GameObject and tick **Use Override Level**. That field is editor-only
and is compiled out of builds.

## Licence

MIT — Copyright (c) 2022 Peak. See `LICENSE.md`.
