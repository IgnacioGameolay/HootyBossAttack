using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 5f;

    Animator anim;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ExplotionAnimation());

        
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

        Destroy(gameObject, 0.5f);  // Agregado un pequeño tiempo para asegurar que la animación termine antes de destruir el objeto
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))  // Asegúrate de ajustar la etiqueta según tus necesidades
        {
            Debug.LogWarning("Collision with enemy");
            Explode();
        }
    }
}
