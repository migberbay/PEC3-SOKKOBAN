using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int colLen, rowLen;
    public float x_space, y_space;
    public GameObject indicator;

    public Sprite[] placeables;
    public int[] representation = new int[] {-1,0,1,2,3};

    void Start()
    {
        for (int i = 0; i < colLen * rowLen; i++)
        {
            
        }
    }
    void Update()
    {
        
    }
}
