using UnityEngine;

namespace PainfulSmile.Runtime.Core
{
    public static class PainfulSmileKeys
    {
        public const string ScriptablePath = "PainfulSmile/Scriptable/";

        public static class Audio
        {
            public static readonly string MasterAudioVolumeKey = "PS_MASTER";
            public static readonly string MusicAudioVolumeKey = "PS_MUSIC";
            public static readonly string SfxAudioVolumeKey = "PS_SFX";

            public static readonly string MasterAudioMuteKey = "PS_MASTER_MUTE";
            public static readonly string MusicAudioMuteKey = "PS_MUSIC_MUTE";
            public static readonly string SfxAudioMuteKey = "PS_SFX_MUTE";
        }

        public static class Options
        {
            public static readonly string FOVKey = "PS_FOV";
            public static readonly string MouseSensibilityKey = "PS_MOUSE_SENSIBILITY";
            public static readonly string GamepadVibrationKey = "PS_GAMEPAD_VIBRATION";
            public static readonly string VsyncKey = "PS_VSYNC";
            public static readonly string FullscreenKey = "PS_FULLSCREEN";
            public static readonly string ResolutionKey = "PS_RESOLUTION";
        }

        public static class Save
        {
            public static readonly string SaveJsonPath = Application.persistentDataPath + "/Saves/save.json";
        }
    }
}