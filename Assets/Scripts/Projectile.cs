using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 13f;
    public float projectileDistance = 15f;
    public int projectileDamage = 10;
    Vector3 moveDirection;
    float angle;

    public Vector3 target;

    public Type type;

    Rigidbody2D rb;

    public enum Type { 
        FRIENDLY,
        ENEMY
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (type == Type.FRIENDLY)
        {
            moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        }
        else
        {
            moveDirection = target - transform.position;
        }
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        moveDirection.z = 0;
        moveDirection.Normalize();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        target = transform.position + moveDirection * projectileDistance;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
        //transform.position = transform.position + moveDirection * projectileSpeed * Time.deltaTime;
        if (transform.position == target) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StationaryEnemy enemy = collision.gameObject.GetComponent<StationaryEnemy>();
        if(enemy != null && type == Type.FRIENDLY)
        {
            enemy.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
