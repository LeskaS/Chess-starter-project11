
using System.Collections.Generic;
using UnityEngine;
//пешка ходит вперед на 2 клетки, на одну, рубит по диагонали
public class Pawn : Piece
{
    public override List<Vector2Int>  MoveLocations(Vector2Int gridPoint) 
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        
        // всегда умеет ходить вперед если клетка ещё на доске
        int forwardDirection = GameManager.instance.currentPlayer.forward;
        Vector2Int forwardOne = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardOne) == false)

        {
             locations.Add(forwardOne);
        }

        // ходит на 2 если ещё не ходила
        Vector2Int forwardTwo = new Vector2Int(gridPoint.x, gridPoint.y + 2 * forwardDirection);
        if (GameManager.instance.HasPawnMoved(gameObject) == false && GameManager.instance.PieceAtGrid(forwardTwo) == false)
        {
            locations.Add(forwardTwo);
        }

        // может рубить по диагонали вперед справа

        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardRight))
        {
            locations.Add(forwardRight);
        }

        // рубить по диаганали слева
        Vector2Int forwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardLeft))
        {
            locations.Add(forwardLeft);
        }

        return locations;

        
    }
}
