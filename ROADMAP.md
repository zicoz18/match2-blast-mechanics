# Roadmap — playable WebGL build

Turning the mechanics sandbox into something a stranger can open in a browser and play.
Features are dependency-ordered; each is roughly one working session.

Current state: six levels of mechanics work, but the level is chosen from the Unity
inspector, there are no goals, no move limit, no win/lose condition and no UI.

---

## F1 — Input that works outside the editor ✅

**Blocking.** `TouchManager` branched on `#if UNITY_EDITOR`, using the mouse in the editor
and `Input.GetTouch(0)` everywhere else — with no `touchCount` guard. In a desktop browser
that throws `IndexOutOfRangeException` every frame and accepts no input at all.

Replaced with runtime detection: touches when present, mouse otherwise. Touch is checked
first because Unity synthesises mouse events from touches by default, which would
otherwise double-fire.

**Acceptance:** WebGL build responds to clicks in a desktop browser and taps on a phone
browser, with a clean console.

---

## F2 — Goals and move limit in level data

`LevelData` exposes only `GridData` and `GetNextFillItemType()`. Add a move limit and a
goal set.

**Decisions:**
- Goal shape — a list of `(ItemType, count)` is the genre standard.
- How a goal identifies its target: `Item` has no `ItemType` field, only `GetMatchType()`.
  Either store `ItemType` on `Item` at creation, or use type checks (`item is CrateItem`).
- Which levels ship. `LevelTest_1/2/3` are debug levels with everything on the board at
  once; a player-facing sequence is probably `Level_0` .. `Level_6_2`.

**Acceptance:** every shipping level declares a move count and at least one goal, readable
without touching the board.

---

## F3 — Runtime tracking of moves and goals

**Moves:** `Board.CellTapped` already separates taps that did something from taps that did
nothing — it calls `MarkChanged()` only on the successful paths. A tap on a lone cube or on
a crate must not cost a move.

**Goals:** `Item.OnDeath()` is virtual and every destruction path funnels through it —
match, blast, combo. That is the natural reporting point.

**Decision:** does the item report its own death upward, or does something observe the
board? Reporting is simpler; observing keeps `Item` ignorant of goals.

**Acceptance:** moves decrement only on real moves; goal counts rise for items destroyed by
cascades and combos, not just direct matches.

---

## F4 — Win and lose resolution

The hard part is *when* to evaluate. Use the primitives that already exist:
`Board.IsMoving()` and `Board.ChangeVersion`, exactly as `HintManager` does — evaluate only
on a settled board.

**The case that catches people:** the last move drops the counter to zero, but the
resulting cascade completes the final goal. That is a win. Never evaluate goals at the
moment a move is spent.

**Acceptance:** goals complete with moves left → win. Moves exhausted with goals incomplete
→ lose. Last move completes goals during the cascade → win. Input blocked after resolution.

---

## F5 — HUD

The project has **no UI at all** — no Canvas, no TextMeshPro, no `UnityEngine.UI`. This is
the largest genuinely new piece.

Needs moves remaining and per-goal icon + count. Goal icons can reuse `ImageLibrary`
sprites, so no new art.

`ScreenManager` already sets `orthographicSize` from `cam.aspect`, so the board adapts to
the viewport; the Canvas has to match that and not overlap the board on wide desktop
aspects.

**Acceptance:** HUD reads correctly at 9:16 portrait, 16:9 desktop, and on window resize.

---

## F6 — Level flow and progression

`Level.CurrentLevel` is a public enum set in the inspector — that has to go.

Needs runtime level selection, advance on win, retry on loss, and persistence of the
highest level reached. `PlayerPrefs` works in WebGL (backed by IndexedDB).

**Decision:** reload the scene per level, or rebuild the board in place? Reloading is
simpler and slower. Rebuilding is faster but every manager must reset — `ServiceProvider`
is a static dictionary and will not clear itself.

**Acceptance:** finish level 1 → land on level 2 → close tab → reopen → still on level 2.

---

## F7 — Screens

Minimum: win popup with **Next**, lose popup with **Retry**, and something on first load so
players are not dropped mid-game with no context. A level-select grid is optional;
auto-advance plus a level counter in the HUD is enough.

**Acceptance:** a first-time visitor can start, finish or fail a level, and continue,
without instructions.

---

## F8 — WebGL build and hosting

- Compression: Gzip (see below), exceptions disabled in release, engine code stripping on.
- Size: `Files/` sits at the repo root, not under `Assets/`, so those GIFs are not in the
  build. Expect roughly 10-20MB compressed.
- Hosting: GitHub Pages from a `gh-pages` branch keeps everything under this repo. itch.io
  is the alternative and handles Unity templates natively.
- **Brotli needs MIME headers GitHub Pages does not send** — use Gzip there.

**Acceptance:** paste the URL into a browser on a machine that has never seen the project
and play three levels.

---

## Notes

**Level design is real work.** These levels were built to demonstrate mechanics, not to be
beaten. Assigning move counts and goals means playtesting each until it is winnable but not
trivial — likely as much time as any single feature above.

**Minimum for a CV link:** F1-F4 plus a minimal F5 is playable. F6-F7 make it feel like a
game. If time is short, one polished level with goals and a move limit beats seven levels
with no flow.
