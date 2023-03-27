namespace Managers
{
    using UnityEngine;
   
    ///<summary>Basic singleton template class</summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();
        
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit." +
                                     " Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError("[Singleton] There is more than one instance of '" + typeof(T) + "' in the scene.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();

                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool _applicationIsQuitting = false;

        void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }

}