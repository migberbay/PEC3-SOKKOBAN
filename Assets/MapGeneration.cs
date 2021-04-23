using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Row{
    public int[] rowdata;

    public Row(int size){
        rowdata = new int[size];
    }
}


public class MapGeneration : MonoBehaviour
{
    // 0 = air, 1 = wall, 2 = Box, 3 = Portal, -1 = link spawn
    public Row[] mapLayout;

    public GameObject[] floorTiles; // 2nd & 3rd position 5% weight
    public GameObject box, portal, link;

    [Header("Front Wall Objects")]
    public GameObject FrontWallRightConnection;
    public GameObject FrontWallLeftConnection, FrontWallBothConnections, FrontWallNoConnection;

    [Header("Wall Objects")]
    public GameObject WallSolo;
    public GameObject WallTopRightLeft, WallTopRight, WallTopLeft, WallTop, WallLeft, WallRight, WallFull, WallLeftRight;

    public Transform elementHolder, mapHolder;

    void Start()
    {
        SurroundMapWithWalls();

        for (int row = 0; row < mapLayout.Length; row++)
        {
            for(int col = 0; col < mapLayout[row].rowdata.Length; col++){
                int element = mapLayout[row].rowdata[col];                
                SpawnFloor(row, col);
                if(element == 1){
                    SpawnWall(row, col);
                }else{
                    SpawnGeneric(row, col, element);
                }  
            }
        }

        Debug.Log("Map was built in " + Time.realtimeSinceStartup + " seconds.");
    }

    void SurroundMapWithWalls(){
        int firstRowLen = mapLayout[0].rowdata.Length + 2;
        int lastRowLen = mapLayout[mapLayout.Length-1].rowdata.Length + 2;

        Row fullRowFirst = new Row(firstRowLen);
        Row fullRowLast = new Row(lastRowLen);

        for(int i = 0; i < firstRowLen ; i++){
            fullRowFirst.rowdata[i] = 1;
        }

        for(int i = 0; i < lastRowLen ; i++){
            fullRowLast.rowdata[i] = 1;
        }
        int l_len = mapLayout.Length;

        Row[] mapLayoutFull = new Row[l_len + 4];     
        mapLayoutFull[0] = fullRowFirst;
        mapLayoutFull[1] = fullRowFirst;
        mapLayoutFull[l_len + 2] = fullRowLast;
        mapLayoutFull[l_len + 3] = fullRowLast;
        

        for (int row = 2; row < l_len + 2; row++)
        {
            mapLayoutFull[row] = new Row(mapLayout[row - 2].rowdata.Length + 2);
            mapLayoutFull[row].rowdata[0] = 1;
            mapLayoutFull[row].rowdata[mapLayoutFull[row].rowdata.Length-1] = 1;

            for(int col = 1; col < mapLayoutFull[row].rowdata.Length-1; col++){
                mapLayoutFull[row].rowdata[col] = mapLayout[row - 2].rowdata[col-1];
            }
        }

        mapLayout = mapLayoutFull;
       
        string str = "";
        foreach (var row in mapLayout)
        {
            str = "";
            foreach (var val in row.rowdata)
            {
                str += val+" ";
            }
            // Debug.Log(str);
        }
    }

    void SpawnFloor(int row, int col){
        GameObject toInstantiate;

        float r = Random.Range(0,100);

        if(r > 0 && r < 47.5f){
            toInstantiate = floorTiles[0];
        }else if (r > 47.5f && r < 95f){
            toInstantiate = floorTiles[1];
        }else if (r > 95f && r < 97.5f){
            toInstantiate = floorTiles[2];
        }else{
            toInstantiate = floorTiles[3];
        }

        var instance = GameObject.Instantiate(toInstantiate,new Vector3(col,-row), Quaternion.identity);
        instance.transform.SetParent(mapHolder);
    }

    void SpawnGeneric(int row, int col, int element){
        
        GameObject toInstantiate;
        // 0 = air, 1 = wall, 2 = Box, 3 = Portal, -1 = link spawn

        if(element == 2){
            toInstantiate = box;
        }else if(element == 3){
            toInstantiate = portal;
        }else if(element == -1){
            toInstantiate = link;
        }else if(element == 0){
            toInstantiate = null;
        }else{
            Debug.Log("adding not contemplated element: " + element);
            toInstantiate = floorTiles[0];
        }
        if(toInstantiate != null){
            var instance = GameObject.Instantiate(toInstantiate, new Vector3(col,-row), Quaternion.identity);
            instance.transform.SetParent(elementHolder);
        }
        
    }

    void SpawnWall(int row, int col){
        int up, down, left, right; 
                
        if(col == 0){
            left = 0;
            right = mapLayout[row].rowdata[col+1] == 1 ? 1 : 0;
        }else if(col == mapLayout[row].rowdata.Length - 1){
            right = 0;
            left = mapLayout[row].rowdata[col-1] == 1 ? 1 : 0;
        }else{
            right = mapLayout[row].rowdata[col+1] == 1 ? 1 : 0;
            left = mapLayout[row].rowdata[col-1] == 1 ? 1 : 0;
        }

        if(row == 0){
            up = 0;
            down = mapLayout[row+1].rowdata[col] == 1 ? 1 : 0; 
        }else if(row == mapLayout.Length-1){
            down = 0;
            up = mapLayout[row-1].rowdata[col] == 1 ? 1 : 0;
        }else{
            down = mapLayout[row+1].rowdata[col] == 1 ? 1 : 0; 
            up = mapLayout[row-1].rowdata[col] == 1 ? 1 : 0;
        }

        string bin_val = up+""+left+""+right+""+down;
        
        switch (bin_val)
        {
            case "0000":
                var instance = GameObject.Instantiate(WallSolo,new Vector3(col,-row), Quaternion.identity);
                instance.transform.SetParent(mapHolder);
                break;

            case "0001":
                var instance1 = GameObject.Instantiate(WallTopRightLeft,new Vector3(col,-row), Quaternion.identity);
                instance1.transform.SetParent(mapHolder);
                break;

            // case "0010":
            //     Debug.Log("Only wall to the right, Can't Exist." + row+ " " +col);
            //     GameObject.Instantiate(WallSolo,new Vector3(col,-row), Quaternion.identity);
            //     break;
            
            case "0011":
                var instance2 = GameObject.Instantiate(WallTopRight,new Vector3(col,-row), Quaternion.identity);
                instance2.transform.SetParent(mapHolder);
                break;
            
            // case "0100":
            //     Debug.Log("Only Wall to the left Can't Exist." + row+ " " +col);
            //     GameObject.Instantiate(WallSolo,new Vector3(col,-row), Quaternion.identity);
            //     break;

            case "0101":
                var instance3 = GameObject.Instantiate(WallTopLeft,new Vector3(col,-row), Quaternion.identity);
                instance3.transform.SetParent(mapHolder);
                break;

            // case "0110":
            //     Debug.Log("Wall to the left and to the right, Can't Exist." + row+ " " +col);
            //     GameObject.Instantiate(WallSolo,new Vector3(col,-row), Quaternion.identity);
            //     break;
            
            case "0111":
                var instance4 = GameObject.Instantiate(WallTop,new Vector3(col,-row), Quaternion.identity);
                instance4.transform.SetParent(mapHolder);
                break;

            case "1000":
                var instance5 = GameObject.Instantiate(FrontWallNoConnection,new Vector3(col,-row), Quaternion.identity);
                instance5.transform.SetParent(mapHolder);
                break;

            case "1001":
                var instance6 = GameObject.Instantiate(WallLeftRight,new Vector3(col,-row), Quaternion.identity);
                instance6.transform.SetParent(mapHolder);
                break;
            
            case "1010":
                var instance7 = GameObject.Instantiate(FrontWallRightConnection,new Vector3(col,-row), Quaternion.identity);
                instance7.transform.SetParent(mapHolder);
                break;
            
            case "1011":
                var instance8 = GameObject.Instantiate(WallRight,new Vector3(col,-row), Quaternion.identity);
                instance8.transform.SetParent(mapHolder);
                break;
            
            case "1100":
                var instance9 = GameObject.Instantiate(FrontWallLeftConnection,new Vector3(col,-row), Quaternion.identity);
                instance9.transform.SetParent(mapHolder);
                break;
            
            case "1101":
                var instance10 = GameObject.Instantiate(WallLeft,new Vector3(col,-row), Quaternion.identity);
                instance10.transform.SetParent(mapHolder);
                break;

            case "1110":
                var instance11 = GameObject.Instantiate(FrontWallBothConnections,new Vector3(col,-row), Quaternion.identity);
                instance11.transform.SetParent(mapHolder);
                break;
            
            case "1111":
                var instance12 = GameObject.Instantiate(WallFull,new Vector3(col,-row), Quaternion.identity);
                instance12.transform.SetParent(mapHolder);
                break;

            default:
                var instance13 = GameObject.Instantiate(WallSolo,new Vector3(col,-row), Quaternion.identity);
                instance13.transform.SetParent(mapHolder);
                break;
        }
    }
}

