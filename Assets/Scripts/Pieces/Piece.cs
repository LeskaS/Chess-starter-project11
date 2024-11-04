
using System.Collections.Generic;
using UnityEngine;
//класс, который предсьавляет собой фигуру 
public enum PieceType {King, Queen, Bishop, Knight, Rook, Pawn};

public abstract class Piece : MonoBehaviour
{
    public PieceType type;
    public AudioSource DestroySound;

    //возможные направления ладьи

    protected Vector2Int[] RookDirections = {
        new Vector2Int(0,1), //up можно записать как Vector2Int.up
        new Vector2Int(1, 0),//right
        new Vector2Int(0, -1),// down
        new Vector2Int(-1, 0)}; // left

        // возможные направления для слона по диагоналям
    protected Vector2Int[] BishopDirections = {
        new Vector2Int(1,1), 
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1), 
        new Vector2Int(-1, 1)};

        // метод кот. возвращает список возможных локаций на доске для фигуры, игнорируя остальные

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
}