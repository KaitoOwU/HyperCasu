using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        Instantiate(Resources.Load<GameObject>("Prefab/-- PUZZLE PIECE --"), Vector3.zero, Quaternion.identity).GetComponent<PuzzlePieceCore>().GeneratePuzzlePiece(5, 0.1f);
    }
}
