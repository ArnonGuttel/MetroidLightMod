using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy3Script : MonoBehaviour
{
    #region Inspector
    
    public GameObject EnregyBall;
    [SerializeField] private float hitDelay; // for how long to freeze enemy movement after bullet hit
    [SerializeField] private int hitsTillDestroy;
    [SerializeField] private float dropRate; // drop rate for energy ball
    [SerializeField] private Vector2 direction;
    [SerializeField] private float speed;

    #endregion

    #region Fields

    private Rigidbody2D _rb;
    private Vector3 _initPosition;
    private int _hitCounter;
    private bool _attackPlayer;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _initPosition = gameObject.transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.position = _initPosition;
        _hitCounter = 0;
        gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            gameObject.GetComponent<AudioSource>().Play(0);
            _hitCounter++;
            if (_hitCounter == hitsTillDestroy)
                EnemyDead();
            gameObject.GetComponent<Animator>().SetTrigger("EnemyHit");
            GetComponent<WaypointFollower>().delayCounter = hitDelay;
        }
    }
    
    #endregion

    #region Methods

    public void AttackPlayer()
    {
        float velX = direction.x * speed;
        float velY = direction.y * speed;
        _rb.velocity = new Vector2(velX, velY);
        _attackPlayer = true;
    }
    private void EnemyDead()
    // check for energy ball instantiation, play "EnemyDead" animation, and update GameManager DeadEnemies. 
    {
        if (Random.Range(0f, 1f) <= dropRate)
        {
            var transform1 = transform;
            Instantiate(EnregyBall, transform1.position, transform1.rotation);
        }
        gameObject.GetComponent<Animator>().SetTrigger("EnemyDead");
        gameObject.GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(DeActiveEnemy),GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        
        transform.GetChild(0).gameObject.SetActive(false);
    }
    
    private void DeActiveEnemy()
    {
        GameManager.addToDeadEnemies(gameObject);
        GameManager.InvokeEnemyKilled();
        gameObject.SetActive(false);
    }

    #endregion
    



}


