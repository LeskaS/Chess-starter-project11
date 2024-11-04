using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<GameObject> pieces; // фигуры
    public List<GameObject> capturedPieces;

    public string name; // имя игрока (белый или чёрный)
    public int forward; // направление

    public Player(string name, bool positiveZMovement)
    {
        this.name = name;
        pieces = new List<GameObject>();
        capturedPieces = new List<GameObject>();

        if (positiveZMovement == true)
        {
            this.forward = 1;
        }
        else
        {
            this.forward = -1;
        }
    }
}