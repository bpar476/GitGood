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
        Level level;
        switch (levelType) {
            case LevelType.TUTORIAL_1:
                level = new Level("Tutorial");
                level.AppendTeardownHook(() => {
                    Destroy(EngineController.Instance().gameObject);
                });
                return level;
            default:
            case LevelType.MAIN_MENU:
                level = new Level("StartMenu");
                return level;
        }
    }
}
