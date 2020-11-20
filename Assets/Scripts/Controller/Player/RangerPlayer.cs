using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Macros;

public class RangerPlayer : Player
{
    private float chargeMoveSpeed;
    private Animator anim;

    private State state;

    private enum State
    {
        PLAYING,
        ATTACKING,
        CHARGING,
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

        currentCharge = 0;
        chargeBar.SetMaxCharge(100);
        chargeBar.SetCharge(0);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        HandleAbilitySelector();

        switch (state)
        {
            case State.PLAYING:
                HandleChargeAttack();
                break;
            case State.CHARGING:
                HandleBasicAttack();
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
                break;
            case State.CHARGING:
                MovePlayer();
                ChargeAttack();
                //HandleChargeAnimation();
                break;
            case State.ATTACKING:
                StartCoroutine(BasicAttack());
                break;
        }
    }

    //public void MovePlayer()
    //{
    //    rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    //}

    private void HandleChargeAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            state = State.CHARGING;
            chargeMoveSpeed = moveSpeed / 3;
            moveSpeed = chargeMoveSpeed;
        }
    }
    private void HandleBasicAttack()
    {
        if (Input.GetButtonUp("Fire1")) state = State.ATTACKING;
    }
    private void ChargeAttack()
    {
        currentCharge += (chargeSpeed * (Time.deltaTime));
        if (currentCharge >= maxCharge) currentCharge = maxCharge;
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
        //anim.SetFloat("Up Speed", 0);
        //anim.SetFloat("Down Speed", 0);
        //anim.SetFloat("Right Speed", 0);
        //anim.SetFloat("Left Speed", 0);
        ////play attack animation
        //anim.SetBool("Attack", true);


        // stop moving for 0.3 seconds
        moveSpeed = 0;
        Instantiate(projectile, firePosition.position, firePosition.rotation);
        state = State.WAIT;

        yield return new WaitForSeconds(0.3f);
        state = State.PLAYING;

        //anim.SetBool("Attack", false);


        //set charge to 0 and move speed to normal
        currentCharge = 0;
        chargeBar.SetCharge(currentCharge);
        moveSpeed = chargeMoveSpeed * 3;
    }

    private void HandleChargeAnimation()
    {
        Vector3 mousePos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (angle >= 45.0f && angle < 135.0f)
            anim.Play("Charge_Up");
        else if (angle >= 135.0f || angle < -135.0f)
            anim.Play("Charge_Left");
        else if (angle >= -135.0f && angle < -45.0f)
            anim.Play("Charge_Down");
        else
            anim.Play("Charge_Right");

    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
