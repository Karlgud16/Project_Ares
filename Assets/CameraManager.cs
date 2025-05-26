using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera")]
    [ReadOnly] public bool CameraMove;
    [Range(0, 1)] public float SmoothTime = 0.3f;
    public float MinZoom = 5f;
    public float MaxZoom = 12f;
    public float ZoomLimiter = 50f; // Max expected distance between players
    public Vector2 DeadZone = new Vector2(1f, 1f); // X and Y deadzone
    public Vector3 CameraOffset;
    public BoxCollider LevelBounds;
    public float LeashLimitLeft = 10f; // Distance behind camera center on X axis
    public float LeashPushSpeed = 5f;  // Speed to move players forward

    private void Start()
    {
        CameraMove = true;
    }

}
