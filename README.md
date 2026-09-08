# Match-2 Blast — Gameplay Mechanics

A 9x9 tap-to-match puzzle game in Unity. Six levels of gameplay mechanics — obstacles,
colour-matched targets, bombs, rockets and special-item combos — implemented on top of an
open-source starter project.

## Provenance

Built on an MIT-licensed match-2 blast starter project (original copyright Peak Games — see
`LICENSE.md`), which provides the board/cell/item architecture, flood-fill match finding,
the fall & fill simulation, the service locator and Level 0. Public copies of the case
materials are available at
[MertBalkan/peak-unithon-matching-game](https://github.com/MertBalkan/peak-unithon-matching-game)
and [BeraDemirhan/Unithon-Peak-2022](https://github.com/BeraDemirhan/Unithon-Peak-2022).

Levels 1-6 are my implementation.

**Files I wrote:** `SpecialItem`, `BalloonItem`, `ColorBalloonItem`, `BombItem`,
`HorizontalRocketItem`, `VerticalRocketItem`, `Combo`, `BombBombCombo`,
`RocketBombCombo`, `RocketRocketCombo`, `ComboFactory`, `ComboManager`,
`MatchNeighbourFinder`, `HintManager`, `ComboType`, `SpecialType`.

**Starter files I modified:** `Board`, `Cell`, `Item`, `ItemFactory`, `CrateItem`,
`CubeItem`, `ImageLibrary`, `ParticleManager`, `ServiceProvider`, `FallAndFillManager`,
`MatchFinder`, `LevelDataFactory`.

Level layouts (`LevelData_*`) and all art assets came with the starter.

## What I built

| Level | Mechanic                                                                            |
| ----- | ----------------------------------------------------------------------------------- |
| 1     | `CrateItem` — fixed in place, two layers, destroyed by adjacent matches             |
| 2     | `BalloonItem` — created from scratch, pops from adjacent matches, 10% fill rate     |
| 3     | `ColorBalloonItem` — pops only from matches of its own colour                       |
| 4     | `BombItem` — 3x3 blast, chain reactions, created from 7+ matches, with player hints |
| 5     | Rocket items — full row/column blasts, created from 5–6 matches                     |
| 6     | Combos — rocket+rocket, bomb+rocket, bomb+bomb, with combo hint particles           |

Levels 5 and 6 have no written brief in the available materials. Their requirements were
reconstructed from the provided sprites, level layouts and enum definitions - including
judgement calls on the blast shape of each combo type.

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

**Hints derived from board state.** `HintManager` recomputes group sizes only when the
board has settled _and_ changed since the last pass, tracked by a change counter on
`Board`. Each item decides how to express its hint (`SetHint`), so the manager needs no
knowledge of item types. The creation thresholds live in one place, so a hint can never
disagree with what a match will actually produce.

## Running it

Unity **6000.5.1f1** (the project was originally authored for 2021.3.5f1 and upgraded).

1. Open the project and load `Assets/Scenes/LevelScene.unity`.
2. Select the **Level** GameObject in the Hierarchy.
3. Pick a level from the **Current Level** dropdown in the Inspector.
4. Press Play. A 9:16 aspect ratio is recommended.

`LevelTest1`–`LevelTest3` are integration levels containing every item type at once.

## Licence

MIT — Copyright (c) 2022 Peak. See `LICENSE.md`.
