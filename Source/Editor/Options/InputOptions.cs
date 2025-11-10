// Copyright (c) Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEngine;

#pragma warning disable 1591

namespace FlaxEditor.Options
{
    /// <summary>
    /// Action to perform when a Scene Node receive a double mouse left click.
    /// </summary>
    public enum SceneNodeDoubleClick
    {
        /// <summary>
        /// Toggles expand/state of the node.
        /// </summary>
        Expand,

        /// <summary>
        /// Rename the node.
        /// </summary>
        RenameActor,

        /// <summary>
        /// Focus the object in the viewport.
        /// </summary>
        FocusActor,

        /// <summary>
        /// If possible, open the scene node in an associated Editor (eg. Prefab Editor).
        /// </summary>
        OpenPrefab,
    }

    /// <summary>
    /// Shortcut availability in play mode.
    /// </summary>
    public enum PlayModeShortcutAvailability
    {
        /// <summary>
        /// None of the window shortcuts will be available in play mode.
        /// </summary>
        None,
        /// <summary>
        /// Only the profiler window shortcut will be available in play mode.
        /// </summary>
        ProfilerOnly,
        /// <summary>
        /// All window shortcuts will be available in play mode.
        /// </summary>
        All,
    }

    /// <summary>
    /// Input editor options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<InputOptions>))]
    [HideInEditor]
    public sealed class InputOptions
    {
        /// <summary>
        /// Gets a value based on the current settings that indicates wether window shortcuts will be avaliable during play mode.
        /// </summary>
        public static bool WindowShortcutsAvaliable => !Editor.IsPlayMode || Editor.Instance.Options.Options.Input.PlayModeWindowShortcutAvaliability == PlayModeShortcutAvailability.All;

        /// <summary>
        /// Gets a value based on the current settings that indicates wether the profiler window shortcut will be avaliable during play mode.
        /// </summary>
        public static bool ProfilerShortcutAvaliable => WindowShortcutsAvaliable || Editor.Instance.Options.Options.Input.PlayModeWindowShortcutAvaliability == PlayModeShortcutAvailability.ProfilerOnly;

        #region Common

        [DefaultValue(typeof(InputBinding), "Ctrl+S")]
        [EditorDisplay("Common"), EditorOrder(100)]
        public InputBinding Save = new(KeyboardKeys.S, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "F2")]
        [EditorDisplay("Common"), EditorOrder(110)]
        public InputBinding Rename = new(KeyboardKeys.F2);

        [DefaultValue(typeof(InputBinding), "Ctrl+C")]
        [EditorDisplay("Common"), EditorOrder(120)]
        public InputBinding Copy = new(KeyboardKeys.C, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+X")]
        [EditorDisplay("Common"), EditorOrder(130)]
        public InputBinding Cut = new(KeyboardKeys.X, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+V")]
        [EditorDisplay("Common"), EditorOrder(140)]
        public InputBinding Paste = new(KeyboardKeys.V, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+D")]
        [EditorDisplay("Common"), EditorOrder(150)]
        public InputBinding Duplicate = new(KeyboardKeys.D, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Delete")]
        [EditorDisplay("Common"), EditorOrder(160)]
        public InputBinding Delete = new(KeyboardKeys.Delete);

        [DefaultValue(typeof(InputBinding), "Ctrl+Z")]
        [EditorDisplay("Common"), EditorOrder(170)]
        public InputBinding Undo = new(KeyboardKeys.Z, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Y")]
        [EditorDisplay("Common"), EditorOrder(180)]
        public InputBinding Redo = new(KeyboardKeys.Y, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+A")]
        [EditorDisplay("Common"), EditorOrder(190)]
        public InputBinding SelectAll = new(KeyboardKeys.A, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Shift+A")]
        [EditorDisplay("Common"), EditorOrder(195)]
        public InputBinding DeselectAll = new(KeyboardKeys.A, KeyboardKeys.Shift, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "F")]
        [EditorDisplay("Common"), EditorOrder(200)]
        public InputBinding FocusSelection = new(KeyboardKeys.F);

        [DefaultValue(typeof(InputBinding), "Shift+F")]
        [EditorDisplay("Common"), EditorOrder(200)]
        public InputBinding LockFocusSelection = new(KeyboardKeys.F, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Ctrl+F")]
        [EditorDisplay("Common"), EditorOrder(210)]
        public InputBinding Search = new(KeyboardKeys.F, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+O")]
        [EditorDisplay("Common"), EditorOrder(220)]
        public InputBinding ContentFinder = new(KeyboardKeys.O, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "R")]
        [EditorDisplay("Common"), EditorOrder(230)]
        public InputBinding RotateSelection = new(KeyboardKeys.R);

        [DefaultValue(typeof(InputBinding), "F11")]
        [EditorDisplay("Common"), EditorOrder(240)]
        public InputBinding ToggleFullscreen = new(KeyboardKeys.F11);

        [DefaultValue(typeof(InputBinding), "Ctrl+BackQuote")]
        [EditorDisplay("Common"), EditorOrder(250)]
        public InputBinding FocusConsoleCommand = new(KeyboardKeys.BackQuote, KeyboardKeys.Control);

        #endregion

        #region File

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("File"), EditorOrder(300)]
        public InputBinding SaveScenes = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("File"), EditorOrder(310)]
        public InputBinding CloseScenes = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("File"), EditorOrder(320)]
        public InputBinding OpenScriptsProject = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("File"), EditorOrder(330)]
        public InputBinding GenerateScriptsProject = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("File"), EditorOrder(340)]
        public InputBinding RecompileScripts = new(KeyboardKeys.None);

        #endregion

        #region Scene

        [DefaultValue(typeof(InputBinding), "End")]
        [EditorDisplay("Scene", "Snap To Ground"), EditorOrder(500)]
        public InputBinding SnapToGround = new(KeyboardKeys.End);

        [DefaultValue(typeof(InputBinding), "End")]
        [EditorDisplay("Scene", "Vertex Snapping"), EditorOrder(550)]
        public InputBinding SnapToVertex = new(KeyboardKeys.V);

        [DefaultValue(typeof(InputBinding), "F5")]
        [EditorDisplay("Scene", "Play/Stop"), EditorOrder(510)]
        public InputBinding Play = new(KeyboardKeys.F5);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene", "Play Current Scenes/Stop"), EditorOrder(520)]
        public InputBinding PlayCurrentScenes = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "F6")]
        [EditorDisplay("Scene"), EditorOrder(530)]
        public InputBinding Pause = new(KeyboardKeys.F6);

        [DefaultValue(typeof(InputBinding), "F11")]
        [EditorDisplay("Scene"), EditorOrder(540)]
        public InputBinding StepFrame = new(KeyboardKeys.F11);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene", "Cook & Run"), EditorOrder(550)]
        public InputBinding CookAndRun = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene", "Run cooked game"), EditorOrder(560)]
        public InputBinding RunCookedGame = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene", "Move actor to viewport"), EditorOrder(570)]
        public InputBinding MoveActorToViewport = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene", "Align actor with viewport"), EditorOrder(571)]
        public InputBinding AlignActorWithViewport = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene", "Align viewport with actor"), EditorOrder(572)]
        public InputBinding AlignViewportWithActor = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Scene"), EditorOrder(573)]
        public InputBinding PilotActor = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Ctrl+G")]
        [EditorDisplay("Scene"), EditorOrder(574)]
        public InputBinding GroupSelectedActors = new(KeyboardKeys.G, KeyboardKeys.Control);

        #endregion

        #region Tools

        [DefaultValue(typeof(InputBinding), "Ctrl+F10")]
        [EditorDisplay("Tools", "Build scenes data"), EditorOrder(600)]
        public InputBinding BuildScenesData = new(KeyboardKeys.F10, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Tools", "Bake lightmaps"), EditorOrder(601)]
        public InputBinding BakeLightmaps = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Tools", "Clear lightmaps data"), EditorOrder(602)]
        public InputBinding ClearLightmaps = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Tools", "Bake all env probes"), EditorOrder(603)]
        public InputBinding BakeEnvProbes = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Tools", "Build CSG mesh"), EditorOrder(604)]
        public InputBinding BuildCSG = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Tools", "Build Nav Mesh"), EditorOrder(605)]
        public InputBinding BuildNav = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Tools", "Build all meshes SDF"), EditorOrder(606)]
        public InputBinding BuildSDF = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "F12")]
        [EditorDisplay("Tools", "Take screenshot"), EditorOrder(607)]
        public InputBinding TakeScreenshot = new(KeyboardKeys.F12);

        #endregion

        #region Profiler

        [DefaultValue(typeof(InputBinding), "Ctrl+Alpha7")]
        [EditorDisplay("Profiler", "Open Profiler Window"), EditorOrder(630)]
        public InputBinding ProfilerWindow = new(KeyboardKeys.Alpha7, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Profiler", "Start/Stop Profiler"), EditorOrder(631)]
        public InputBinding ProfilerStartStop = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Profiler", "Clear Profiler data"), EditorOrder(632)]
        public InputBinding ProfilerClear = new(KeyboardKeys.None);

        #endregion

        #region Debugger

        [DefaultValue(typeof(InputBinding), "F5")]
        [EditorDisplay("Debugger", "Continue"), EditorOrder(810)]
        public InputBinding DebuggerContinue = new(KeyboardKeys.F5);

        [DefaultValue(typeof(InputBinding), "Shift+F11")]
        [EditorDisplay("Debugger", "Unlock mouse in Play Mode"), EditorOrder(820)]
        public InputBinding DebuggerUnlockMouse = new(KeyboardKeys.F11, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "F10")]
        [EditorDisplay("Debugger", "Step Over"), EditorOrder(830)]
        public InputBinding DebuggerStepOver = new(KeyboardKeys.F10);

        [DefaultValue(typeof(InputBinding), "F11")]
        [EditorDisplay("Debugger", "Step Into"), EditorOrder(840)]
        public InputBinding DebuggerStepInto = new(KeyboardKeys.F11);

        [DefaultValue(typeof(InputBinding), "Shift+F11")]
        [EditorDisplay("Debugger", "Step Out"), EditorOrder(850)]
        public InputBinding DebuggerStepOut = new(KeyboardKeys.F11, KeyboardKeys.Shift);

        #endregion

        #region Gizmo

        [DefaultValue(typeof(InputBinding), "Alpha1")]
        [EditorDisplay("Gizmo"), EditorOrder(1000)]
        public InputBinding TranslateMode = new(KeyboardKeys.Alpha1);

        [DefaultValue(typeof(InputBinding), "Alpha2")]
        [EditorDisplay("Gizmo"), EditorOrder(1010)]
        public InputBinding RotateMode = new(KeyboardKeys.Alpha2);

        [DefaultValue(typeof(InputBinding), "Alpha3")]
        [EditorDisplay("Gizmo"), EditorOrder(1020)]
        public InputBinding ScaleMode = new(KeyboardKeys.Alpha3);

        [DefaultValue(typeof(InputBinding), "Alpha4")]
        [EditorDisplay("Gizmo"), EditorOrder(1030)]
        public InputBinding ToggleTransformSpace = new(KeyboardKeys.Alpha4);

        #endregion

        #region Viewport

        [DefaultValue(typeof(InputBinding), "W")]
        [EditorDisplay("Viewport"), EditorOrder(1500)]
        public InputBinding Forward = new(KeyboardKeys.W);

        [DefaultValue(typeof(InputBinding), "S")]
        [EditorDisplay("Viewport"), EditorOrder(1510)]
        public InputBinding Backward = new(KeyboardKeys.S);

        [DefaultValue(typeof(InputBinding), "A")]
        [EditorDisplay("Viewport"), EditorOrder(1520)]
        public InputBinding Left = new(KeyboardKeys.A);

        [DefaultValue(typeof(InputBinding), "D")]
        [EditorDisplay("Viewport"), EditorOrder(1530)]
        public InputBinding Right = new(KeyboardKeys.D);

        [DefaultValue(typeof(InputBinding), "E")]
        [EditorDisplay("Viewport"), EditorOrder(1540)]
        public InputBinding Up = new(KeyboardKeys.E);

        [DefaultValue(typeof(InputBinding), "Q")]
        [EditorDisplay("Viewport"), EditorOrder(1550)]
        public InputBinding Down = new(KeyboardKeys.Q);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Viewport", "Toggle Camera Rotation"), EditorOrder(1560)]
        public InputBinding CameraToggleRotation = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Viewport", "Increase Camera Move Speed"), EditorOrder(1570)]
        public InputBinding CameraIncreaseMoveSpeed = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Viewport", "Decrease Camera Move Speed"), EditorOrder(1571)]
        public InputBinding CameraDecreaseMoveSpeed = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Numpad0")]
        [EditorDisplay("Viewport"), EditorOrder(1700)]
        public InputBinding ViewpointFront = new(KeyboardKeys.Numpad0);

        [DefaultValue(typeof(InputBinding), "Numpad5")]
        [EditorDisplay("Viewport"), EditorOrder(1710)]
        public InputBinding ViewpointBack = new(KeyboardKeys.Numpad5);

        [DefaultValue(typeof(InputBinding), "Numpad4")]
        [EditorDisplay("Viewport"), EditorOrder(1720)]
        public InputBinding ViewpointLeft = new(KeyboardKeys.Numpad4);

        [DefaultValue(typeof(InputBinding), "Numpad6")]
        [EditorDisplay("Viewport"), EditorOrder(1730)]
        public InputBinding ViewpointRight = new(KeyboardKeys.Numpad6);

        [DefaultValue(typeof(InputBinding), "Numpad8")]
        [EditorDisplay("Viewport"), EditorOrder(1740)]
        public InputBinding ViewpointTop = new(KeyboardKeys.Numpad8);

        [DefaultValue(typeof(InputBinding), "Numpad2")]
        [EditorDisplay("Viewport"), EditorOrder(1750)]
        public InputBinding ViewpointBottom = new(KeyboardKeys.Numpad2);

        [DefaultValue(typeof(InputBinding), "NumpadDecimal")]
        [EditorDisplay("Viewport"), EditorOrder(1760)]
        public InputBinding ToggleOrthographic = new(KeyboardKeys.NumpadDecimal);

        #endregion

        #region Debug Views

        [DefaultValue(typeof(InputBinding), "Alt+Alpha4")]
        [EditorDisplay("Debug Views"), EditorOrder(2000)]
        public InputBinding Default = new(KeyboardKeys.Alpha4, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "Alt+Alpha3")]
        [EditorDisplay("Debug Views"), EditorOrder(2010)]
        public InputBinding Unlit = new(KeyboardKeys.Alpha3, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2020)]
        public InputBinding NoPostFX = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Alt+Alpha2")]
        [EditorDisplay("Debug Views"), EditorOrder(2030)]
        public InputBinding Wireframe = new(KeyboardKeys.Alpha2, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "Alt+Alpha5")]
        [EditorDisplay("Debug Views"), EditorOrder(2040)]
        public InputBinding LightBuffer = new(KeyboardKeys.Alpha5, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2050)]
        public InputBinding ReflectionsBuffer = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2060)]
        public InputBinding DepthBuffer = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2070)]
        public InputBinding MotionVectors = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2080)]
        public InputBinding LightmapUVDensity = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2090)]
        public InputBinding VertexColors = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Alt+Alpha1")]
        [EditorDisplay("Debug Views"), EditorOrder(2100)]
        public InputBinding PhysicsColliders = new(KeyboardKeys.Alpha1, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2110)]
        public InputBinding LODPreview = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2120)]
        public InputBinding MaterialComplexity = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2130)]
        public InputBinding QuadOverdraw = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2140)]
        public InputBinding GloablSDF = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2150)]
        public InputBinding GlobalSurfaceAtlas = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Debug Views"), EditorOrder(2160)]
        public InputBinding GlobalIllumination = new(KeyboardKeys.None);

        #endregion

        #region View Flags

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3000)]
        public InputBinding AntiAliasing = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3010)]
        public InputBinding Shadows = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha7")]
        [EditorDisplay("View Flags"), EditorOrder(3020)]
        public InputBinding EditorSprites = new(KeyboardKeys.Alpha7, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3030)]
        public InputBinding Reflections = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3040)]
        public InputBinding ScreenSpaceReflections = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3050)]
        public InputBinding AmbientOcclusion = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha6")]
        [EditorDisplay("View Flags", "Global Illumination"), EditorOrder(3060)]
        public InputBinding GlobalIlluminationViewFlag = new(KeyboardKeys.Alpha6, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3070)]
        public InputBinding DirectionalLights = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3080)]
        public InputBinding PointLights = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3090)]
        public InputBinding SpotLights = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3100)]
        public InputBinding SkyLights = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3110)]
        public InputBinding Sky = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3120)]
        public InputBinding Fog = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3130)]
        public InputBinding SpecularLight = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3140)]
        public InputBinding Decals = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha3")]
        [EditorDisplay("View Flags"), EditorOrder(3150)]
        public InputBinding CustomPostProcess = new(KeyboardKeys.Alpha3, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3160)]
        public InputBinding Bloom = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3170)]
        public InputBinding ToneMapping = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha2")]
        [EditorDisplay("View Flags"), EditorOrder(3180)]
        public InputBinding EyeAdaptation = new(KeyboardKeys.Alpha2, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3190)]
        public InputBinding CameraArtifacts = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3200)]
        public InputBinding LensFlares = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3210)]
        public InputBinding DepthOfField = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3220)]
        public InputBinding MotionBlur = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("View Flags"), EditorOrder(3230)]
        public InputBinding ContactShadows = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha1")]
        [EditorDisplay("View Flags"), EditorOrder(3240)]
        public InputBinding PhysicsDebug = new(KeyboardKeys.Alpha1, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha5")]
        [EditorDisplay("View Flags"), EditorOrder(3250)]
        public InputBinding LightsDebug = new(KeyboardKeys.Alpha5, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Alpha4")]
        [EditorDisplay("View Flags"), EditorOrder(3260)]
        public InputBinding DebugDraw = new(KeyboardKeys.Alpha4, KeyboardKeys.Control, KeyboardKeys.Shift);

        #endregion

        #region Interface

        [DefaultValue(typeof(InputBinding), "Ctrl+W")]
        [EditorDisplay("Interface"), EditorOrder(3500)]
        public InputBinding CloseTab = new(KeyboardKeys.W, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Tab")]
        [EditorDisplay("Interface"), EditorOrder(3510)]
        public InputBinding NextTab = new(KeyboardKeys.Tab, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Shift+Ctrl+Tab")]
        [EditorDisplay("Interface"), EditorOrder(3520)]
        public InputBinding PreviousTab = new(KeyboardKeys.Tab, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(SceneNodeDoubleClick.Expand)]
        [EditorDisplay("Interface"), EditorOrder(3530)]
        public SceneNodeDoubleClick DoubleClickSceneNode = SceneNodeDoubleClick.Expand;

        #endregion

        #region Windows

        /// <summary>
        /// Gets or sets a value indicating what window shortcuts will be available during play mode.
        /// </summary>
        [DefaultValue(PlayModeShortcutAvailability.ProfilerOnly)]
        [EditorDisplay("Windows", "Avaliability in Play Mode"), EditorOrder(3000)]
        public PlayModeShortcutAvailability PlayModeWindowShortcutAvaliability { get; set; } = PlayModeShortcutAvailability.ProfilerOnly;

        [DefaultValue(typeof(InputBinding), "Ctrl+Alpha5")]
        [EditorDisplay("Windows"), EditorOrder(3010)]
        public InputBinding ContentWindow = new(KeyboardKeys.Alpha5, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Alpha4")]
        [EditorDisplay("Windows"), EditorOrder(3020)]
        public InputBinding SceneWindow = new(KeyboardKeys.Alpha4, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(3030)]
        public InputBinding ToolboxWindow = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Ctrl+Alpha3")]
        [EditorDisplay("Windows"), EditorOrder(3040)]
        public InputBinding PropertiesWindow = new(KeyboardKeys.Alpha3, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Alpha2")]
        [EditorDisplay("Windows"), EditorOrder(3050)]
        public InputBinding GameWindow = new(KeyboardKeys.Alpha2, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Alpha1")]
        [EditorDisplay("Windows"), EditorOrder(3060)]
        public InputBinding EditorWindow = new(KeyboardKeys.Alpha1, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(3070)]
        public InputBinding DebugLogWindow = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(3080)]
        public InputBinding OutputLogWindow = new(KeyboardKeys.C, KeyboardKeys.Control, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(3090)]
        public InputBinding GraphicsQualityWindow = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(4000)]
        public InputBinding GameCookerWindow = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(4010)]
        public InputBinding ContentSearchWindow = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "None")]
        [EditorDisplay("Windows"), EditorOrder(4020)]
        public InputBinding VisualScriptDebuggerWindow = new(KeyboardKeys.None);

        [DefaultValue(typeof(InputBinding), "Control+Comma")]
        [EditorDisplay("Windows"), EditorOrder(4030)]
        public InputBinding EditorOptionsWindow = new(KeyboardKeys.Comma, KeyboardKeys.Control);

        #endregion

        #region Node Editors

        [DefaultValue(typeof(InputBinding), "Shift+W")]
        [EditorDisplay("Node Editors"), EditorOrder(4500)]
        public InputBinding NodesAlignTop = new(KeyboardKeys.W, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Shift+A")]
        [EditorDisplay("Node Editors"), EditorOrder(4510)]
        public InputBinding NodesAlignLeft = new(KeyboardKeys.A, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Shift+S")]
        [EditorDisplay("Node Editors"), EditorOrder(4520)]
        public InputBinding NodesAlignBottom = new(KeyboardKeys.S, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Shift+D")]
        [EditorDisplay("Node Editors"), EditorOrder(4530)]
        public InputBinding NodesAlignRight = new(KeyboardKeys.D, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Alt+Shift+W")]
        [EditorDisplay("Node Editors"), EditorOrder(4540)]
        public InputBinding NodesAlignMiddle = new(KeyboardKeys.W, KeyboardKeys.Shift, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "Alt+Shift+S")]
        [EditorDisplay("Node Editors"), EditorOrder(4550)]
        public InputBinding NodesAlignCenter = new(KeyboardKeys.S, KeyboardKeys.Shift, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "Q")]
        [EditorDisplay("Node Editors"), EditorOrder(4560)]
        public InputBinding NodesAutoFormat = new(KeyboardKeys.Q);

        [DefaultValue(typeof(InputBinding), "Shift+Q")]
        [EditorDisplay("Node Editors"), EditorOrder(4560)]
        public InputBinding NodesStraightenConnections = new(KeyboardKeys.Q, KeyboardKeys.Shift);

        [DefaultValue(typeof(InputBinding), "Alt+W")]
        [EditorDisplay("Node Editors"), EditorOrder(4570)]
        public InputBinding NodesDistributeHorizontal = new(KeyboardKeys.W, KeyboardKeys.Alt);

        [DefaultValue(typeof(InputBinding), "Alt+A")]
        [EditorDisplay("Node Editors"), EditorOrder(4580)]
        public InputBinding NodesDistributeVertical = new(KeyboardKeys.A, KeyboardKeys.Alt);

        #endregion
    }
}
