using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : Player
{
    private float blockMoveSpeed;
    [SerializeField]
    private int initialShieldCost = 45;
    private Animator anim;
    float blockEnergyDrain = 20.0f;
    public GameObject shield;

    private State state;

    private enum State
    {
        PLAYING,
        ATTACKING,
        CHARGING,
        BLOCKING,
        ABILITY,
        WAIT
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

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        shield.SetActive(false);
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        HandleAbilitySelector();

        switch (state)
        {
            case State.PLAYING:
                HandleBlocking();
                break;
            case State.CHARGING:
                HandleBasicAttack();
                ChargeAttack();
                break;
            case State.BLOCKING:
                StopBlocking();
                break;

        }

        HandleAbilitySelector();

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
            case State.CHARGING:
                MovePlayer();
                break;
            case State.ATTACKING:
                StartCoroutine(BasicAttack());
                break;
            case State.BLOCKING:
                HandleShieldPosition();
                MovePlayer();
                Block();
                break;
        }
    }

    private void HandleBasicAttack()
    {
        if (Input.GetButtonDown("Fire1")) state = State.ATTACKING;
    }
    private void ChargeAttack()
    {
        currentCharge += (chargeSpeed * (Time.deltaTime));
        if (currentCharge > maxCharge) currentCharge = maxCharge;
        chargeBar.SetCharge(currentCharge);
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

    private IEnumerator BasicAttack()
    {
        // stop moving for 0.3 seconds
        moveSpeed = 0;
        Instantiate(projectile, firePosition.position, firePosition.rotation);

        state = State.WAIT;

        yield return new WaitForSeconds(0.3f);

        state = State.PLAYING;

        //set charge to 0 and move speed to normal
        currentCharge = 0;
        chargeBar.SetCharge(currentCharge);
        moveSpeed = blockMoveSpeed * 3;
    }

    private void Recharge()
    {
        currentCharge += (chargeSpeed * (Time.deltaTime));
        if (currentCharge >= 100) currentCharge = 100;
        chargeBar.SetCharge(currentCharge);
    }

    private void HandleBlocking()
    {
        if (currentCharge >= 46 && Input.GetButtonDown("Special"))
        {
            blockMoveSpeed = moveSpeed / 3;
            moveSpeed = blockMoveSpeed;
            currentCharge -= initialShieldCost;
            chargeBar.SetCharge(currentCharge);
            if (Input.GetButton("Special")) state = State.BLOCKING;
        }
    }

    private void Block()
    {
        shield.SetActive(true);
        currentCharge -= blockEnergyDrain * Time.deltaTime;
        chargeBar.SetCharge(currentCharge);
    }

    private void StopBlocking()
    {
        if (Input.GetButtonUp("Special") || currentCharge <= 0)
        {
            moveSpeed *= 3;
            shield.SetActive(false);
            state = State.PLAYING;
        }
    }

    private void HandleShieldPosition()
    {
        shield.transform.position = firePosition.transform.position;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
