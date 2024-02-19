using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyholeUI : MonoBehaviour
{
    [SerializeField] private Image[] _keyholes = new Image[9];
    [SerializeField] private Color _colorHole, _colorPlain;

    public void SetupTemplate(bool[][] template2D)
    {
        for (int i = 0; i < template2D.Length; i++)
        {
            for (int j = 0; j < template2D[i].Length; j++)
            {
                bool result = template2D[i][j];
                _keyholes[template2D.Length * i + j].color = result ? _colorHole : _colorPlain;
            }
        }
    }
    
}
