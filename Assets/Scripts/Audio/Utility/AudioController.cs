using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KLFrame
{
    [Singleton]
    public class AudioController:KLFrameBase<AudioController>
    {
        public static Dictionary<string, int> dic_audioAsset = new Dictionary<string, int>();
        public static Dictionary<int, AudioConfigData> dic_audioConfig = new Dictionary<int, AudioConfigData>();
        public static AudioAsset audioAsset;
        public static AudioConfig audioConfig;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            GameObject.DontDestroyOnLoad(new GameObject("AudioController", typeof(AudioController))
            {
               // hideFlags = HideFlags.HideInHierarchy
            });
          
            audioAsset = Resources.Load<AudioAsset>("Audio/AudioSetting/"+typeof(AudioAsset).ToString());
            foreach (AudioData a in audioAsset.audioData) {
                if(a.audioClip)
                 dic_audioAsset.Add(a.audioClip.name,a.id);
            }
            audioConfig= Resources.Load<AudioConfig>("Audio/AudioSetting/" + typeof(AudioConfig).ToString());
            for (int i = 0; i < audioConfig.configs.Length; i++) {
                dic_audioConfig.Add(i, audioConfig.configs[i]);
            }
        }
       
        public static int GetAudioDataIdByName(string clipName)
        {
            return AudioController.dic_audioAsset[clipName];
        }
        public static AudioData GetAudioDataById(int id)
        {
            return AudioController.audioAsset.audioData[id];
        }
        public static AudioConfigData GetAudioConfigDataById(int id)
        {
            return AudioController.dic_audioConfig[id];
        }
        public static AudioData GetAudioData(string clipName) {
         return   GetAudioDataById(GetAudioDataIdByName(clipName));
        }
        
    }
}