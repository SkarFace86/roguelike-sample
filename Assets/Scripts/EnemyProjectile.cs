using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    public int projectileDamage = 10;

    public Vector3 targetPos;

    private void Start()
    {
        type = Type.ENEMY;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Fire();
    }

    public void Fire()
    {

        Debug.Log(targetPos);
        //if (rb.velocity == Vector2.zero)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
