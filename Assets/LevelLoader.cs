using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public MapGeneration generator;
    public CheckForWin winCheck;
    string path = "Assets/Resources";
    List<int> levels = new List<int>();
    int current_level = 0, current_level_index = 0;
    public GameObject endOfLevelsCard;
    public MenuController controller;
    public int resetKeyDownForFixedUpdates = 0, quitKeyDownForFixedUpdates = 0;
    bool hasReset = false;
    

    private void Awake() {
        foreach(string file in System.IO.Directory.GetFiles(path)){
            if(!file.Contains("json.meta")){
                int lvl = int.Parse(file.Split('-')[1].Split('.')[0]);
                levels.Add(lvl);
            }
        }
        // TODO: setup current level from level selector
        controller = GameObject.FindObjectOfType<MenuController>();
        current_level = controller.levelToLoad;
        Destroy(controller.gameObject);
        current_level_index = levels.IndexOf(current_level);
    }

    void Start()
    {
        generator = GameObject.FindObjectOfType<MapGeneration>().GetComponent<MapGeneration>();
        winCheck = GameObject.FindObjectOfType<CheckForWin>().GetComponent<CheckForWin>();
        Load();
    }

    public void LoadNext(){
        current_level_index++;
        try
        {
            current_level = levels[current_level_index];
            Load();
        }
        catch (System.Exception)
        {
            foreach(var player in GameObject.FindGameObjectsWithTag("Player")){
                player.GetComponent<MovementScript>().enabled = false;
            }
            endOfLevelsCard.SetActive(true);
        }
    }

    private void Load(){
        generator.stage = current_level;
        generator.LoadMap();
        StartCoroutine(winCheck.CheckAllSet());
    }

    public void ToMainMenu(){
        SceneManager.LoadScene("Menu");
    }

    private void FixedUpdate() {
        if(Input.GetKey(KeyCode.R)){
            resetKeyDownForFixedUpdates ++;
            if(resetKeyDownForFixedUpdates > 100 & !hasReset){
                current_level_index --;
                LoadNext();
                hasReset = true;
            }
        }else{
            resetKeyDownForFixedUpdates = 0;
            hasReset = false;
        }

        if(Input.GetKey(KeyCode.Q)){
            quitKeyDownForFixedUpdates ++;
            if(quitKeyDownForFixedUpdates > 100){
                SceneManager.LoadScene("Menu");
            }
        }else{
            quitKeyDownForFixedUpdates = 0;
        }
    }
}
