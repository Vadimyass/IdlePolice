using System;
using System.Collections;
using System.Collections.Generic;
using Tayx.Graphy;
using UnityEngine;

public class FpsmetrActivator : MonoBehaviour
{
    [SerializeField] private GameObject _fpsMetr;

    private bool EditorInput => Input.GetKeyDown(KeyCode.T);
    private void Update()
    {
        if(Input.touchCount == 5 || EditorInput)
        {
            _fpsMetr.SetActive(!_fpsMetr.activeSelf);
        }
    }
}
