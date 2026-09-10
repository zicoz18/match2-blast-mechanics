using Game.Core.Enums;
using UnityEngine;

namespace Game.Managers
{
    public class ImageLibrary : MonoBehaviour, IProvidable
    {
        public Sprite GreenCubeSprite;
        public Sprite GreenCubeRocketHintSprite;
        public Sprite GreenCubeBombHintSprite;
        public Sprite YellowCubeSprite;
        public Sprite YellowCubeRocketHintSprite;
        public Sprite YellowCubeBombHintSprite;
        public Sprite BlueCubeSprite;
        public Sprite BlueCubeRocketHintSprite;
        public Sprite BlueCubeBombHintSprite;
        public Sprite RedCubeSprite;
        public Sprite RedCubeRocketHintSprite;
        public Sprite RedCubeBombHintSprite;
        public Sprite CubeSprite;

        public Sprite BalloonSprite;
        public Sprite ColoredBalloonSprite;

        public Sprite GreenBalloonSprite;
        public Sprite YellowBalloonSprite;
        public Sprite BlueBalloonSprite;
        public Sprite RedBalloonSprite;

        public Sprite CrateLayer1Sprite;
        public Sprite CrateLayer2Sprite;

        public Sprite VerticalRocketSprite;
        public Sprite HorizontalRocketSprite;
        public Sprite RocketSprite;
        public Sprite BombSprite;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Sprite GetSpriteForColorBalloonItem(MatchType matchType)
        {
            switch (matchType)
            {
                case MatchType.Green:
                    return GreenBalloonSprite;
                case MatchType.Yellow:
                    return YellowBalloonSprite;
                case MatchType.Blue:
                    return BlueBalloonSprite;
                case MatchType.Red:
                    return RedBalloonSprite;
                default:
                    return null;
            }
        }
        public Sprite GetSpriteForCubeItem(MatchType matchType, SpecialType specialType = SpecialType.None)
        {
            switch (specialType)
            {
                case SpecialType.None:
                    return GetCubeSprite(matchType);
                case SpecialType.Rocket:
                    return GetCubeRocketSprite(matchType);
                case SpecialType.Bomb:
                    return GetCubeBombSprite(matchType);
                default:
                    return null;
            }
        }

        private Sprite GetCubeSprite(MatchType matchType)
        {
            switch (matchType)
            {
                case MatchType.Green:
                    return GreenCubeSprite;
                case MatchType.Yellow:
                    return YellowCubeSprite;
                case MatchType.Blue:
                    return BlueCubeSprite;
                case MatchType.Red:
                    return RedCubeSprite;
                default:
                    return null;
            }
        }

        private Sprite GetCubeRocketSprite(MatchType matchType)
        {
            switch (matchType)
            {
                case MatchType.Green:
                    return GreenCubeRocketHintSprite;
                case MatchType.Yellow:
                    return YellowCubeRocketHintSprite;
                case MatchType.Blue:
                    return BlueCubeRocketHintSprite;
                case MatchType.Red:
                    return RedCubeRocketHintSprite;
                default:
                    return null;
            }
        }

        private Sprite GetCubeBombSprite(MatchType matchType)
        {
            switch (matchType)
            {
                case MatchType.Green:
                    return GreenCubeBombHintSprite;
                case MatchType.Yellow:
                    return YellowCubeBombHintSprite;
                case MatchType.Blue:
                    return BlueCubeBombHintSprite;
                case MatchType.Red:
                    return RedCubeBombHintSprite;
                default:
                    return null;
            }
        }

        public Sprite GetSpriteForGoalType(GoalType goalType)
        {
            switch (goalType)
            {
                case GoalType.None:
                    return null;
                case GoalType.GreenCube:
                    return GreenCubeSprite;
                case GoalType.YellowCube:
                    return YellowCubeSprite;
                case GoalType.BlueCube:
                    return BlueCubeSprite;
                case GoalType.RedCube:
                    return RedCubeSprite;
                case GoalType.Cube:
                    return CubeSprite;
                case GoalType.Balloon:
                    return BalloonSprite;
                case GoalType.ColoredBalloon:
                    return ColoredBalloonSprite;
                case GoalType.GreenBalloon:
                    return GreenBalloonSprite;
                case GoalType.YellowBalloon:
                    return YellowBalloonSprite;
                case GoalType.BlueBalloon:
                    return BlueBalloonSprite;
                case GoalType.RedBalloon:
                    return RedBalloonSprite;
                case GoalType.Crate:
                    return CrateLayer2Sprite;
                case GoalType.Bomb:
                    return BombSprite;
                case GoalType.VerticalRocket:
                    return VerticalRocketSprite;
                case GoalType.HorizontalRocket:
                    return HorizontalRocketSprite;
                case GoalType.Rocket:
                    return RocketSprite;
                default:
                    Debug.LogWarning("Can not get sprite for goal type: " + goalType);
                    return null;
            }
        }
    }
}
