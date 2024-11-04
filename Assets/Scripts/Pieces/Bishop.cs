

using System.Collections.Generic;
using UnityEngine;
// слон (вертикали, горизонтали)
public class Bishop : Piece
{    

    // метод кот. принимает доску и возвращает возможные позиции
    // Vector2Int - позиции, представление двумерных векторов и точек с помощью целых чисел
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint) 
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        foreach (Vector2Int dir in BishopDirections) // проходимся по всем направлениям
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int nextGridPoint = 
                new Vector2Int(
                    gridPoint.x + i * dir.x,
                     gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);


                if (GameManager.instance.PieceAtGrid(nextGridPoint)) // находится ли на доске
                {
                    break;
                }
            }
        }

        return locations;
    }
}
