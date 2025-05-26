using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    private CameraManager _cameraManager;

    private void Start()
    {
        _cameraManager = GameManager.Instance.GetComponent<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && _cameraManager.CameraMove == true)
        {
            _cameraManager.CameraMove = false;  
            Destroy(gameObject);
        }
    }
}
