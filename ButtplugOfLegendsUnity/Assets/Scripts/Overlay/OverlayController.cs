using System;
using UnityEngine;
using System.Runtime.InteropServices;

// Only tested with the default render pipeline, might not work with HDRP or URP.
// Use a camera with clear flags = solid color, and color = black with 0 alpha.
// In Project Settings > Player > Resolution and Presentation, disable "Use DXGI Flip Model Swapchain for D3D1" and enable "Run In Background".
public class OverlayController : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    IntPtr hWnd;

    // make transparent 
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    struct MARGINS {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomtHeight;
    }
        
    // make clickthrough
    [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
    static extern int SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);                   
    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    // make topmost
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
    readonly IntPtr HWIN_TOPMOST = new IntPtr(-1);

    void Start()
    {
        Application.quitting += () => SetClickThrough(false);
        if (Application.isFocused == false)
            SetClickThrough(true);
#if !UNITY_EDITOR
        hWnd = GetActiveWindow();
        MARGINS margins = new MARGINS() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins); // makes fullscreen and transparent
        SetWindowPos(hWnd, HWIN_TOPMOST, 0, 0, 0, 0, 0); // makes topmost
        
#endif
    }

    public void SetClickThrough(bool clickThrough)
    {
#if !UNITY_EDITOR
        if (clickThrough)
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        else
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
#endif
    }
}