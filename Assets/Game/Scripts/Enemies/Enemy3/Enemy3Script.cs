using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy3Script : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject EnemyBullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDestroyTime;
    [SerializeField] private int bulletsNumber;
    [SerializeField] private float bulletInstantiateDelay;
    [SerializeField] private Vector2 bulletDirection;
    [SerializeField] private bool autoFire; // enemy auto fire or fire only on Player detection
    [SerializeField] private float autoFireDelay;

    #endregion

    #region Fields

    private Vector3 _initPosition;
    private bool _attackPlayer;
    private bool _canAttack = true;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        GameManager.OpenHiddenDoor += StartAutoFire;
    }

    private void OnDestroy()
    {
        GameManager.OpenHiddenDoor -= StartAutoFire;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (autoFire) return;

        if (other.CompareTag("Player") && _canAttack)
        {
            AttackPlayer();
        }
    }

    #endregion

    #region Methods

    private void AttackPlayer()
        // Once the Player entered the Enemy collider,the enemy will fire a bullet every "bulletInstantiateDelay".
    {
        _canAttack = false;
        StartCoroutine(InstantiateBullet());
    }

    IEnumerator InstantiateBullet()
    {
        for (int i = 0; i < bulletsNumber; i++)
        {
            GameObject bullet = Instantiate(EnemyBullet);

            Vector3 temp = transform.position; // set the initial Bullet position
            bullet.transform.position = temp;

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>(); // set the velocity
            bulletRb.velocity = bulletDirection * bulletSpeed;

            GetComponent<Animator>().SetTrigger("EnemyFire"); // active the animation and sound
            if (!autoFire)
                GetComponent<AudioSource>().Play();

            Destroy(bullet, bulletDestroyTime);

            yield return new WaitForSeconds(bulletInstantiateDelay);
        }

        _canAttack = true;
    }

    IEnumerator InstantiateAutoBullet()
    {
        while (true)
        {
            AttackPlayer();
            yield return new WaitForSeconds(autoFireDelay);
        }
    }

    private void StartAutoFire()
    {
        if (autoFire)
            StartCoroutine(InstantiateAutoBullet());
    }

    #endregion
}