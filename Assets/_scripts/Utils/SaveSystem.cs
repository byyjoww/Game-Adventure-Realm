using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Save.Simple
{
    public static class SaveSystem
    {
        public static void SaveName(string name) => Save(PlayerPrefKeys.PLAYER_NAME, name);
        public static string LoadName(string defaultName) => LoadString(PlayerPrefKeys.PLAYER_NAME, defaultName);

        public static void SavePlayerPosition(Vector3 pos)
        {
            Save(PlayerPrefKeys.PLAYER_POS_X, pos.x);
            Save(PlayerPrefKeys.PLAYER_POS_Y, pos.y);
            Save(PlayerPrefKeys.PLAYER_POS_Z, pos.z);
        }
        public static Vector3 LoadPlayerPosition(Vector3 defaultPosition)
        {
            float posX = LoadFloat(PlayerPrefKeys.PLAYER_POS_X, defaultPosition.x);
            float posY = LoadFloat(PlayerPrefKeys.PLAYER_POS_Y, defaultPosition.y);
            float posZ = LoadFloat(PlayerPrefKeys.PLAYER_POS_Z, defaultPosition.z);
            return new Vector3(posX, posY, posZ);
        }

        #region BASE_FUNCTIONS
        public static string LoadString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
        public static int LoadInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
        public static float LoadFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);

        public static void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static void Save(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public static void Save(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }
        #endregion
    }

    public static class PlayerPrefKeys
    {
        public const string PLAYER_NAME = "Save.Name";
        public const string PLAYER_POS_X = "Save.Pos.x";
        public const string PLAYER_POS_Y = "Save.Pos.y";
        public const string PLAYER_POS_Z = "Save.Pos.z";
    }
}