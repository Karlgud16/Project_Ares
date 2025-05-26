using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Vector3 _velocity;
    private Camera _cam;

    private float _lastCamXPos;

    private CameraManager _cameraManager;
    private PlayerManager _playerManager;

    void Start()
    {
        _cam = GetComponent<Camera>();

        _cameraManager = GameManager.Instance.GetComponent<CameraManager>();
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();

    }

    void LateUpdate()
    {
        if (_cameraManager.CameraMove || _playerManager.Players.Count > 0)
        {
            Move();
            Zoom();
        }
    }

    void Move()
    {
        //Apply the added offset to the center point of players
        Vector3 centerPoint = GetCenterPoint();
        Vector3 offsetPos = centerPoint + _cameraManager.CameraOffset;

        //Add deadzone to camera (going outside of deadzone starts camera movement)
        Vector2 delta = new Vector2(offsetPos.x - transform.position.x, offsetPos.z - transform.position.z);
        if (Mathf.Abs(delta.x) < _cameraManager.DeadZone.x)
        {
            offsetPos.x = transform.position.x;
        }
        if (Mathf.Abs(delta.y) < _cameraManager.DeadZone.y) offsetPos.z = transform.position.z;

        // Smooth the movement
        Vector3 newPos = Vector3.SmoothDamp(transform.position, offsetPos, ref _velocity, _cameraManager.SmoothTime);

        // Clamp to level bounds
        if (_cameraManager.LevelBounds != null)
        {
            Bounds bounds = _cameraManager.LevelBounds.bounds;

            newPos.x = Mathf.Clamp(newPos.x, bounds.min.x, bounds.max.x);
            newPos.z = Mathf.Clamp(newPos.z, bounds.min.z, bounds.max.z);
        }

        transform.position = newPos;

        //Move the player if it is too far left of the camera while the camera is moving
        float leftLimit = transform.position.x - _cameraManager.LeashLimitLeft;

        foreach (GameObject player in _playerManager.Players)
        {
            if (player.transform.position.x < leftLimit)
            {
                Vector3 pos = player.transform.position;
                pos.x = Mathf.MoveTowards(pos.x, leftLimit, _cameraManager.LeashPushSpeed * Time.deltaTime);
                player.transform.position = pos;
            }
        }
    }

    void Zoom()
    {
        //Zoom in/out depending on how far right a player goes compared to another player
        float greatestDistance = GetGreatestDistance();
        float newZoom = Mathf.Lerp(_cameraManager.MinZoom, _cameraManager.MaxZoom, greatestDistance / _cameraManager.ZoomLimiter);
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //Gets the center of all of the players combined
    Vector3 GetCenterPoint()
    {
        if (_playerManager.Players.Count == 1)
            return _playerManager.Players[0].transform.position;

        Bounds bounds = new Bounds(_playerManager.Players[0].transform.position, Vector3.zero);
        foreach (GameObject player in _playerManager.Players)
            bounds.Encapsulate(player.transform.position);

        return bounds.center;
    }

    //Finds how big of a gap the total amount of players are to each other
    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(_playerManager.Players[0].transform.position, Vector3.zero);
        foreach (GameObject player in _playerManager.Players)
            bounds.Encapsulate(player.transform.position);

        return Mathf.Max(bounds.size.x, bounds.size.y);
    }
}