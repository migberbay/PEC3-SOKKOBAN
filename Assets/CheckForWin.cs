using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckForWin : MonoBehaviour
{
    public GameObject[] allPortals;
    public bool allSet = false;
    public int setCount = 0;
    public TMP_Text win;

    void Start()
    {
        StartCoroutine(CheckAllSet());
    }

    public IEnumerator CheckAllSet(){
        yield return new WaitForSeconds(2.5f);
        if(allPortals.Length == 0){
            allPortals = GameObject.FindGameObjectsWithTag("Portal");
        }

        setCount = 0;
        foreach(var p in allPortals){
            PortalLogic pl = p.GetComponent<PortalLogic>();
            if(pl.set){
                setCount += 1;
            }
        }

        if(setCount == allPortals.Length){
            win.text = "You win.";
        }else{
            StartCoroutine(CheckAllSet());
        }

    }
    
}
