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

    [SerializeField] private float ColliderChangeSpeed; // This will used to match the collider with light change.
    [SerializeField] private float timeTillAttack;

    #endregion

    #region Fields

    private BoxCollider2D _boxCollider2D;
    private Vector2 _colliderInitSize;

    private Light2D _enemyLight;
    private Color _lightInitColor;
    private float _lightInitIntense;
    private float _lightInitOuterAngle;
    private float _lightInitInnerAngle;
    private int _lightChangeDirection = -1;

    private bool _startTimer;
    private float _timer;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _enemyLight = enemyLightGameObject.GetComponent<Light2D>();
        _lightInitOuterAngle = _enemyLight.pointLightOuterAngle;
        _lightInitInnerAngle = _enemyLight.pointLightInnerAngle;
        _lightInitColor = _enemyLight.color;
        _lightInitIntense = _enemyLight.intensity;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _colliderInitSize = _boxCollider2D.size;

        _timer = timeTillAttack;
    }

    private void Update()
    {
        changeLightAngle();
        changeBoxColliderSize();

        if (!_startTimer) return; // check if the detection timer started

        if (_timer <= 0)
            attackPlayer();
        else
            _timer -= Time.deltaTime;
    }

    private void OnEnable()
        // On enemy respawn we will reset it fields
    {
        _boxCollider2D.enabled = true;
        _boxCollider2D.size = _colliderInitSize;
        _enemyLight.pointLightOuterAngle = _lightInitOuterAngle;
        _enemyLight.pointLightInnerAngle = _lightInitInnerAngle;
        _lightChangeDirection = -1;
        _enemyLight.color = _lightInitColor;
        _enemyLight.intensity = _lightInitIntense;

        _timer = timeTillAttack;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            // We will update the attack target position to the Player one.
        {
            script.playerPosition = other.gameObject.transform.position;
            _startTimer = true;
            gameObject.GetComponentInParent<Animator>().SetBool("PlayerDetected", true);
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (script.attackPlayer)
            // We will update the attack target position to the Player one.
        {
            if (other.gameObject.CompareTag("Player"))
            {
                script.playerPosition = other.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            // We will reset the detection timer.
        {
            _startTimer = false;
            _timer = timeTillAttack;
            gameObject.GetComponentInParent<Animator>().SetBool("PlayerDetected", false);
            GetComponent<AudioSource>().Stop();
        }
    }

    #endregion

    #region Methods

    private void attackPlayer()
    {
        script.attackPlayer = true;
        gameObject.GetComponentInParent<Animator>().SetTrigger("AttackPlayer");
        gameObject.GetComponentInParent<Animator>().SetTrigger("EnemyActivate");
    }

    private void changeLightAngle()
        // This method will change the enemy detection light on every frame 
    {
        if (script.attackPlayer || script._explode || _startTimer) return;
        _enemyLight.pointLightInnerAngle += lightChangeSpeed * _lightChangeDirection * Time.deltaTime;
        _enemyLight.pointLightOuterAngle += lightChangeSpeed * _lightChangeDirection * Time.deltaTime;
        if (_enemyLight.pointLightInnerAngle <= lightMinAngle)
            _lightChangeDirection = 1;
        else if (_enemyLight.pointLightInnerAngle >= lightMaxAngle)
            _lightChangeDirection = -1;
    }

    private void changeBoxColliderSize()
        // This method will change the enemy Collider accordingly.
    {
        if (script.attackPlayer || script._explode || _startTimer) return;
        var temp = _boxCollider2D.size;
        temp.x += lightChangeSpeed * _lightChangeDirection * Time.deltaTime * ColliderChangeSpeed;
        _boxCollider2D.size = temp;
    }

    #endregion
}