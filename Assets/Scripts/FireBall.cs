using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 5f;
    public float damage = 1f;

    Animator anim;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ExplotionAnimation());
        Physics2D.IgnoreLayerCollision(6, 6);

    }

    private IEnumerator ExplotionAnimation()
    {
        yield return new WaitForSeconds(lifeTime);

        Explode();
    }

    private void Explode()
    {
        anim.SetBool("isExploting", true);
        speed = 0.5f;
        float scaleIncrease = 3.5f;
        transform.localScale *= scaleIncrease;
        
        Destroy(gameObject, 0.5f);  // Agregado un peque�o tiempo para asegurar que la animaci�n termine antes de destruir el objeto
    }

    private void Update()
    {
        
    }
    public void Shoot(Vector2 direction)
    {
        transform.right = direction; // Establece la direcci�n de la bala seg�n el vector de direcci�n
        GetComponent<Rigidbody2D>().velocity = direction * speed; // Aplica velocidad en la direcci�n deseada
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Aseg�rate de ajustar la etiqueta seg�n tus necesidades
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);

            Explode();
        }

        
    }
}
