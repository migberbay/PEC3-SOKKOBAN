using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovementScript : MonoBehaviour
{
    public bool moving, pushing, pulling;
    public float orientation;// 0 = front, .5 = up, 1 = side
    public Animator anim;
    public SpriteRenderer lonk_render;
    public Rigidbody2D rb2d;
    public float moveSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        moving = pushing = pulling = false;

        float h_move = Input.GetAxis("Horizontal");
        float v_move = Input.GetAxis("Vertical");

        bool space_press = Input.GetKey(KeyCode.Space);
        bool shift_press = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        bool v_idle = false;
        bool h_idle = false;

        if(h_move != 0 || v_move != 0){
            moving  = true;
        }

        if (h_move > 0 ){ // right 
            orientation = 1f;
            lonk_render.flipX = false;
            rb2d.velocity = new Vector2(moveSpeed,0);
        }else if(h_move < 0){// left
            orientation = 1f;
            lonk_render.flipX = true;
            rb2d.velocity = new Vector2(-moveSpeed,0);
        }else{ 
            h_idle = true;
        }

        if(h_idle){ // priorize horizontal movement.
            if(v_move > 0){ // up
                orientation = .5f;
                rb2d.velocity = new Vector2(0,moveSpeed);
            }else if(v_move < 0){// down
                orientation = 0f;
                rb2d.velocity = new Vector2(0,-moveSpeed);
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

    public bool CheckIfTouchingBlock(){

        return true;
    }

    public void UpdateAnimator(){
        anim.SetBool("moving", moving);
        anim.SetBool("pushing", pushing);
        anim.SetBool("pulling", pulling);
        anim.SetFloat("looking_at", orientation);
    }

}
