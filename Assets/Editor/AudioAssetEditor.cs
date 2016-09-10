using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using KLFrame;
using System.Reflection;
using System;

[CustomEditor(typeof(AudioAsset))]
public class AudioAssetEditor : Editor
{
    private AudioAsset audioAsset;
    private bool[] playFlag = new bool[100];
    private AudioClip playingClip;
    private int clipFlagIndex;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if (audioAsset == null) {
            audioAsset = (AudioAsset)target;
        }
        if (playingClip) {
            if (!IsClipPlaying(playingClip)) {
                playingClip = null;
                playFlag[clipFlagIndex] = false;
            }                    
        }    
        if (audioAsset.audioData == null) return;
        for (int i = 0; i < audioAsset.audioData.Count; i++)
        {

            if (audioAsset.audioData[i] == null) return;
            audioAsset.audioData[i].audioClip = (AudioClip)EditorGUILayout.ObjectField("Clip", audioAsset.audioData[i].audioClip, typeof(AudioClip), true);

            if (AudioConfig.GetAllTagName().Length == 0)
            {
                AudioConfig config = Resources.Load<AudioConfig>("Audio/AudioSetting/" + typeof(AudioConfig).ToString());
                config.Initialize();
            }
            audioAsset.audioData[i].tag = EditorGUILayout.Popup("选择类型", audioAsset.audioData[i].tag, AudioConfig.GetAllTagName());
            audioAsset.audioData[i].id = i;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID:" + audioAsset.audioData[i].id);
            if (GUILayout.Button(GetFlag(i)))
            {
                if (audioAsset.audioData[i].audioClip != null)
                {
                    if (playFlag[i])
                    {
                        playFlag[i] = false;
                        playingClip = null;
                        StopAllClips();
                        Repaint();
                    }
                    else
                    {
                        if (playingClip) return;
                        PlayClip(audioAsset.audioData[i].audioClip);
                        playFlag[i] = true;
                        playingClip = audioAsset.audioData[i].audioClip;
                        clipFlagIndex = i;
                        Repaint();
                    }
                }
            }
            if (GUILayout.Button("删除音频", GUILayout.Height(25)))
            {
                audioAsset.audioData.Remove(audioAsset.audioData[i]);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("新增音频"))
        {
            audioAsset.audioData.Add(null);
           
        }
    }


    


    private GUIContent GetFlag(int i)
    {
        if (playFlag[i]) return EditorGUIUtility.IconContent("PauseButton Anim");
        else return EditorGUIUtility.IconContent("PlayButton Anim");
    }

    public static void PlayClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { typeof(AudioClip) }, null);
        method.Invoke(null, new object[] { clip });
    }
    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod("StopAllClips", BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { }, null);
        method.Invoke(
            null,
            new object[] { }
        );
    }

    public static bool IsClipPlaying(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "IsClipPlaying",
            BindingFlags.Static | BindingFlags.Public
            );

        bool playing = (bool)method.Invoke(
            null,
            new object[] {
                clip,
        }
        );

        return playing;
    }
}
