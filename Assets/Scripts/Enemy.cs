using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 10f;
    [SerializeField] float deathCountdown = 5f;

    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpCooldown = 2f; // Nuevo: Tiempo mínimo entre saltos


    [SerializeField] float deathShakeIntensity = 0.1f;


    private Transform player;
    private Rigidbody2D rb;
    private Vector3 initialScale;
    private Animator anim;
    private bool canJump = true; // Nuevo: Permite saltar solo si es true
    private bool isGrounded = false;

    private void Start()
    {
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;

        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            // Calcular la dirección hacia el jugador
            Vector2 direction = player.position - transform.position;

            // Mover al enemigo hacia el jugador
            rb.velocity = new Vector2(direction.normalized.x * movementSpeed, rb.velocity.y);

            // Girar al enemigo según la dirección del jugador
            transform.localScale = new Vector3(Mathf.Sign(direction.x) * initialScale.x, initialScale.y, initialScale.z);

            // Decidir aleatoriamente si saltar (con cooldown)
            if (canJump && isGrounded)
            {
                Jump();
            }

            yield return null;
        }
    }

    private void Jump()
    {
        bool shouldJump = Random.Range(0, 10) == 0; // Ajusta el valor para cambiar la probabilidad de salto

        if (shouldJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            StartCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private IEnumerator DeathShake()
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while ((elapsed += Time.deltaTime) < deathCountdown)
        {
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * deathShakeIntensity;
            yield return null;
        }

        transform.position = originalPosition;
    }

    void Death()
    {
        StopAllCoroutines();
        StartCoroutine(DeathShake());
        Destroy(gameObject, deathCountdown);
        anim.SetTrigger("isDefeated");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
