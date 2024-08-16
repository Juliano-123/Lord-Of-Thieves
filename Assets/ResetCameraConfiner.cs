using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ResetCameraConfiner : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;

    ResetConfiner _resetConfiner;

    void Awake()
    {
        _resetConfiner = _virtualCamera.GetComponent<ResetConfiner>();
        //_resetConfiner.ResetCameraConfiner();
        Debug.Log("reseteo gamemanager");
    }


}
