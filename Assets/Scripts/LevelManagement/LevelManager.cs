using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager> {
    private Level currentLevel;
    private LevelType startingLevel = LevelType.TUTORIAL_1;

    protected override void Awake() {
        base.Awake();
        currentLevel = ResolveLevel(startingLevel);
    }
    public void SwitchLevels(LevelType levelType) {
        currentLevel.GetTeardownHooks()();

        Level level = ResolveLevel(levelType);
        SceneManager.LoadScene(level.GetLocation());

        level.GetStartupHooks()();
        currentLevel = level;
    }

    private Level ResolveLevel(LevelType levelType) {
        switch (levelType) {
            case LevelType.TUTORIAL_1:
                return new Level("Tutorial");
            default:
            case LevelType.MAIN_MENU:
                return new Level("TestLevel");
        }
    }
}
