using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovementScript : MonoBehaviour
{
    public bool moving, pushing, pulling;
    public float orientation;// 0 = front, .5 = up, 1 = side
    public Animator anim;
    
    void Start()
    {
        
    }

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
            orientation = 1;

        }else if(h_move < 0){// left
            orientation = 1;

        }else{ 
            h_idle = true;
        }

        if(h_idle){ // priorize horizontal movement.
            if(v_move > 0){ // up


            }else if(v_move < 0){// down


            }else{ 
                v_idle = true;

            }
        }

        if(h_idle & v_idle){
            
        }

        UpdateAnimator();

    }

    public bool CheckIfTouchingBlock(){

        return true;
    }

    public void UpdateAnimator(){

    }

}
