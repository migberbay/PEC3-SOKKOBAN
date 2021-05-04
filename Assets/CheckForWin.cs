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
    public LevelLoader loader;

    private void Start(){
        loader = GameObject.FindObjectOfType<LevelLoader>().GetComponent<LevelLoader>();
    }

    public IEnumerator CheckAllSet(){
        yield return new WaitForSeconds(2.5f);
        allPortals = GameObject.FindGameObjectsWithTag("Portal");
        
        setCount = 0;
        foreach(var p in allPortals){
            PortalLogic pl = p.GetComponent<PortalLogic>();
            if(pl.set){
                setCount += 1;
            }
        }

        if(setCount == allPortals.Length){
            loader.LoadNext();
        }else{
            StartCoroutine(CheckAllSet());
        }

    }
    
}
