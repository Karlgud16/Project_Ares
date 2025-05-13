using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Vector3 _velocity;
    private Camera _cam;

    private float _lastCamXPos;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (GameManager.Instance.Players.Count == 0) return;

        Move();
        Zoom();
    }

    void Move()
    {
        //Apply the added offset to the center point of players
        Vector3 centerPoint = GetCenterPoint();
        Vector3 offsetPos = centerPoint + GameManager.Instance.CameraOffset;

        //Add deadzone to camera (going outside of deadzone starts camera movement)
        Vector2 delta = new Vector2(offsetPos.x - transform.position.x, offsetPos.z - transform.position.z);
        if (Mathf.Abs(delta.x) < GameManager.Instance.DeadZone.x)
        {
            offsetPos.x = transform.position.x;
        }
        if (Mathf.Abs(delta.y) < GameManager.Instance.DeadZone.y) offsetPos.z = transform.position.z;

        // Smooth the movement
        Vector3 newPos = Vector3.SmoothDamp(transform.position, offsetPos, ref _velocity, GameManager.Instance.SmoothTime);

        // Clamp to level bounds
        if (GameManager.Instance.LevelBounds != null)
        {
            Bounds bounds = GameManager.Instance.LevelBounds.bounds;

            newPos.x = Mathf.Clamp(newPos.x, bounds.min.x, bounds.max.x);
            newPos.z = Mathf.Clamp(newPos.z, bounds.min.z, bounds.max.z);
        }

        transform.position = newPos;

        //Move the top and bottom colliders depending on where the camera's x position is
        if(GameManager.Instance.PlayerBounds != null)
        {
            Vector3 newColPos = new Vector3(transform.position.x, GameManager.Instance.PlayerBounds.transform.position.y, GameManager.Instance.PlayerBounds.transform.position.z);
            GameManager.Instance.PlayerBounds.transform.position = newColPos;
        }

        //Move the player if it is too far left of the camera while the camera is moving
        float leftLimit = transform.position.x - GameManager.Instance.LeashLimitLeft;

        foreach (GameObject player in GameManager.Instance.Players)
        {
            if (player.transform.position.x < leftLimit)
            {
                Vector3 pos = player.transform.position;
                pos.x = Mathf.MoveTowards(pos.x, leftLimit, GameManager.Instance.LeashPushSpeed * Time.deltaTime);
                player.transform.position = pos;
            }
        }
    }

    void Zoom()
    {
        //Zoom in/out depending on how far right a player goes compared to another player
        float greatestDistance = GetGreatestDistance();
        float newZoom = Mathf.Lerp(GameManager.Instance.MinZoom, GameManager.Instance.MaxZoom, greatestDistance / GameManager.Instance.ZoomLimiter);
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //Gets the center of all of the players combined
    Vector3 GetCenterPoint()
    {
        if (GameManager.Instance.Players.Count == 1)
            return GameManager.Instance.Players[0].transform.position;

        Bounds bounds = new Bounds(GameManager.Instance.Players[0].transform.position, Vector3.zero);
        foreach (GameObject player in GameManager.Instance.Players)
            bounds.Encapsulate(player.transform.position);

        return bounds.center;
    }

    //Finds how big of a gap the total amount of players are to each other
    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(GameManager.Instance.Players[0].transform.position, Vector3.zero);
        foreach (GameObject player in GameManager.Instance.Players)
            bounds.Encapsulate(player.transform.position);

        return Mathf.Max(bounds.size.x, bounds.size.y);
    }
}