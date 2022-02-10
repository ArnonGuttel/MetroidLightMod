using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    #region Inspector
    
    [SerializeField] private GameObject lightGameObject;
    [SerializeField] private float lightGain;
    [SerializeField] private float lightLost;
    [SerializeField] private float MaxLight;

    #endregion

    #region fields

    private Light2D _light2D;
    private float _initOuterRadius;
    private float _currentRadius;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        GameManager.EnemyKilled += AddLight;
    }

    private void OnDestroy()
    {
        GameManager.EnemyKilled -= AddLight;
    }

    private void Start()
    {
        _light2D = lightGameObject.GetComponent<Light2D>();
        _initOuterRadius = _light2D.pointLightOuterRadius;
        _currentRadius = _initOuterRadius;
    }

    private void Update()
    {
        if (!(_currentRadius > _initOuterRadius)) return;
        _light2D.pointLightInnerRadius -= lightLost * Time.deltaTime;
        _light2D.pointLightOuterRadius -= lightLost * Time.deltaTime;
        _currentRadius = _light2D.pointLightOuterRadius;
    }
    
    #endregion

    #region Methods
    
    private void AddLight()
    {   if (_currentRadius >= MaxLight) return;
        _light2D.pointLightInnerRadius += lightGain;
        _light2D.pointLightOuterRadius += lightGain;
        _currentRadius = _light2D.pointLightOuterRadius;
    }

    #endregion
}
