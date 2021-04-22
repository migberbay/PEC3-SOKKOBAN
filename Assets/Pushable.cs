using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    public bool grabbed = false;
    public GameObject player;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update(){
        if(grabbed && transform.parent == null){
            transform.parent = player.transform;
        }
    }
}