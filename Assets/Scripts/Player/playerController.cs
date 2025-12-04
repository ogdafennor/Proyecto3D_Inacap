using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public int vidas = 10;
    public bool isDead = false;
    private float speed = 7f;
    private float dodgeForce = 5f;
    private float rotationSpeed = 120f;

    private Rigidbody playerRb;
    private Animator animator;
    private bool isOnGround;
    public bool isAttacking = false;
    private bool isDodging = false;
    private float attackCooldown = .75f;
    private float nextAttackTime = 0f;
    private float damageCooldown = .5f;
    private float nextDamageTime = 0f;
    private float dodgeCooldown = 1f;
    private float nextDodgeTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0)
        {
            float rotacion = horizontalInput * rotationSpeed * Time.deltaTime;
            playerRb.MoveRotation(playerRb.rotation * Quaternion.Euler(0, rotacion, 0));
        }

        Vector3 movimiento = transform.forward * verticalInput;
        playerRb.MovePosition(playerRb.position + movimiento * speed * Time.deltaTime);

        animator.SetFloat("movementSpeed", verticalInput);

        if (animator.GetFloat("movementSpeed") > 0.1f){
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // Correr
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 15f;
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 7f;
            animator.SetBool("isRunning", false);
        }

        // Rodar / Dodge
        if (Input.GetKey(KeyCode.Space) && isOnGround && !isDodging && (animator.GetFloat("movementSpeed") > 0.1f) && Time.time >= nextDodgeTime)
        {
            animator.SetTrigger("dodge");
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            nextDodgeTime = Time.time + dodgeCooldown;

            isDodging = true;

            playerRb.AddForce(playerRb.transform.forward * dodgeForce * Time.deltaTime, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.Space) && isDodging)
        {
            isDodging = false;
        }

        // Ataques
        if (Input.GetMouseButton(0) && !isAttacking && Time.time >= nextAttackTime)
        {
            StartCoroutine(AttackDuration(.5f));
            animator.SetTrigger("primaryAttack");
            nextAttackTime = Time.time + attackCooldown;
        }
        if (Input.GetMouseButton(1) && !isAttacking && Time.time >= nextAttackTime)
        {
            StartCoroutine(AttackDuration(.5f));
            animator.SetTrigger("secundaryAttack");
            nextAttackTime = Time.time + attackCooldown;
        }

    }

    IEnumerator AttackDuration(float duration)
    {
        isAttacking = true;
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Limites del mapa
        if (other.gameObject.CompareTag("Limits"))
        {
            transform.position = new Vector3(0, .5f, 0);
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // Arma del enemigo
        if (other.CompareTag("enemyWeapon"))
        {
            enemyController enemy = other.GetComponentInParent<enemyController>();
            if (enemy != null && enemy.isAttacking && Time.time >= nextDamageTime)
            {
                takeDamage(1);
                nextDamageTime = Time.time + damageCooldown;
            }
        }

        // Planta curativa
        if (other.CompareTag("healingPlant"))
        {
            if (vidas <= 8)
            {
                vidas += 2;
                Destroy(other.gameObject);
                GameManager.instance.addScore(50);
            }
        }
    }

    public void takeDamage(int damage)
    {
        if (isDead) return;

        vidas -= damage;
        if (vidas <= 0)
        {
            isDead = true;
            animator.SetTrigger("isDead");
            playerRb.velocity = Vector3.zero;

            FindObjectOfType<UI_Manager>().showRestartButton();
        }
    }
}
