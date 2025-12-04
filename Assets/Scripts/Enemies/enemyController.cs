using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public int vidas = 5;
    private bool isDead = false;
    private float speed = 4f;
    public bool isAttacking = false;
    private int rutina;
    private float cronometro;
    public float attackCooldown = 1f;
    public float nextAttackTime = 0f;
    private float damageCooldown = 0.1f;
    private float nextDamageTime = 0f;
    private Animator animator;
    private Quaternion rotacion;
    //private GameObject target;
    private playerController player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (player.isDead)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);

            animator.SetBool("walk", false);
            animator.SetBool("run", false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) > 20 || player.isDead)
        {
            // Movimiento aleatorio
            animator.SetBool("run", false);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    animator.SetBool("walk", false);
                    break;
                case 1:
                    int grado = Random.Range(0, 360);
                    rotacion = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacion, 0.5f);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    animator.SetBool("walk", true);
                    break;
            }
        }
        else
        {    
            if (Vector3.Distance(transform.position, player.transform.position) > 2 && !isAttacking)
            {
                // Seguir al jugador
                var lookPos = player.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                animator.SetBool("walk", false);
                animator.SetBool("run", true);
                transform.Translate(Vector3.forward * (speed * 2) * Time.deltaTime);
            }
            else
            {
                if (!isAttacking && Time.time >= nextAttackTime)
                {
                    // Atacar al jugador
                    animator.SetBool("walk", false);
                    animator.SetBool("run", false);
                    isAttacking = true;
                    animator.SetBool("isAttacking", true);
                    nextAttackTime = Time.time + attackCooldown;
                }
                else if (isAttacking && Time.time >= nextAttackTime)
                {
                    isAttacking = false;
                    animator.SetBool("isAttacking", false);
                }
            }        
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerWeapon"))
        {
            if (player != null && player.isAttacking && Time.time >= nextDamageTime)
            {
                takeDamage(1);
                nextDamageTime = Time.time + damageCooldown;
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
            StartCoroutine(die());
            GameManager.instance.addScore(100);
        }
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
