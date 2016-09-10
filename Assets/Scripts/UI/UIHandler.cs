using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using KLFrame;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,IDragHandler
{
    public ButtonType buttonType;
    private Slider sliderTime;
    private Slider sliderVolume;
    private bool sliderTimeHandler = false;
    private float volume = 1;

    public void OnPointerClick(PointerEventData eventData)
    {

        switch (buttonType)
        {
            case ButtonType.None:
                break;
            case ButtonType.ListButton:
                Play(transform.GetSiblingIndex());
                break;
            case ButtonType.Next:
               ButtonIndex(Next());
                break;
            case ButtonType.Last:
               ButtonIndex(Last());
                break;
            case ButtonType.Play:
                Play();
                break;
            case ButtonType.Exit:
                Application.Quit();
                break;
            case ButtonType.Minimize:
                TransparentWindow.Minimize();
                break;
            default:
                break;
        }
    }

    void Play(int index)
    {
        MusicPlayer.GetInstance().handler.Play(index);
        ButtonIndex(index);
    }

    void ButtonIndex(int index)
    {
        for (int i = 0; i < MusicPlayer.GetInstance().list_button.Length; i++)
        {
        
            if (i == index) { MusicPlayer.GetInstance().list_button[i].SetColor(Color.black); }
            else MusicPlayer.GetInstance().list_button[i].SetColor(new Color(0.441f,0.441f,0.441f,1));
        }
    }
    void Start()
    {
        switch (buttonType)
        {
            case ButtonType.Slider_Time:
                sliderTime = GetComponent<Slider>();
                break;
            case ButtonType.Slider_Volume:
                sliderVolume = GetComponent<Slider>();
                break;
        }
        Play(0);
        MusicPlayer.GetInstance().handler.OnStopPlay = (sender, arg) => { ButtonIndex(arg.queueIndex);if (arg.queueIndex == ((AudioHandler)sender).Length - 1) { Play(0); }Debug.Log("on stop" + arg.queueIndex); };
    }

    void Update()
    {
        if (sliderTime && !sliderTimeHandler && MusicPlayer.GetInstance().handler)
        {
           
            sliderTime.value = Mathf.Clamp(MusicPlayer.GetInstance().handler.GetTime() / MusicPlayer.GetInstance().handler.GetCurrentDuration(), 0, 1);
        }
        if (sliderVolume)
        { 
            MusicPlayer.GetInstance().handler.Volume = volume;
        }

    }


    void Play()
    {

        if (MusicPlayer.GetInstance().handler.isPause)
        {
            this.GetComponent<Button>().SetText("播放");
            MusicPlayer.GetInstance().handler.Play();
        }
        else
        {
            this.GetComponent<Button>().SetText("暂停");
            MusicPlayer.GetInstance().handler.Pause();
        }
    }
    int Last()
    {
        int playIndex = MusicPlayer.GetInstance().handler.PlayLast();
        if (playIndex == -1)
        {
            Debug.Log("-1");
            return  MusicPlayer.GetInstance().handler.Play(MusicPlayer.GetInstance().handler.Length - 1);
        }
        return playIndex;
    }
    int Next()
    {
        int playIndex = MusicPlayer.GetInstance().handler.PlayNext();
        if (playIndex == -1)
        {
            Debug.Log("-1");
         return   MusicPlayer.GetInstance().handler.Play(0);
        }
        return playIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Slider_Time:
                sliderTimeHandler = true;
                break;
            case ButtonType.Title:
                Debug.LogError("drag true");
                TransparentWindow.GetInstance().drag = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Slider_Time:
                sliderTimeHandler = false;
                ValueChanage();
                break;
            case ButtonType.Title:
                Debug.LogError("drag false");
                TransparentWindow.GetInstance().drag = false;
                break;
        }
    }

    public void ValueChanage()
    {
        Debug.Log("SET TIME");
        MusicPlayer.GetInstance().handler.SetTime(MusicPlayer.GetInstance().handler.GetCurrentDuration() * sliderTime.value);
    }

    public void OnVolumeChange()
    {
        SetVolume();
    }

    void SetVolume()
    {
        volume = sliderVolume.value;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Title:
                Debug.LogError("drag false");
                TransparentWindow.GetInstance().drag = false;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }
}
public enum ButtonType
{
    None,
    ListButton,
    Next,
    Last,
    Play,
    Slider_Time,
    Title,
    Slider_Volume,
    Exit,
    Minimize
}

