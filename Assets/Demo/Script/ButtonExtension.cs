using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class ButtonExtension   {


    public static void SetText(this Button btn,string text) {
        btn.GetComponentInChildren<Text>().text = text;
    }
    public static void SetColor(this Button btn,Color color) {
        btn.GetComponent<Image>().color = color;
    }
}
