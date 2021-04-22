using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovementScript : MonoBehaviour
{
    public bool moving, pushing, pulling, space_pressed, shift_pressed;
    public bool topBox, leftBox, rightBox, downBox;
    public GameObject h_push_top, h_push_down, h_push_left, h_push_right;
    public float orientation;// 0 = front, .5 = up, 1 = side

    public Animator anim;
    public SpriteRenderer lonk_render;
    public Rigidbody2D rb2d;
    public float moveSpeed = 2f;
    public GameObject grabbedBox;
    public GameObject pushedBox;

    // Update is called once per frame
    void Update()
    {
        moving = pushing = pulling = false;

        float h_move = Input.GetAxis("Horizontal");
        float v_move = Input.GetAxis("Vertical");

        space_pressed = Input.GetKey(KeyCode.Space); // pull key
        shift_pressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); // push key

        bool v_idle = false;
        bool h_idle = false;

        // Check for box release.
        if(!space_pressed){
            if(grabbedBox != null){
                grabbedBox.GetComponent<Pushable>().grabbed = false;
                grabbedBox = null;
                moveSpeed *= 2;
                topBox = leftBox = rightBox = downBox = false;
            }
        }

        // Check for no push
        if(!shift_pressed){
            if(moveSpeed < 2){
                moveSpeed *= 2;
            }
            topBox = leftBox = rightBox = downBox = false;
            h_push_down.SetActive(false);
            h_push_top.SetActive(false);
            h_push_left.SetActive(false);
            h_push_right.SetActive(false);
        }

        // Move booleans for animation controller.
        if(h_move != 0 || v_move != 0){
            if(grabbedBox == null){
                moving  = true;
            }
        }

        if (h_move > 0 ){ // right 

            orientation = 1f;
            if(grabbedBox != null){
                if(rightBox){
                    pulling = true;
                    lonk_render.flipX = true;
                    rb2d.velocity = new Vector2(moveSpeed,0);
                }
            }else{
                lonk_render.flipX = false;
                rb2d.velocity = new Vector2(moveSpeed,0);
            }

        }else if(h_move < 0){// left

            orientation = 1f;
            if(grabbedBox != null){
                if(leftBox){
                    pulling = true;
                    lonk_render.flipX = false;
                    rb2d.velocity = new Vector2(-moveSpeed,0);
                }
            }else{
                lonk_render.flipX = true;
                rb2d.velocity = new Vector2(-moveSpeed,0);
            }

        }else{ 
            h_idle = true;
        }

        if(h_idle){ // priorize horizontal movement.
            if(v_move > 0){ // up

                if(grabbedBox != null){     
                    if(topBox){
                        pulling = true;
                        orientation = 0f;
                        rb2d.velocity = new Vector2(0,moveSpeed);
                    }
                }else{
                    orientation = .5f;
                    rb2d.velocity = new Vector2(0,moveSpeed);
                }

            }else if(v_move < 0){// down

                if(grabbedBox != null){
                    if(downBox){
                        pulling = true;
                        orientation = .5f;
                        rb2d.velocity = new Vector2(0,-moveSpeed);
                    }
                }else{
                    orientation = 0f;
                     rb2d.velocity = new Vector2(0,-moveSpeed);
                }

            }else{ 
                v_idle = true;
            }
        }

        if(h_idle & v_idle){
            rb2d.velocity = new Vector2(0,0);
            moving = false;
            pushing = false;
            pulling = false;
        }
        UpdateAnimator();
    }

    public void UpdateAnimator(){
        anim.SetBool("moving", moving);
        anim.SetBool("pushing", pushing);
        anim.SetBool("pulling", pulling);
        anim.SetFloat("looking_at", orientation);
    }

    private void OnCollisionStay2D(Collision2D col) {
        if(col.transform.tag == "Pushable"){

            if(space_pressed){
                col.transform.GetComponent<Pushable>().grabbed = true;
                grabbedBox = col.transform.gameObject;
                if(moveSpeed > 2){
                    moveSpeed /=2;
                }
                BoxPosCompute(col.GetContact(0).normal);
            }
            else if(shift_pressed){
                BoxPosCompute(col.GetContact(0).normal);
                if(moveSpeed > 2){
                    moveSpeed /=2;
                }

                if(rightBox){
                    h_push_right.SetActive(true);
                }else if(leftBox){
                    h_push_left.SetActive(true);
                }else if(topBox){
                    h_push_top.SetActive(true);
                }else if(downBox){
                    h_push_down.SetActive(true);
                }

            }
        }
    }

    public void BoxPosCompute(Vector2 normal_to_box){

        if(normal_to_box == new Vector2(1,0)){
            rightBox = true; 
        }else if(normal_to_box == new Vector2(-1,0)){
            leftBox = true;
        }else if(normal_to_box == new Vector2(0,1)){
            topBox = true;
        }else if(normal_to_box == new Vector2(0,-1)){
            downBox = true;
        }
    }
}
