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

    #endregion

    #region Fields
    
    private Vector3 _initPosition;
    private bool _attackPlayer;
    private bool _canAttack = true;

    #endregion

    #region MonoBehaviour
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _canAttack)
        {
            AttackPlayer();
        }
    }
    
    #endregion

    #region Methods

    public void AttackPlayer()
    {
        _canAttack = false;
        StartCoroutine(InstantiateBullet());
    }

    IEnumerator InstantiateBullet()
    {
        for (int i=0 ; i<bulletsNumber; i++)
        {
            GameObject bullet = Instantiate(EnemyBullet);
            Vector3 temp = transform.position;
            bullet.transform.position = temp;
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = bulletDirection * bulletSpeed;
            Destroy(bullet, bulletDestroyTime);
            GetComponent<Animator>().SetTrigger("EnemyFire");
            yield return new WaitForSeconds(bulletInstantiateDelay);
        }
        _canAttack = true;
    }
    
    #endregion
    



}


