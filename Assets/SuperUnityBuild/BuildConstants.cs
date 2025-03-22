using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        NewReleaseType,
    }

    public enum Platform
    {
        None,
        Windows,
    }

    public enum ScriptingBackend
    {
        None,
        Mono,
    }

    public enum Target
    {
        None,
        Player,
    }

    public enum Distribution
    {
        None,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638781734820122669);
        public const string version = "1.0.0.1";
        public const int buildCounter = 1;
        public const ReleaseType releaseType = ReleaseType.NewReleaseType;
        public const Platform platform = Platform.Windows;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.Mono;
        public const Target target = Target.Player;
        public const Distribution distribution = Distribution.None;
    }
}

