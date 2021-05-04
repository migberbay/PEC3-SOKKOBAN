using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MenuController : MonoBehaviour
{
    public int levelToLoad = 0, widthOnChange = 0, heightOnChange = 0;
    string base_path = "Assets/Resources";
    public GameObject levelSelectBoxPrefab;
    public GameObject itemPanel;
    public TMP_Text widthValueText, heightValueText;
    public Slider widthSlider, heightSlider;
    public InputField lvlInput;


    private void Start() {
        DontDestroyOnLoad(this.gameObject);
        lvlInput.characterValidation = InputField.CharacterValidation.Integer;

        foreach(string file in System.IO.Directory.GetFiles(base_path)){
            if(!file.Contains("json.meta")){
                int lvl = int.Parse(file.Split('-')[1].Split('.')[0]);
                var instance = Instantiate(levelSelectBoxPrefab, itemPanel.transform);
                var b = instance.GetComponent<Button>();
                b.onClick.AddListener(delegate{OnClickLevel(lvl);});
                b.GetComponentInChildren<TMP_Text>().text = "Level-"+lvl.ToString();
            }
        }
    }

    public void OnClickLevel(int level){
        levelToLoad = level;
        Debug.Log("loading level: "+ levelToLoad);
        SceneManager.LoadScene("LevelPlay");
    }

    public void updateValueText(string t){
        if(t == "w"){
            widthValueText.text = widthSlider.value.ToString();
        }else if(t == "h"){
            heightValueText.text = heightSlider.value.ToString();
        }
    }

    public void OnClickCreate(){
        widthOnChange = (int)widthSlider.value;
        heightOnChange = (int)heightSlider.value;
        levelToLoad = int.Parse(lvlInput.text);
        SceneManager.LoadScene("LevelCreator");
    }
}
