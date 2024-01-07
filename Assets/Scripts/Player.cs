using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float h;
    bool isGrounded = false;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;

    private Rigidbody2D rb;

    Animator anim;
    SpriteRenderer spriteRenderer;


    [SerializeField] FireBall bulletPrefab;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); //hace la referencia al componente Animator
        spriteRenderer = GetComponent<SpriteRenderer>(); //hace la referencia al componente SpriteRenderer
    }
    
    void Shoot()
    {
        Vector2 shootDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        float spawnOffset = spriteRenderer.flipX ? -1f : 1f;
        FireBall bulletClone = Instantiate(bulletPrefab, rb.position + new Vector2(spawnOffset, 0f), Quaternion.identity);
        bulletClone.Shoot(shootDirection);
    }
    void Animating()
    {
        if (h != 0)
        {
            anim.SetBool("isRunning", true);
        } else {
            anim.SetBool("isRunning", false);
        }
    }

    void FlipSprite()
    {
        if (h > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (h < 0)
        {
            spriteRenderer.flipX = true;
        }
    }


    private void ReadInput()
    {
        h = Input.GetAxis("Horizontal");
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private IEnumerator ShootingAnimation()
    {
        
        anim.SetBool("isShooting", true);
        anim.SetTrigger("ThrowBullet");

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isShooting", false);

        Shoot();
    }

    private void Update()
    {
        ReadInput();
        Animating();
        FlipSprite();
        rb.velocity = new Vector2(h * speed, rb.velocity.y);

        if (Input.GetButtonDown("Vertical") && isGrounded)
        {
            Jump();
        }
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(ShootingAnimation());
            

        }
    }

    
}
