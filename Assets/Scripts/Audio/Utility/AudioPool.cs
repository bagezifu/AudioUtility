using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace KLFrame
{

    public class AudioPool : MonoBehaviour
    {

        private static AudioPool instance;
        public static List<AudioHandler> list_unused = new List<AudioHandler>();
        public static List<AudioHandler> list_using = new List<AudioHandler>();
        public static List<GameObject> list_unusedObj = new List<GameObject>();
        public static List<GameObject> list_usingObj = new List<GameObject>();

        private static AudioListener listener;

        public static int poolMax = 5;
        public static float cullInterval = 1;

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            listener = GameObject.FindObjectOfType<AudioListener>();
            if (!instance)
            {
                GameObject pool = new GameObject("AudioPool") { hideFlags = HideFlags.HideInHierarchy };
                instance = pool.AddComponent<AudioPool>();
            }
        }

        void Awake()
        {
            StartCoroutine(AutoCull());
        }


        IEnumerator AutoCull()
        {
            for (;;)
            {
                if (list_unused.Count > poolMax)
                {
                    AudioHandler handler = list_unused[list_unused.Count - 1];
                    list_unused.Remove(handler);
                    Destroy(handler);
                }
                if (list_unusedObj.Count > poolMax)
                {
                    GameObject go = list_unusedObj[list_unusedObj.Count - 1];
                    list_unusedObj.Remove(go);
                    Destroy(go);
                }
               // Debug.Log("auto cull");
                yield return new WaitForSeconds(cullInterval);
            }
        }

        public static AudioHandler SpawnAudioHandlerl()
        {
            if (list_unused.Count == 0)
            {
                AudioHandler handler = listener.gameObject.AddComponent<AudioHandler>();
                list_using.Add(handler);
                return handler;
            }
            else
            {
                AudioHandler handler = list_unused[0];
                handler.enabled = true;
                list_unused.RemoveAt(0);
                list_using.Add(handler);
                return handler;
            }
        }
        public static void DeSpawn(AudioHandler handler)
        {
            if (handler.gameObject.name == "AudioPoolDespawnable")
            {
                list_usingObj.Remove(handler.gameObject);
                list_unusedObj.Add(handler.gameObject);
                handler.gameObject.SetActive(false);
            }
            else
            {
                list_using.Remove(handler);
                list_unused.Add(handler);
                handler.enabled = false;
            }

        }
        public static AudioHandler SpawnAudioHandlerl(Vector3 pos)
        {
            if (list_unusedObj.Count == 0)
            {
                GameObject go = new GameObject("AudioPoolDespawnable");
                go.transform.SetParent(AudioController.GetInstance().transform);
                go.transform.position = pos;
                AudioHandler handler = go.AddComponent<AudioHandler>();
                list_usingObj.Add(go);
                return handler;
            }
            else
            {
                AudioHandler handler = list_unusedObj[0].GetComponent<AudioHandler>();
                handler.gameObject.SetActive(true);
                list_usingObj.Add(handler.gameObject);
                list_unusedObj.RemoveAt(0);
                return handler;
            }
        }

    }
}