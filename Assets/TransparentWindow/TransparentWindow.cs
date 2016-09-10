using System;
using System.Runtime.InteropServices;
using UnityEngine;
using KLFrame;
[Singleton]
public class TransparentWindow : KLFrameBase<TransparentWindow>
{
    [SerializeField]
    private Material m_Material;
    public bool drag;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    // Define function signatures to import from Windows APIs

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);



    // Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
    const int WM_SYSCOMMAND = 0x0112;
    const int SC_MOVE = 0xF010;
    const int HTCAPTION = 0x0002;
    static IntPtr hwnd;

    void Start()
    {
        Screen.SetResolution(300, 520, false);
        hwnd = GetActiveWindow();
#if !UNITY_EDITOR
       
        var margins = new MARGINS() { cxLeftWidth = -1 };
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
        
#endif
    }

    // Pass the output of the camera to the custom material
    // for chroma replacement
    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
    void Update()
    {
       /* if (drag) {
            Debug.LogError("on drag");
        }*/

#if !UNITY_EDITOR
        if (drag) { 
        ReleaseCapture();
        SendMessage(hwnd, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);}
#endif
    }

    public static void Minimize() {
       
#if !UNITY_EDITOR
        ShowWindow(hwnd, 2);
#endif
    }

}