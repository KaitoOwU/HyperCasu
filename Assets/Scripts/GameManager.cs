using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private PuzzlePieceCore _currentPuzzle;
    private bool _rotateOnX, _rotateOnY, _rotateOnZ;

    public const float VALIDATION_TRESHOLD = 30f;
    public const float ROTATION_SPEED = 0.25f;
    
    private void Awake()
    {
        Instance = this;
        _currentPuzzle = Instantiate(Resources.Load<GameObject>("Prefab/-- PUZZLE PIECE --"), Vector3.zero, Quaternion.identity).GetComponent<PuzzlePieceCore>().GeneratePuzzlePiece(3, 0.1f);
    }

    public void RotateOnAxis(string axis)
    {
        switch (axis)
        {
            case "X":
                _rotateOnX = !_rotateOnX;
                break;
            
            case "Y":
                _rotateOnY = !_rotateOnY;
                break;
            
            case "Z":
                _rotateOnZ = !_rotateOnZ;
                break;
            
            default:
                throw new ArgumentException();
        }
    }

    private void Update()
    {
        if (_rotateOnX)
        {
            _currentPuzzle.transform.Rotate(ROTATION_SPEED, 0, 0, Space.World);
        }
        if (_rotateOnY)
        {
            _currentPuzzle.transform.Rotate(0, ROTATION_SPEED, 0, Space.World);
        }
        if (_rotateOnZ)
        {
            _currentPuzzle.transform.Rotate(0, 0, ROTATION_SPEED, Space.World);
        }
    }

    public void CheckIfPuzzleCorrect()
    {
        if (_currentPuzzle.TestIfValid())
        {
            Debug.Log("Let's go");
        }
    }
}
