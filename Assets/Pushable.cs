using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    public bool grabbed = false, prevFrameGrabbed = false;
    public bool pushed = false, prevFramePushed = false;
    public GameObject player;
    public Vector2 prevFramePlayerPos;
    public Rigidbody2D rb2d;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update(){
        if(grabbed){
            if(prevFrameGrabbed){
                Vector2 playerPosDelta =  (Vector2)player.transform.position - prevFramePlayerPos;
                transform.Translate(playerPosDelta, Space.Self);
            }
            prevFramePlayerPos = player.transform.position;
            prevFrameGrabbed = true;
        }else{
            prevFrameGrabbed = false;
        }

        if(pushed){
            if(prevFramePushed){
                Vector2 playerPosDelta =  (Vector2)player.transform.position - prevFramePlayerPos;
                transform.Translate(playerPosDelta, Space.Self);
            }
            prevFramePlayerPos = player.transform.position;
            prevFramePushed = true;
        }else{
            prevFramePushed = false;
        }

    }
}