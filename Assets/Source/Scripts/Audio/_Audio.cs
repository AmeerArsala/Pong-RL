// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.Audio;

using static Audio.Constants;
using static Audio.Helpers;

namespace Audio {
    public static class Constants {
        public const string SFX_CONFIG_FILEPATH = "Game/Data/Settings/sfxconfig.json";
        public static readonly SfxMappings SFX_MAPPINGS = DeserializeSfxMappings();

        private static SfxMappings DeserializeSfxMappings() {
            string filePath = Application.dataPath + "/" + SFX_CONFIG_FILEPATH;

            // Read JSON file as text
            string jsonRaw = System.IO.File.ReadAllText(filePath);

            // Deserialize from JSON to C# Object SfxMappings
            SfxMappings sfxMappings = JsonUtility.FromJson<SfxMappings>(jsonRaw);

            return sfxMappings;
        }
    }

    public static class Cache {
        public static SfxPack SFX;
    }

    public static class Helpers {
        public static AudioType IdentifyAudioType(string audioFilePath) {
            if (audioFilePath.EndsWith(".mp3")) {
                return AudioType.MPEG;
            }

            if (audioFilePath.EndsWith(".wav")) {
                return AudioType.WAV;
            }

            if (audioFilePath.EndsWith(".ogg")) {
                return AudioType.OGGVORBIS;
            }

            return AudioType.UNKNOWN;
        }

        /*public static async Task<AudioClip> LoadAudioClip(string audioFilePath, AudioType audioType) {
            // Create Unity Web Request to load Audio File
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioFilePath, audioType)) {
                var result = www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError("Failed to load audio: " + www.error);
                    return null;
                } else {
                    return DownloadHandlerAudioClip.GetContent(www);;
                }
            }
        }

        public static async Task<AudioClip> LoadAudioClip(string audioFilePath) {
            return await LoadAudioClip(audioFilePath, IdentifyAudioType(audioFilePath));
        }

        public static async Task<AudioClip> sfx(string filename) {
            // relative to Assets/
            string filepath = "Content/Audio/SFX/" + filename;

            return await LoadAudioClip(filepath, IdentifyAudioType(filepath));
        }*/
    }

    [System.Serializable]
    public class SfxMappings {
        // pong game sounds
        public string paddleHit;
        public string wallHit;
        public string scoreSound;
        public string gameResult;
        
        // menu sounds
        public string hoverOption;
        public string selectOption;

        public SfxMappings(
            string paddleHit, string wallHit, string scoreSound, string gameResult,
            string hoverOption, string selectOption) 
        {
            this.paddleHit = paddleHit;
            this.wallHit = wallHit;
            this.scoreSound = scoreSound;
            this.gameResult = gameResult;
            this.hoverOption = hoverOption;
            this.selectOption = selectOption;
        }
    }

    public class SfxPack {
        // pong game sounds
        public AudioClip paddleHit;
        public AudioClip wallHit;
        public AudioClip scoreSound;
        public AudioClip gameResult;
        
        // menu sounds
        public AudioClip hoverOption;
        public AudioClip selectOption; 

        public SfxPack(
            AudioClip paddleHit, AudioClip wallHit, AudioClip scoreSound, AudioClip gameResult,
            AudioClip hoverOption, AudioClip selectOption) 
        {
            this.paddleHit = paddleHit;
            this.wallHit = wallHit;
            this.scoreSound = scoreSound;
            this.gameResult = gameResult;
            this.hoverOption = hoverOption;
            this.selectOption = selectOption;
        }

        /*public static SfxPack FromMappings(SfxMappings sfxMappings) {
            return new SfxPack(
                sfx(sfxMappings.paddleHit),
                sfx(sfxMappings.wallHit),
                sfx(sfxMappings.scoreSound),
                sfx(sfxMappings.gameResult),
                sfx(sfxMappings.hoverOption),
                sfx(sfxMappings.selectOption)
            );
        }

        public static SfxPack FromRegisteredMappings() {
            return FromMappings(Constants.SFX_MAPPINGS);
        }*/
    }
}