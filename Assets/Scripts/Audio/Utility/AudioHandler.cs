//author:kuribayashi     2016年8月31日05:25:58
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KLFrame
{
    /// <summary>
    /// 声音处理类
    /// 继承自MonoBehaviour,存在于场景中
    /// </summary>
    public class AudioHandler : MonoBehaviour, IEnumerable
    {

        public AudioSource source;
        public delegate void SoundDelegate(UnityEngine.Object sender, AudioEventArgs arg);
        //开始播放
        public SoundDelegate OnStartPlay;
        //暂停播放
        public SoundDelegate OnPausePlay;
        //停止播放|播放结束
        public SoundDelegate OnStopPlay;

        public bool notDespawn { get; set; }

        public float intervalTime;
        //系统音源组件

        private List<AudioData> AudioQueues = new List<AudioData>();
        private int queuesIndex = -1;
        public bool isPause = false;
        private List<Coroutine> Cor_Stop=new List<Coroutine>();


        void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();

        }

        public void AddQueue(AudioData att)
        {
            AudioQueues.Add(att);
        }

        public void AddQueues(AudioData[] atts)
        {
            AudioQueues.AddRange(atts);
        }

        /// <summary>
        /// 执行开始播放
        /// </summary>
        /// <param name="att">声音属性类</param>
        public void Play()
        {

            if (isPause)
            {
                source.Play();
                Cor_Stop.Add(StartCoroutine(StopEvent(source.clip.length - source.time)));
                isPause = false;
            }
            else
            {
                source.time = 0;
                Play(queuesIndex + 1);
            }

        }


        public int Play(int index)
        {

            if (index > AudioQueues.Count - 1 || index < 0) return -1;
            else
            {
                StopAllAudioStopCoroutine();
                queuesIndex = index;
                ApplyAudioConfigAndPlay(AudioController.GetAudioConfigDataById(AudioQueues[queuesIndex].tag));
                if (OnStartPlay != null)
                    OnStartPlay(this, new AudioEventArgs(AudioQueues[queuesIndex].audioClip.name, queuesIndex));
                if (!source.loop) Cor_Stop.Add(StartCoroutine(StopEvent(source.clip.length)));
                source.time = 0;
                source.Play();
                return index;
            }

        }

        


        /// <summary>
        /// 执行暂停播放
        /// </summary>
        public void Pause()
        {
            isPause = true;
            source.Pause();
            if (OnPausePlay != null)
                OnPausePlay(this, new AudioEventArgs(source.clip.name, queuesIndex));
            StopAllCoroutines();
        }
        /// <summary>
        /// 执行停止播放
        /// </summary>
        public void Stop()
        {
            source.Stop();
            if (OnStopPlay != null)
                OnStopPlay(this, new AudioEventArgs(source.clip.name, queuesIndex));
            StopAllAudioStopCoroutine();
        }
        /// <summary>
        /// 协程延时执行声音播放完成事件
        /// </summary>
        /// <param name="time">延迟时间</param>
        /// <returns></returns>
        IEnumerator StopEvent(float time)
        {
            yield return new WaitForSeconds(time * Time.timeScale);
            if (OnStopPlay != null)
            {
                OnStopPlay(this, new AudioEventArgs(source.clip.name, queuesIndex));
            }
            if (queuesIndex + 1 == AudioQueues.Count)
            {

                // Destroy(this.gameObject);
                if (!notDespawn)
                {
                    AudioPool.DeSpawn(this);
                }

            }
            else
            {
                Debug.LogError("on stop going next");
                Play();
            }
        }

        public void FadeOut(AudioSource source, float duration)
        {
            StartCoroutine(Cor_Fade(FadeType.Out, source, duration));
        }

        public void FadeIn(AudioSource source, float duration)
        {
            source.volume = 0;
            StartCoroutine(Cor_Fade(FadeType.In, source, duration));
        }

        IEnumerator Cor_Fade(FadeType ft, AudioSource source, float duration)
        {
            float intervalCount = duration / intervalTime;
            float tween = 1.0f / intervalCount;
            for (;;)
            {
                switch (ft)
                {
                    case FadeType.In:
                        source.volume += tween;
                        break;
                    case FadeType.Out:
                        source.volume -= tween;
                        break;
                }
                yield return new WaitForSecondsRealtime(intervalTime);
                if (source.volume == 1 || source.volume == 0) StopCoroutine("Cor_Fade");
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new AudioHandlerEnumerator(AudioQueues.ToArray());
        }

        public AudioData this[int index]
        {
            get { if (index < AudioQueues.Count && index >= 0) return AudioQueues[index]; else return null; }
            set { if (index < AudioQueues.Count && index >= 0) AudioQueues[index] = value; }
        }

        public void SetTime(float time)
        {
            StopAllAudioStopCoroutine();
            float realTime = Mathf.Clamp(time, 0, source.clip.length - 0.1f);
            source.time = realTime;
            Cor_Stop.Add( StartCoroutine(StopEvent(AudioQueues[queuesIndex].audioClip.length - realTime)));
        }
        public float GetTime()
        {
            return source.time;
        }
        public float GetCurrentDuration()
        {
            return source.clip.length;
        }

        public void ApplyAudioConfigAndPlay(AudioConfigData config)
        {
            source.playOnAwake = false;
            source.clip = AudioQueues[queuesIndex].audioClip;
            source.bypassEffects = config.bypassEffects;
            source.bypassListenerEffects = config.bypassLisenterEffects;
            source.bypassReverbZones = config.bypassReverbZones;
            source.loop = config.loop;
            source.priority = config.priority;
            source.volume = config.volume;
            source.pitch = config.pitch;
            source.spatialBlend = config.spatialBlend;
            source.reverbZoneMix = config.reverbZonMix;
            source.maxDistance = config.maxDistance;
            source.minDistance = config.minDistance;
            source.Play();
        }

        public int PlayNext()
        {

            return Play(queuesIndex + 1);
        }
        public int PlayLast()
        {

            return Play(queuesIndex - 1);
        }

        public int Length { get { return AudioQueues.Count; } }
        public float Volume { get { return source.volume; } set { source.volume = value; } }

        void OnDisable()
        {
            // Debug.Log("ondisable");
            AudioQueues.Clear();
            queuesIndex = -1;
            OnStartPlay = null;
            OnStopPlay = null;
            OnPausePlay = null;
            source.enabled = false;
        }

        void OnEnable()
        {
            source.enabled = true;
        }
        void OnDestroy()
        {
            Destroy(source);
        }

        void StopAllAudioStopCoroutine() {
            Debug.Log(Cor_Stop.Count);
            for (int i = 0; i < Cor_Stop.Count; i++) {
                StopCoroutine(Cor_Stop[i]);
                Cor_Stop.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 声音事件参数
    /// </summary>
    public class AudioEventArgs : EventArgs
    {
        //声音片段名称
        public string ClipName { get; set; }
        public int queueIndex { get; set; }
        public AudioEventArgs(string name, int index)
        {
            ClipName = name;
            queueIndex = index;
        }
    }
    /// <summary>
    /// 淡入淡出枚举
    /// </summary>
    public enum FadeType
    {
        In,
        Out
    }
}