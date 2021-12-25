using System;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Enemy2Radius : MonoBehaviour
{
    #region Inspector

    public Enemy2Script script;
    [SerializeField] private GameObject enemyLightGameObject;
    [SerializeField] private float lightMinAngle;
    [SerializeField] private float lightMaxAngle;
    [SerializeField] private float lightChangeSpeed;

    [SerializeField] private float ColliderChangeSpeed;
    #endregion

    #region Fields

    // private float _circleRaduis;
    // private CircleCollider2D _circleCollider2D;
    private BoxCollider2D _boxCollider2D;
    private Vector2 _colliderInitSize;
    private Light2D _enemyLight;
    private Color _lightInitColor;
    private float _lightInitIntense;
    private float _lightInitDir;
    private int _lightChangeDirection = -1;


    #endregion

    #region MonoBehaviour
    
    private void Awake()
    {
        // _circleCollider2D = GetComponent<CircleCollider2D>();
        // _circleRaduis = _circleCollider2D.radius;
        _enemyLight = enemyLightGameObject.GetComponent<Light2D>();
        _lightInitDir = _enemyLight.pointLightOuterAngle;
        _lightInitColor = _enemyLight.color;
        _lightInitIntense = _enemyLight.intensity;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _colliderInitSize = _boxCollider2D.size;
    }

    private void Update()
    {
        changeLightAngle();
        changeBoxColliderSize();
    }

    private void OnEnable()
    {
        // _circleCollider2D.enabled = true;
        // _circleCollider2D.radius = _circleRaduis;
        _boxCollider2D.enabled = true;
        _boxCollider2D.size = _colliderInitSize;
        _lightChangeDirection = -1;
        _enemyLight.pointLightOuterAngle = _lightInitDir;
        _enemyLight.color = _lightInitColor;
        _enemyLight.intensity = _lightInitIntense;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            script.playerPosition = other.gameObject.transform.position;
            script.attackPlayer = true;
            gameObject.GetComponentInParent<Animator>().SetTrigger("EnemyActivate");
            _enemyLight.color = Color.red;
            _enemyLight.intensity = 1;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
        // reduce radius size
    {
        if (script.attackPlayer)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                script.playerPosition = other.gameObject.transform.position;
                // _circleCollider2D.radius -= 0.1f * Time.deltaTime;
            }
        }
    }

    #endregion

    private void changeLightAngle()
    {
        if (script.attackPlayer || script._explode) return;
        _enemyLight.pointLightInnerAngle += lightChangeSpeed * _lightChangeDirection * Time.deltaTime;
        _enemyLight.pointLightOuterAngle += lightChangeSpeed * _lightChangeDirection * Time.deltaTime;
        if (_enemyLight.pointLightInnerAngle <= lightMinAngle)
            _lightChangeDirection = 1;
        else if (_enemyLight.pointLightInnerAngle >= lightMaxAngle)
            _lightChangeDirection = -1;
    }

    private void changeBoxColliderSize()
    {
        if (script.attackPlayer || script._explode) return;
        var temp = _boxCollider2D.size;
        temp.x +=  lightChangeSpeed * _lightChangeDirection * Time.deltaTime * ColliderChangeSpeed;
        _boxCollider2D.size = temp;
    }
    
}