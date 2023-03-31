using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1f;
    [SerializeField] float deathDelay = 0.5f;

    Rigidbody2D enemyRb;
    Animator enemyAnim;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    void Update()
    {
        enemyRb.velocity = new Vector2(enemySpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        enemySpeed = -enemySpeed;
        ChangeEnemyLookDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyAnim.SetBool("isDeath", true);
            Destroy(gameObject, deathDelay);
        }    
    }

    void ChangeEnemyLookDirection()
    {
        transform.localScale = new Vector2(Mathf.Sign(enemyRb.velocity.x), 1f);
    }
}
