using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    public bool grabbed = false, prevFrameGrabbed = false;
    public bool pushed = false, prevFramePushed = false;
    public GameObject[] players;
    public Vector2 prevFramePlayerPos;
    public Rigidbody2D rb2d;
    public GameObject player;

    public void Start(){
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void Update(){
        if(grabbed){
            player = GetClosestPlayer();
            if(prevFrameGrabbed){
                Vector2 playerPosDelta = (Vector2)player.transform.position - prevFramePlayerPos;
                transform.Translate(playerPosDelta, Space.Self);
            }
            prevFramePlayerPos = player.transform.position;
            prevFrameGrabbed = true;
        }else{
            prevFrameGrabbed = false;
        }

        if(pushed){
            player = GetClosestPlayer();
            if(prevFramePushed){
                Vector2 playerPosDelta =  (Vector2)player.transform.position - prevFramePlayerPos;
                transform.Translate(playerPosDelta, Space.Self);
            }
            prevFramePlayerPos = player.transform.position;
            prevFramePushed = true;
        }else{
            prevFramePushed = false;
        }

        if(player != null){
            var ms = player.GetComponent<MovementScript>();
            if(ms.grabbedBox == null && ms.pushedBox == null){
                grabbed = pushed = prevFrameGrabbed = prevFramePushed = false;
                GetComponent<BoxCollider2D>().enabled = true;
                player = null;
            }
        }
        

    }

    public GameObject GetClosestPlayer(){
        float minDist = float.MaxValue;
        GameObject res = null;

        foreach(var p in players){
            if(p == null)
                continue;
                
            var d = Vector2.Distance(transform.position, p.transform.position);
            if(d < minDist){
                res = p;
                minDist = d;
            }
        }
        return res;
    }
}