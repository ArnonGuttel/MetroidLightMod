using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed;
    public float delayCounter;

    #endregion

    #region Fields

    private int _waypointsArrayIndex;
    private float _enemySpeed;

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        _waypointsArrayIndex = 0;
        _enemySpeed = speed;
    }

    private void Update()
    {
        if (GameManager.FreezeGameMovement) return;
        
        if (delayCounter > 0)
        {
            delayCounter -= Time.deltaTime;
            return;
        }
        
        if (waypoints.Length == 0)
            return;
        
        if (transform.position == waypoints[_waypointsArrayIndex].transform.position)
        {
            _waypointsArrayIndex++;
            if (_waypointsArrayIndex == waypoints.Length)
                _waypointsArrayIndex = 0;
        }
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[_waypointsArrayIndex].transform.position, _enemySpeed * Time.deltaTime);
    }

    #endregion

    #region Methods

    public void SetSpeed(float speedValue)
    {
        _enemySpeed = speedValue;
    }

    public void ResetSpeed()
    {
        _enemySpeed = speed;
    }

    #endregion



}
