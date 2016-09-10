//author:kuribayashi    2016年8月31日05:24:15
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;

namespace KLFrame
{
    /// <summary>
    /// 音效实用工具类
    /// </summary>
    public static class AudioUtility
    {
       
        /// <summary>
        /// 在指定位置生成音效物体并增加音效脚本
        /// </summary>
        /// <param name="pos">坐标</param>
        /// <returns></returns>
        private static AudioHandler InstantiateAudioObject(Vector3 pos)
        {
            return AudioPool.SpawnAudioHandlerl(pos);
        }

        private static AudioHandler InstantiateAudioObject()
        {
            return AudioPool.SpawnAudioHandlerl();
        }

        /// <summary>
        /// Event统一处理
        /// </summary>
        /// <param name="att"></param>
        /// <param name="handler"></param>
        /*  private static void AudioEventHandle(AudioAttribute att, AudioHandler handler)
          {
              if (att.OnStartPlay != null) handler.OnStartPlay += att.OnStartPlay;
              if (att.OnPausePlay != null) handler.OnPausePlay += att.OnPausePlay;
              if (att.OnStopPlay != null) handler.OnStopPlay += att.OnStopPlay;
          }*/


        public static AudioHandler PlaySoundAsQueue(string[] clipNames)
        {
            AudioData[] audioDataArray = new AudioData[clipNames.Length];
            for (int i = 0; i < audioDataArray.Length; i++) {
                audioDataArray[i] = AudioController.GetAudioData(clipNames[i]);
            }
            return PlaySoundAsQueue(audioDataArray);
        }


        public static AudioHandler PlaySoundAsQueue(AudioData[] ads)
        {
            AudioHandler handler = InstantiateAudioObject();
            handler.AddQueues(ads);
            handler.Play();
            return handler;
        }



        public static AudioHandler PlaySoundAsQueueAtLocation(AudioData[] ads, Vector2 pos)
        {
            AudioHandler handler = InstantiateAudioObject(pos);
            handler.AddQueues(ads);
            handler.Play();
            return handler;
        }

        public static AudioHandler PlaySound(string clipName)
        {
           return PlaySound(AudioController.GetAudioData(clipName));
        }


        public static AudioHandler PlaySound(AudioData ads)
        {
            AudioHandler handler = InstantiateAudioObject();
            handler.AddQueue(ads);
            handler.Play();
            return handler;
        }

        public static AudioHandler PlaySoundAtLocation(string clipName, Vector3 pos)
        {
            AudioData ads = AudioController.GetAudioData(clipName);
            return PlaySoundAtLocation(ads,pos);
        }

        /// <summary>
        /// 在指定位置播放音效
        /// </summary>
        /// <param name="att">声音属性设置</param>
        /// <param name="pos">坐标</param>
        public static AudioHandler PlaySoundAtLocation(AudioData ads, Vector3 pos)
        {
            
            AudioHandler handler = InstantiateAudioObject(pos);
            handler.AddQueue(ads);
          //  Debug.Log(ads);
            handler.Play();
            return handler;
        }

        /// <summary>
        /// 在指定位置随机使用数组中的某一声音属性设置
        /// </summary>
        /// <param name="audios">声音属性设置</param>
        /// <param name="pos">坐标</param>
        public static AudioHandler PlayRandomSoundAtLocation(AudioData[] audios, Vector3 pos)
        {
            if (audios.Length == 0) return null;
            int random = Random.Range(0, audios.Length);
            return PlaySoundAtLocation(audios[random], pos);
        }

        public static bool SetAudioConfigWhitTagName(string tagName,AudioConfigData config) {
            AudioConfigData data = AudioController.GetAudioConfigDataById(AudioConfig.dic_Config[tagName]);
            if (data != null) {
                data = config;
                return true;
            }
            else return false;
        }

        
    }
   
    [System.Serializable]
    public class AudioData
    {
        public AudioClip audioClip;
        public int tag;
        public int id;
    }
    [System.Serializable]
    public class AudioConfigData {
        public string tagName;
        public AudioMixerGroup output;
        public bool bypassEffects;
        public bool bypassLisenterEffects;
        public bool bypassReverbZones;
        public bool loop;
        [Range(0, 256)]
        public int priority=128;
        [Range(0, 1)]
        public float volume=1;
        [Range(0, 1)]
        public float pitch;
        [Range(-1, 1)]
        public float stereoPan;
        [Range(0, 1)]
        public float spatialBlend;
        [Range(0, 1.1f)]
        public float reverbZonMix=1;
        [Range(0, 10000)]
        public float maxDistance=500;
        [Range(0, 10000)]
        public float minDistance=1;
    }
}