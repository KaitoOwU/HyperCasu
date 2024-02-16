using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceCore : MonoBehaviour
{
    
    
    public bool[][][] GeneratePuzzlePiece(int size, float chance = 0.1f)
    {
        if(size < 0)
            throw new ArgumentException("Size must be greater than 0");
        
        if (size % 2 == 0)
            size++;
        int half = size / 2;
        Debug.Log(half);
        
        bool[][][] template = new bool[size][][];

        for (int i = 0; i < size; i++)
        {
            template[i] = new bool[size][];
            for (int j = 0; j < size; j++)
            {
                template[i][j] = new bool[size];
                for (int k = 0; k < size; k++)
                {
                    if (i == j && j == k && k == half)
                    {
                        template[i][j][k] = true;
                    }
                    else
                    {
                        template[i][j][k] = UnityEngine.Random.Range(0f, 1f) <= chance;
                    }
                    
                    if (template[i][j][k])
                    {
                        var obj = Instantiate(Resources.Load<GameObject>("Prefab/Piece"), new Vector3(i - half, j - half, k - half), Quaternion.identity);
                        obj.transform.SetParent(transform);
                    }
                    Debug.Log($"{i}, {j}, {k} : {template[i][j][k]}");
                }
            }
        }

        transform.localScale = Vector3.one / half;
        return template;
    }
}
