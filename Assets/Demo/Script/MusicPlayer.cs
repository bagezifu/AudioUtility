using UnityEngine;
using System.Collections;
using KLFrame;
using UnityEngine.UI;

[Singleton]
public class MusicPlayer : KLFrameBase<MusicPlayer>
{

    public AudioHandler handler;
    public Button[] list_button;

    void Start()
    {
        GameObject listButtonPanel = GameObject.FindGameObjectWithTag("ListButtonPanel");
        handler = AudioUtility.PlaySoundAsQueue(new string[3] { "Blumenkranz", "DOES", "Shinnchann" });
        handler.notDespawn = true;
        handler.Pause();
        list_button = new Button[handler.Length];
        for (int i = 0; i < handler.Length; i++) {
            list_button[i] = listButtonPanel.transform.GetChild(i).GetComponent<Button>();
            list_button[i].SetText(handler[i].audioClip.name);
        }
    }

   
}
