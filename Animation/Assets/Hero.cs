using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 15f;
    private bool isGrouded = false;
    float horizontalMove = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;  


    private void FixedUpdate()
    {
        CheckGround();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        sprite = rb.GetComponentInChildren<SpriteRenderer>();
    }

    private void Run()
    {
        if (isGrouded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }
    private void Jump()
    {
       
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        anim.SetFloat("Spead", Mathf.Abs(horizontalMove));


        if (Input.GetButton("Horizontal"))
            Run();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            anim.SetBool("IsJumping", true); 
        }
    }
    private void CheckGround()
    {
        if (!isGrouded) State = States.jump;

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrouded = collider.Length > 1;
    }
    public enum States
    {
         idle,
         run,
         jump 
    }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state",(int)value); }
    }

    public  void OnLanding()
    {
        anim.SetBool("IsJumping", false);
    }
}
