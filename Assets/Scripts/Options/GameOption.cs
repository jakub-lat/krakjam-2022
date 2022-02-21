using System;
using UnityEngine;

namespace Options
{
    public class GameOption<T> where T : IComparable, IConvertible 
    {
        private string playerPrefsKey;
        private Action<T> setter;
        private T defaultVal;

        public T Value { get; private set; }

        public void Set(T newVal)
        {
            Value = newVal;
            setter(newVal);
            Save(newVal);
        }

        public GameOption(string playerPrefsKey, Action<T> setter, T defaultVal)
        {
            this.defaultVal = defaultVal;
            this.playerPrefsKey = playerPrefsKey;
            this.setter = setter;

            Value = Load();
            setter(Value);
        }
        
        private T Load()
        {
            if (!PlayerPrefs.HasKey(playerPrefsKey)) return defaultVal;
            
            return default(T) switch
            {
                int => (T)(object)PlayerPrefs.GetInt(playerPrefsKey),
                float => (T)(object)PlayerPrefs.GetFloat(playerPrefsKey),
				string => (T)(object)PlayerPrefs.GetString(playerPrefsKey),
				_ => JsonUtility.FromJson<T>(PlayerPrefs.GetString(playerPrefsKey))
            };
        }

        private void Save(T newVal)
        {
            switch (newVal)
            {
                case int i:
                    PlayerPrefs.SetInt(playerPrefsKey, i);
                    break;
                case float f:
                    PlayerPrefs.SetFloat(playerPrefsKey, f);
                    break;
                case string s:
                    PlayerPrefs.SetString(playerPrefsKey, s);
                    break;
                default:
                    PlayerPrefs.SetString(playerPrefsKey, JsonUtility.ToJson(newVal));
                    break;
            }
        }
    }
}
