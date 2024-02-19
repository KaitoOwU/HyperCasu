using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoDoor : MonoBehaviour
{
    [SerializeField] private Transform _doorLeft, _doorRight;
    [SerializeField] private PuzzlePieceCore _puzzlePieceCore;
    
    public PuzzlePieceCore PuzzleCore => _puzzlePieceCore;
    public Transform DoorLeft => _doorLeft;
    public Transform DoorRight => _doorRight;
}
