using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float moveSpeed = 5f;
    Rigidbody2D rb;
    Vector2 velocity;

    bool facingRight = true;

    public Animator animator;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");


        if (velocity.magnitude > 0)
            facingRight = true;
        if (velocity.magnitude < 0)
            facingRight = false;

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, facingRight ? 0f : 180f, 0f));

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
