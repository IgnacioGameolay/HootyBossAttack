using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Vector2 movementSpeed;

    private Vector2 offset;

    private Material material;

    private Rigidbody2D playerRB;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        offset = (playerRB.velocity.x * 0.1f) * movementSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
