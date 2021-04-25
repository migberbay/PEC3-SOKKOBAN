using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    public GameObject[] blocks;
    public bool set = false;
    public GameObject set_block = null;
    

    private void Start() {
        StartCoroutine(CheckForBlockOnTop());
    }

    public IEnumerator CheckForBlockOnTop(){
        yield return new WaitForSeconds(2.5f);
        if(blocks.Length == 0){
            blocks = GameObject.FindGameObjectsWithTag("Pushable");
        }

        if(!set){
            foreach(var b in blocks){
                Pushable p = b.GetComponent<Pushable>();
                if(!p.grabbed && !p.pushed && Vector2.Distance(b.transform.position, this.transform.position)< 0.25f){
                    b.transform.position = this.transform.position;
                    set_block = b;
                    set = true;   
                }
            }
        }
        else if(Vector2.Distance(set_block.transform.position, this.transform.position) > 0.25f){
                set = false;
                set_block = null;     
            }
        StartCoroutine(CheckForBlockOnTop());
    }
}