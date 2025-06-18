//Handles all of the Camera Values

using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name != "MainMenu")
        {
            LevelBounds = GameObject.FindGameObjectWithTag("PlayableArea").transform.GetChild(0).GetComponent<BoxCollider>();
        }
    }

    private void Start()
    {
        CameraMove = true;
    }
}
