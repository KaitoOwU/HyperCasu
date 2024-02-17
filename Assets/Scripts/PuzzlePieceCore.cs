using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PuzzlePieceCore : MonoBehaviour
{
    [SerializeField] private bool[][] _validTemplate;
    
    public PuzzlePieceCore GeneratePuzzlePiece(int size, float chance = 0.1f)
    {
        if(size < 0)
            throw new ArgumentException("Size must be greater than 0");
        
        if (size % 2 == 0)
            size++;
        int half = size / 2;
        
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
                        template[i][j][k] = Random.Range(0f, 1f) <= chance;
                    }
                    
                    if (template[i][j][k])
                    {
                        var obj = Instantiate(Resources.Load<GameObject>("Prefab/Piece"), new Vector3(i - half, j - half, k - half), Quaternion.identity);
                        obj.transform.SetParent(transform);
                    }
                }
            }
        }

        transform.localScale = Vector3.one / half;
        
        transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 3) * 90f, Random.Range(0, 3) * 90f, Random.Range(0, 3) * 90f));

        bool[][] template2D = new bool[size][];
        Vector3 startPos = transform.position - new Vector3(0, 0, 10);
        
        for (int i = 0; i < size; i++)
        {
            template2D[i] = new bool[size];
            for (int j = 0; j < size; j++)
            {
                Debug.DrawRay(startPos + new Vector3(-1 + j, 1 - i, 0), Vector3.forward * 20, Color.magenta, 1f);
                template2D[i][j] = Physics.Raycast(startPos + new Vector3(-1 + j, 1 - i, 0), Vector3.forward * 40, 40, LayerMask.GetMask("Piece"));
                Debug.Log($"{i}, {j} : {template2D[i][j]}");
            }
        }

        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        KeyholeUI.Instance.SetupTemplate(template2D);
        _validTemplate = template2D;

        return this;
    }

    public bool TestIfValid()
    {
        bool[][] template2D = new bool[_validTemplate.Length][];
        Vector3 startPos = transform.position - new Vector3(0, 0, 10);
        
        for (int i = 0; i < _validTemplate.Length; i++)
        {
            template2D[i] = new bool[_validTemplate.Length];
            for (int j = 0; j < _validTemplate.Length; j++)
            {
                Debug.DrawRay(startPos + new Vector3(-1 + j, 1 - i, 0), Vector3.forward * 20, Color.magenta, 1f);
                template2D[i][j] = Physics.Raycast(startPos + new Vector3(-1 + j, 1 - i, 0), Vector3.forward * 40, 40, LayerMask.GetMask("Piece"));
            }
        }
        
        for (int i = 0; i < _validTemplate.Length; i++)
        {
            for (int j = 0; j < _validTemplate[i].Length; j++)
            {
                if (_validTemplate[i][j] != template2D[i][j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
