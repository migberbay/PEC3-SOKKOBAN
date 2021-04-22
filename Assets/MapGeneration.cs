using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Row{
    public int[] rowdata;
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


    void Start()
    {
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
    }
    void SpawnFloor(int row, int col){
    
    }
    void SpawnGeneric(int row, int col, int element){
    
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
        

    // public GameObject FrontWallRightConnection;
    // public GameObject FrontWallLeftConnection, FrontWallBothConnections, FrontWallNoConnection;

    // public GameObject WallSolo;
    // public GameObject WallTopRightLeft, WallTopRight, WallTopLeft, WallTop, WallLeft, WallRight, WallFull, WallLeftRight;

        switch (bin_val)
        {
            case "0000":
                GameObject.Instantiate(WallSolo,new Vector3(row,col), Quaternion.identity);
                break;

            case "0001":
                GameObject.Instantiate(WallTopRightLeft,new Vector3(row,col), Quaternion.identity);
                break;

            case "0010":
                Debug.Log("Can't Exist.");
                break;
            
            case "0011":
                GameObject.Instantiate(WallTopLeft,new Vector3(row,col), Quaternion.identity);
                break;
            
            case "0100":
                Debug.Log("Can't Exist.");
                break;

            case "0101":
                GameObject.Instantiate(WallTopRight,new Vector3(row,col), Quaternion.identity);
                break;

            case "0110":
                Debug.Log("Can't Exist.");
                break;
            
            case "0111":
                GameObject.Instantiate(WallTop,new Vector3(row,col), Quaternion.identity);
                break;

            case "1000":
                GameObject.Instantiate(FrontWallNoConnection,new Vector3(row,col), Quaternion.identity);
                break;


            default:
                GameObject.Instantiate(WallSolo,new Vector3(row,col), Quaternion.identity);
                break;
        }
    }
}

