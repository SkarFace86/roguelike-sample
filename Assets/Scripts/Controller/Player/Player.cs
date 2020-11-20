using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public float moveSpeed;
    public float chargeSpeed;

    public int maxHealth = 100;
    public int maxCharge = 100;

    protected int currentHealth;
    protected float currentCharge;

    public HealthBar healthBar;
    public ChargeBar chargeBar;

    public GameObject abilitySelector;

    public Transform firePosition;
    public GameObject projectile;

    protected Vector2 input;

    protected Rigidbody2D rb;

    protected void MovePlayer()
    {
        rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public abstract void TakeDamage(int damage);
}
