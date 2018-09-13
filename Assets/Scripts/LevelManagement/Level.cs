using System;

public class Level {
    protected Action startupHooks;
    protected Action teardownHooks;

    protected string sceneLocation;

    protected LevelType levelType;

    public Level() {
        startupHooks = this.StartupHookBase;
        teardownHooks = this.TeardownHookBase;
    }

    public Level(string sceneLocation) : this() {
        this.levelType = levelType;
        this.sceneLocation = sceneLocation;
    }

    protected virtual void StartupHookBase() {
    }

    protected virtual void TeardownHookBase() {
    }

    public string GetLocation() {
        return sceneLocation;
    }

    public LevelType GetLevelType() {
        return levelType;
    }

    public Action GetStartupHooks() {
        return startupHooks;
    }

    public Action GetTeardownHooks() {
        return teardownHooks;
    }

    public void PrependStartupHook(Action hook) {
        startupHooks = hook + startupHooks;
    }

    public void AppendStartupHook(Action hook) {
        startupHooks = startupHooks + hook;
    }

    public void PrependTeardownHook(Action hook) {
        teardownHooks = hook + teardownHooks;
    }

    public void AppendTeardownHook(Action hook) {
        teardownHooks = teardownHooks + hook;
    }
}
