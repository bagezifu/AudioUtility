using System.Collections.Generic;
using UnityEngine;


namespace KLFrame
{
    public class AudioConfig : ScriptableObject
    {


        [SerializeField]
        [Header("配置音频设置")]
        public AudioConfigData[] configs;
        public static AudioConfig instance;
        private static AudioConfigData[] CONFIGS;
        public static Dictionary<string, int> dic_Config = new Dictionary<string, int>();



        public void Initialize()
        {
            if (CONFIGS == null)
            {

                for (int i = 0; i < configs.Length; i++)
                {
                    if (configs[i].tagName == string.Empty)
                    {
                        Debug.LogError("声音配置文件TagName不能为空!!");
                        return;
                    }
                    dic_Config.Add(configs[i].tagName,i);
                }

                CONFIGS = configs;
            }
        }

        void OnEnable()
        {
            Initialize();
        }
        public static string[] GetAllTagName()
        {
            if (CONFIGS == null) return new string[0];
            string[] tagNames = new string[CONFIGS.Length];
            for (int i = 0; i < tagNames.Length; i++)
            {
                tagNames[i] = CONFIGS[i].tagName;
            }
            return tagNames;
        }

    }
}