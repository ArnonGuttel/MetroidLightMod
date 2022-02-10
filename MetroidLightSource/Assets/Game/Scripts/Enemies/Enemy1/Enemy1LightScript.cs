using System;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Enemy1LightScript : MonoBehaviour
{
    #region Inspector

    [SerializeField] float onDetectionSpeed; // Will set the enemy speed on player detection
    [SerializeField] private float timeTillCooldown;

    #endregion

    #region Fields

    private Light2D _enemyLight;
    private float _lightInitIntense;
    private Color _lightInitColor;
    private bool _startTimer;
    private float _timer;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _enemyLight = GetComponent<Light2D>();
        _lightInitColor = _enemyLight.color;
        _lightInitIntense = _enemyLight.intensity;
        _timer = timeTillCooldown;
    }

    private void OnEnable()
        // On enemy respawn we will reset it fields
    {
        _enemyLight.color = _lightInitColor;
        _enemyLight.intensity = _lightInitIntense;

        _startTimer = false;
        _timer = timeTillCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // On Player detection we will increase the enemy speed and set the light animation.
        if (other.CompareTag("Player"))
        {
            _startTimer = false;
            _timer = timeTillCooldown;
            GetComponentInParent<WaypointFollower>().SetSpeed(onDetectionSpeed);
            GetComponentInParent<Animator>().SetBool("PlayerDetected", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Once thr Player leaves the enemy radius we will start a timer till enemy cooldown
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

    #endregion
}