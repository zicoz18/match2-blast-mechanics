using Game.Core.Enums;
using Game.Levels;

namespace Game.Core.LevelBase
{
    public static class LevelDataFactory
    {
        public static LevelData CreateLevelData(LevelName levelName)
        {
            LevelData levelData;
            switch (levelName)
            {
                case LevelName.Level0:
                    levelData = new LevelData_0();
                    break;
                case LevelName.Level1:
                    levelData = new LevelData_1();
                    break;
                case LevelName.Level2:
                    levelData = new LevelData_2();
                    break;
                case LevelName.Level3:
                    levelData = new LevelData_3();
                    break;
                case LevelName.Level4:
                    levelData = new LevelData_4();
                    break;
                case LevelName.Level5:
                    levelData = new LevelData_5();
                    break;
                case LevelName.Level6:
                    levelData = new LevelData_6();
                    break;
                case LevelName.Level7:
                    levelData = new LevelData_7();
                    break;
                case LevelName.Level8:
                    levelData = new LevelData_8();
                    break;
                case LevelName.Level9:
                    levelData = new LevelData_9();
                    break;
                case LevelName.Level10:
                    levelData = new LevelData_10();
                    break;
                default:
                    levelData = new LevelData_0();
                    break;
            }

            levelData.Initialize();

            return levelData;
        }
    }
}