using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float agroRange;
    public int attackDamage;

    private GameObject target;

    public Vector3 attackOffset;
    public float attackRange = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        Attack();
        if(target != null && Vector3.Distance(gameObject.transform.position, target.transform.position) < agroRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange);
        if (colInfo != null && colInfo.gameObject.tag.Equals("Player"))
        {
            colInfo.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }
}
