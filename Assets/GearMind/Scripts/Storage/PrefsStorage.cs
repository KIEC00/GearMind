using System;
using UnityEngine;

namespace Assets.GearMind.Storage
{
    public class PrefsStorage : IStorage<string, string>, IDisposable
    {
        public string Load(string key) =>
            PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : null;

        public void Save(string key, string value) => PlayerPrefs.SetString(key, value);

        public void Dispose() => PlayerPrefs.Save();
    }
}
