﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCcontroller: MonoBehaviour

{

    public static float deltaTime;
    public float movespeed;
    private Rigidbody2D myRigidbody;

    public bool isWalking;
    public float walkTime;
    public float waitTime;

    private float walkCounter;
    private float waitCounter;

    private int WalkDirection;
    private bool right;

    Animator a;


    // Use this for initialization
    void Start ()
    {

        a = GetComponent<Animator>();

        myRigidbody = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
        right = true; //Set true if the NPC is facing right at start of scene
	}
	
	// Update is called once per frame
	void Update () {
        if (isWalking)

        {
            walkCounter -= Time.deltaTime;
            
            switch (WalkDirection)
            {
                case 0: // Up
                    a.SetBool("Walking", true);
                    myRigidbody.velocity = new Vector2(0, movespeed);
                    
                    break;

                case 1: // Right
                    if (right == false)
                    {
                        Flip();
                    }
                    a.SetBool("Walking", true);
                    myRigidbody.velocity = new Vector2(movespeed, 0);

                    break;

                case 2:  // Down
                    a.SetBool("Walking", true);
                    myRigidbody.velocity = new Vector2(0, -movespeed);
                    
                    break;

                case 3: // Left                    
                    if (right == true)
                    {
                        Flip();
                    }
                    a.SetBool("Walking", true);
                    myRigidbody.velocity = new Vector2(-movespeed, 0);

                    break;
            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

        }

        else
        {
            a.SetBool("Walking", false);
            waitCounter -= Time.deltaTime;
            
            if (waitCounter < 0)
            {
                ChooseDirection();
            }
        }
	}

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        right = !right;
    }

    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

}