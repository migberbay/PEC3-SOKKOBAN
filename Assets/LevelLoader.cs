using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    public MapGeneration generator;
    public CheckForWin winCheck;
    string path = "Assets/Resources";
    List<int> levels = new List<int>();
    int current_level = 0, current_level_index = 0;
    public GameObject endOfLevelsCard;
    

    private void Awake() {
        foreach(string file in System.IO.Directory.GetFiles(path)){
            if(!file.Contains("json.meta")){
                int lvl = int.Parse(file.Split('-')[1].Split('.')[0]);
                levels.Add(lvl);
            }
        }
        // TODO: setup current level from level selector
        current_level = 3;
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
        // Destroy whatever dont destroy on load object we have let and load main menu scene.
        Debug.Log("Goes back to main menu fium.");
    }
}
