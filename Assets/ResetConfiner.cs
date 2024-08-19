using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class ResetConfiner : MonoBehaviour
{

    [SerializeField]
    CreadorMounstruos _creadorMounstruos;

    private void Start()
    {
        ResetCameraConfiner();
    }

    private void Update()
    {
        if (_creadorMounstruos._mostrosTotales == 20)
        {
            ResetCameraConfiner();
        }
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    //static void OnAfterSceneLoad()
    //{
    //    //ResetConfiner _resetConfiner = gameObject.AddComponent(typeof(ResetConfiner)) as ResetConfiner;
    //    //_resetConfiner.ResetCameraConfiner();
    //    //Debug.Log("First scene loaded: After Awake is called.");
    //}

    public void ResetCameraConfiner()
    {
        GetComponent<CinemachineConfiner2D>().InvalidateCache();   
    }



}