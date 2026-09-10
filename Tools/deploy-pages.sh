#!/usr/bin/env bash
#
# Builds the game and publishes it to the gh-pages branch, which GitHub Pages serves at
#   https://zicoz18.github.io/match2-blast-mechanics/
#
# Pages cannot run Unity, so the built files themselves are what gets committed. They are
# generated output that nobody edits, so each deploy replaces the branch with a single
# commit rather than adding to its history — otherwise the repo would grow by the size of
# a build every time.
#
# Usage: Tools/deploy-pages.sh
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$REPO_ROOT"

UNITY="${UNITY:-/Applications/Unity/Hub/Editor/6000.5.1f1/Unity.app/Contents/MacOS/Unity}"
BUILD_DIR="Build/WebGL"
BRANCH="gh-pages"

# A batchmode build against a project the editor has open fails somewhere in the middle
# rather than refusing up front, so refuse up front.
if [ -f Temp/UnityLockfile ]; then
	echo "Unity has the project open. Close it and run this again." >&2
	exit 1
fi

if [ ! -x "$UNITY" ]; then
	echo "Unity not found at $UNITY (override with UNITY=/path/to/Unity)" >&2
	exit 1
fi

echo "==> Building $BUILD_DIR"
mkdir -p Logs
"$UNITY" -quit -batchmode -nographics -projectPath "$REPO_ROOT" \
	-executeMethod GameEditor.WebGLBuilder.Build -logFile "$REPO_ROOT/Logs/deploy-build.log"

if [ ! -f "$BUILD_DIR/index.html" ]; then
	echo "Build produced no $BUILD_DIR/index.html — see Logs/deploy-build.log" >&2
	exit 1
fi

SOURCE_COMMIT="$(git rev-parse --short HEAD)"
REMOTE_URL="$(git remote get-url origin)"

# Assemble the branch in a throwaway repo, so nothing here touches the working tree.
STAGING="$(mktemp -d)"
trap 'rm -rf "$STAGING"' EXIT

cp -R "$BUILD_DIR"/. "$STAGING"/
# Stops GitHub from running the site through Jekyll, which skips paths beginning with _.
touch "$STAGING/.nojekyll"

echo "==> Publishing to $BRANCH"
git -C "$STAGING" init -q -b "$BRANCH"
git -C "$STAGING" add -A
git -C "$STAGING" commit -q -m "Deploy $SOURCE_COMMIT"
git -C "$STAGING" remote add origin "$REMOTE_URL"
git -C "$STAGING" push -q --force origin "$BRANCH"

echo "==> Published build of $SOURCE_COMMIT"
echo "    https://zicoz18.github.io/match2-blast-mechanics/"
echo "    Pages can take a minute to pick up a new deploy."
