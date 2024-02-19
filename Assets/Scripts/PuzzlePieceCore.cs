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
    private bool _isWin = false;

    public bool[][] ValidTemplate => _validTemplate;

    public bool IsWin => _isWin;
    
    public IEnumerator GeneratePuzzlePiece(int size, float chance = 0.1f)
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
                        var obj = Instantiate(Resources.Load<GameObject>("Prefab/Piece"), transform);
                        obj.transform.localPosition = new Vector3(i - half, j - half, k - half);
                    }
                }
            }
        }

        transform.localScale = Vector3.one / half;
        yield return new WaitForSeconds(0.5f);

        bool[][] template2D = new bool[size][];
        Vector3 startPos = transform.position - new Vector3(0, 0, 10);
        
        for (int i = 0; i < size; i++)
        {
            template2D[i] = new bool[size];
            for (int j = 0; j < size; j++)
            {
                Debug.DrawRay(startPos + new Vector3(-1 + j, 1 - i, 0), Vector3.forward * 20, Color.green, 1f);
                template2D[i][j] = Physics.Raycast(startPos + new Vector3(-1 + j, 1 - i, 0), Vector3.forward * 40, 40, LayerMask.GetMask("Piece"));
                Debug.Log($"{i}, {j} : {template2D[i][j]}");
            }
        }
        
        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        FindObjectsByType<KeyholeUI>(FindObjectsSortMode.None).ToList().ForEach((keyhole) => keyhole.SetupTemplate(template2D));
        _validTemplate = template2D;
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

        if (_isWin)
            return false;
        
        _isWin = true;
        return true;
    }
}
