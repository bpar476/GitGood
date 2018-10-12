# Building and Running

# Dependencies

GitGood was built with Unity 2018.1.2 which is available here

[Get Unity - Download Archive - Unity](https://unity3d.com/get-unity/download/archive)

# Running the Test Suite

See the instructions on the Unity Test Runner here. Run the tests in **Play Mode**

[Unity - Manual: Unity Test Runner](https://docs.unity3d.com/Manual/testing-editortestsrunner.html)

# Playing in Unity

- Launch Unity and open the project
- Open the "StartMenu" scene (`Assets/Scenes/UI/StartMenu.unity`)
- Click "Play" Button

# Building

There are some nuances to trying to build GitGood. For reasons we could never determine, the game will not build with the test files. This has something to do with the test assembly causing issues with the production build. In order to build GitGood:

1. Delete the `Assets/Tests` Directory
2. Delete `Assets/Scripts/Main.asmdef`
3. Delete `Assets/3rd-Party/Fungus/Fungus.meta`
4. Click File→Build Settings to get to the build settings window

![BuildSettings](https://user-images.githubusercontent.com/21027663/46838141-73119500-ce14-11e8-8635-51e9201e0826.png)

5. Choose PC, Mac & Linux standalone

6. Choose the appropriate platform

7. Click "Build"
