using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 10f;
    [SerializeField] float deathCountdown = 5f;
    SpriteRenderer spriteRenderer;
    Animator anim;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Death()
    {
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


}
