using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Player
{
    public int rollCost = 35;
    public float rollDistance = 10.0f;
    private Animator anim;

    Vector3 mousePos;
    Vector3 movement;

    private State state;
    private enum State
    {
        PLAYING,
        ATTACKING,
        ROLLING,
        ABILITY
    }

    private void Start()
    {
        state = State.PLAYING;
        healthBar = GameObject.Find("Canvas").GetComponent<HealthBar>();
        chargeBar = GameObject.Find("Canvas").GetComponent<ChargeBar>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);

        currentCharge = 100;
        chargeBar.SetMaxCharge(currentCharge);
        chargeBar.SetCharge(currentCharge);

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        HandleAbilitySelector();

        switch (state)
        {
            case State.PLAYING:
                HandleBasicAttack();
                HandleRoll();
                break;
            case State.ATTACKING:
                break;
        }

        if (currentHealth == 0) Die();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.PLAYING:
                MovePlayer();
                Recharge();
                break;
            case State.ROLLING:
                Roll();
                break;

        }
    }

    private void ChargeAttack()
    {
        currentCharge += (chargeSpeed * (Time.deltaTime));
        if (currentCharge > maxCharge) currentCharge = maxCharge;
        chargeBar.SetCharge(currentCharge);
    }

    private void Recharge()
    {
        currentCharge += (chargeSpeed * (Time.deltaTime));
        if (currentCharge >= 100) currentCharge = 100;
        chargeBar.SetCharge(currentCharge);
    }

    private void HandleBasicAttack()
    {
        if (Input.GetButtonDown("Fire1")) StartCoroutine(BasicAttack());
    }

    private IEnumerator BasicAttack()
    {
        Instantiate(projectile, firePosition.position, firePosition.rotation);
        state = State.ATTACKING;

        yield return new WaitForSeconds(0.3f);

        state = State.PLAYING;
        yield return null;
    }

    private void HandleAbilitySelector()
    {
        if (Input.GetButton("AbilitySelector"))
            abilitySelector.SetActive(true);
        else
            abilitySelector.SetActive(false);
    }
    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;

        healthBar.SetHealth(currentHealth);
    }

    private void HandleRoll()
    {
        if (currentCharge >= rollCost && Input.GetButtonDown("Special"))
        {
            state = State.ROLLING;
        }
    }

    private void Roll()
    {
        // TODO:
        // if Input == Vector2.zero move towards mouse


        currentCharge -= rollCost;
        chargeBar.SetCharge(currentCharge);
        rb.MovePosition(rb.position + input * rollDistance);
        state = State.PLAYING;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
