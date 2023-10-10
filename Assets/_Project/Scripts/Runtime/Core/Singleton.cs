using UnityEngine;

namespace PainfulSmile.Runtime.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                string typename = typeof(T).Name;
                Debug.LogWarning($"More that one instance of {typename} found.");
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}