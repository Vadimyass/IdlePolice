using System;
using Gameplay.Scripts.Utils;
using Lean.Touch;
using UI;
using UI.Scripts.ExitWindow;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputController : IDisposable
{
    #if UNITY_IPHONE
    private float _sensitivity = 0.075f;
#endif
#if !UNITY_IOS
    private float _sensitivity = 0.05f;
    private UIManager _uiManager;
    private bool _isBlock;
#endif
    
    public delegate void DeltaInputPosition(Vector3 position);
    public event DeltaInputPosition DeltaInputPositionEvent;

    public delegate void StopTouchScreen();
    public event StopTouchScreen StopTouchScreenEvent;

    [Inject]
    private void Construct(UIManager uiManager)
    {
        _uiManager = uiManager;
    }
    
    public InputController()
    {
        //Lean.Touch.LeanTouch.OnFingerUpdate += Touching;
        //Lean.Touch.LeanTouch.OnFingerUp += StopTouch;
        UnityEventsHandler.OnUpdate.AddListener(OnUpdate);
    }

    public void SetBlock(bool isBlock)
    {
        _isBlock = isBlock;
    }

    private async void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (await _uiManager.HideLast() == false)
            {
                _uiManager.Show<ExitWindowController>();
            }
        }
    }
    
    private void Touching(Lean.Touch.LeanFinger finger)
    {
        if(_isBlock == true) return;
        
        if (finger.Index != -42)
        {
            if(finger.ScreenPosition == finger.LastScreenPosition) finger.StartScreenPosition = finger.LastScreenPosition;
            
            DeltaInputPositionEvent?.Invoke((finger.ScreenPosition - finger.LastScreenPosition) * (_sensitivity*(1/Time.timeScale)));
        }
    }

    private void StopTouch(Lean.Touch.LeanFinger finger)
    {
        if(_isBlock == true) return;
        
        if (finger.Index != -42)
        {
            StopTouchScreenEvent?.Invoke();
        }
    }
    
    public void Dispose()
    {
        //Lean.Touch.LeanTouch.OnFingerUpdate -= Touching;
        //Lean.Touch.LeanTouch.OnFingerUp -= StopTouch;
    }
}