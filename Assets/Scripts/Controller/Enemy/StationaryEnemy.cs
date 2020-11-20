using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : MonoBehaviour
{
    public float agroRange = 11f;
    public float bulletSpeed = 12f;
    public int attackDamage = 10;
    public float attackRange = 9f;

    public int maxHealth = 100;

    private int currentHealth;

    [SerializeField]
    HealthBar healthBar;

    bool canShoot = true;

    [SerializeField]
    GameObject circle;
    [SerializeField]
    GameObject bulletSpawn;

    public GameObject bullet;

    private Animator anim;
    private Player target;
    Transform targetPos;

    public Vector3 attackOffset;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) <= agroRange)
        {
            Vector3 difference = target.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            circle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
            
        if (target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) <= agroRange && canShoot)
        {
            targetPos = target.gameObject.transform;
            canShoot = false;
            StartCoroutine(FireBullet());
        }

        if (currentHealth == 0) Die();
    }

    public IEnumerator FireBullet()
    {
        Debug.Log(target.gameObject.transform.position + "  -  " + targetPos.position);
        //float angle = Mathf.Atan2(targetPos.position.y, targetPos.position.x) * Mathf.Rad2Deg;
        GameObject ob = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity);
        ob.GetComponent<Projectile>().type = Projectile.Type.ENEMY;
        ob.GetComponent<Projectile>().target = new Vector3(targetPos.position.x, targetPos.position.y, 0);
        yield return new WaitForSeconds(1.0f);
        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;

        healthBar.SetHealth(currentHealth);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
