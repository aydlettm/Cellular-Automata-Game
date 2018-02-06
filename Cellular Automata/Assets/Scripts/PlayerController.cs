using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpHeight;

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);

        if (Input.GetKey(KeyCode.D))
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);

        if (Input.GetKey(KeyCode.A))
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);

        anim.SetFloat("Speed", GetComponent<Rigidbody2D>().velocity.x);
        anim.SetFloat("SpeedUD", GetComponent<Rigidbody2D>().velocity.y);

        //if (GetComponent<Rigidbody2D>().velocity.x > 0)
        //{
        //    transform.Rotate(0f, 0f, 0f);
        //}
        //else if (GetComponent<Rigidbody2D>().velocity.x < 0)
        //{ 
        //    transform.Rotate(0f, 180f, 0f);
        //}
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }
}
