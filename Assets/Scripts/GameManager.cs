using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CanvasGroup _mainMenuUI, _gameUI;
    
    private PuzzlePieceCore _currentPuzzle;
    [SerializeField] private Transform _doorLeft, _doorRight;

    private int _score = 0;
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    private bool _rotateOnX, _rotateOnY, _rotateOnZ;
    
    public const float ROTATION_SPEED = 75f;
    
    private void Awake()
    {
        Instance = this;
        GeneratePiece();
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
            _currentPuzzle.transform.Rotate(ROTATION_SPEED * Time.deltaTime, 0, 0, Space.World);
        }
        if (_rotateOnY)
        {
            _currentPuzzle.transform.Rotate(0, ROTATION_SPEED * Time.deltaTime, 0, Space.World);
        }
        if (_rotateOnZ)
        {
            _currentPuzzle.transform.Rotate(0, 0, ROTATION_SPEED * Time.deltaTime, Space.World);
        }
    }

    public void StartGame()
    {
        _mainMenuUI.DOFade(0f, 1f).OnComplete(() =>
        {
            _mainMenuUI.gameObject.SetActive(false);
            
            _gameUI.gameObject.SetActive(true);
            FindObjectsByType<KeyholeUI>(FindObjectsSortMode.None).ToList().ForEach((keyhole) => keyhole.SetupTemplate(_currentPuzzle.ValidTemplate));
            
            _gameUI.DOFade(1f, 1f);
        });
    }

    public void CheckIfPuzzleCorrect()
    {
        if (_currentPuzzle.TestIfValid())
        {
            StartCoroutine(Win());
        }
    }

    private IEnumerator Win()
    {
        _score++;
        _scoreText.text = _score.ToString();
        
        yield return _currentPuzzle.transform.DORotate(new(0, 0, 0), 0.5f).WaitForCompletion();
        yield return _currentPuzzle.transform.DOLocalMoveZ(_currentPuzzle.transform.localPosition.z + 5.75f, 1f).WaitForCompletion();
        _currentPuzzle.transform.SetParent(_doorLeft);


        Transform tempDoorLeft = _doorLeft, tempDoorRight = _doorRight;
        GeneratePiece();
        
        tempDoorLeft.DOLocalRotate(new(0, 90, 0), 1f);
        yield return tempDoorRight.DOLocalRotate(new(0, -90, 0), 1f).WaitForCompletion();
        
        yield return Camera.main.transform.DOMoveZ(Camera.main.transform.position.z + 11f, 1f).WaitForCompletion();
        Destroy(tempDoorLeft.parent.parent.gameObject);
    }

    private void GeneratePiece()
    {
        Vector3 pos = _currentPuzzle is null ? Vector3.zero : _currentPuzzle.transform.position;
        
        var piece = Instantiate(Resources.Load<GameObject>("Prefab/-- DUO DOOR --"), pos + new Vector3(0, 0, 6),
            Quaternion.identity).GetComponentInChildren<DuoDoor>();
        _doorLeft = piece.DoorLeft;
        _doorRight = piece.DoorRight;
        _currentPuzzle = piece.PuzzleCore;

        StartCoroutine(_currentPuzzle.GeneratePuzzlePiece(3, 0.1f));
    }
}
