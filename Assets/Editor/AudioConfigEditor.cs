 using UnityEngine;
using System.Collections;
using UnityEditor;
using KLFrame;
[CustomEditor(typeof(AudioConfig))]
public class AudioConfigEditor : Editor {
    public AudioConfig audioConfigData;
   

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (audioConfigData==null) {
            audioConfigData = (AudioConfig)target;
        }
        audioConfigData.Initialize();
            
       
    }
    
}
