using System;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Enemy1LightScript : MonoBehaviour
{
    [SerializeField] float onDetectionSpeed;
    [SerializeField] private float timeTillCooldown;
    
    private Light2D _enemyLight;
    private float _lightInitIntense;
    private Color _lightInitColor;
    private bool _startTimer;
    private float _timer;
    


    private void Awake()
    {
        _enemyLight = GetComponent<Light2D>();
        _lightInitColor = _enemyLight.color;
        _lightInitIntense = _enemyLight.intensity;
        _timer = timeTillCooldown;
    }

    private void OnEnable()
    {
        _enemyLight.color = _lightInitColor;
        _enemyLight.intensity = _lightInitIntense;
        
        _startTimer = false;
        _timer = timeTillCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _startTimer = false;
            _timer = timeTillCooldown;
            GetComponentInParent<WaypointFollower>().SetSpeed(onDetectionSpeed);
            GetComponentInParent<Animator>().SetBool("PlayerDetected",true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _startTimer = true;
    }

    private void Update()
    {
        if (!_startTimer) return;
        if (_timer <= 0)
        {
            GetComponentInParent<WaypointFollower>().ResetSpeed();
            GetComponentInParent<Animator>().SetBool("PlayerDetected", false);
        }
        else
            _timer -= Time.deltaTime;
    }
}
