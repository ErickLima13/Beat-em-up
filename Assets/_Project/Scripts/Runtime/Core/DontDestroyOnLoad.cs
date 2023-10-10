using UnityEngine;

namespace PainfulSmile.Runtime.Core
{
    public class DontDestroyOnLoad<T> : MonoBehaviour where T : Component
    {
        public bool debugText;
        public bool blocksOtherInstances = true;

        protected virtual void Awake()
        {
            if (blocksOtherInstances)
            {
                if (FindObjectsOfType<T>().Length > 1)
                {
                    string typename = typeof(T).Name;

                    if (debugText)
                        Debug.LogWarning($"More that one instance of {typename} found. Removing other instance...");

                    Destroy(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}