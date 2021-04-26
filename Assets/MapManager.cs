using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGen;
using UnityEngine.UI;
using System.IO;



public class MapManager : MonoBehaviour
{
    public int cols, rows, levelNumber, selectedElement = 1;
    bool newmap = true;

    public Sprite[] placeables;
    public int[] representation = new int[] {-1,0,1,2,3};
    public GameObject[] buttons;

    public Camera mainCam;
    public Map layout = new Map();
    public GameObject GridElementPrefab;

    string base_path = "Assets/Resources/Level-";


    void Start()
    {
        if(newmap){
            layout.map = new Row[rows];
            for (int i = 0; i < rows; i++)
            {
                Row r = new Row(cols);
                for (int j = 0; j < cols; j++)
                {
                    r.rowdata[j] = 0;
                    var e = GameObject.Instantiate(GridElementPrefab, new Vector2(j - cols/2, -i + rows/2), Quaternion.identity, this.transform);
                    GridElement ge = e.GetComponent<GridElement>();
                    ge.s_renderer.sprite = placeables[1];
                    ge.layout_x = j;
                    ge.layout_y = i;
                }
                layout.map[i] = r;
            }
        }else{

        }
    }

    public void ChangeSelection(int selection){
        buttons[selectedElement].GetComponent<Image>().enabled = false;
        selectedElement = selection;
        buttons[selectedElement].GetComponent<Image>().enabled = true;
    }

    public void LoadMapIntoJson(){
        string path = base_path + levelNumber +".json";
        var txt = JsonUtility.ToJson(layout);
        Debug.Log(txt);
        
        File.WriteAllText(path, txt);   
    }

    public void LoadMapFromFile(){
        string path = base_path + levelNumber +".json";
        StreamReader reader = new StreamReader(path); 
        string text =  reader.ReadToEnd();

        Map layout = JsonUtility.FromJson<Map>(text);
        reader.Close();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 25);

            if(hit.collider != null)
            {
                Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                GridElement ge = hit.collider.gameObject.GetComponent<GridElement>();
                ge.s_renderer.sprite = placeables[selectedElement];
                layout.map[ge.layout_y].rowdata[ge.layout_x] = representation[selectedElement];
            }else{
                Debug.Log ("No object hit");
            }
        }
    }
}
