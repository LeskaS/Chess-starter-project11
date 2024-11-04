using UnityEngine;

public class Board : MonoBehaviour
{
    //материалы
    public Material defaultMaterial;
    public Material selectedMaterial;
    
    

    // поставить фигуру == указать местоположение фигуры по колонке и строке

    public GameObject AddPiece(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row); // получаем логическое представление
        GameObject newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint), Quaternion.identity, gameObject.transform); //ставим объект на доску
        return newPiece;
    }

    // Съесть фигуру - уничтожить игровой обьект
    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
        
    }

    // перемещение 

    public void MovePiece(GameObject piece, Vector2Int gridPoint)
    {
        piece.transform.position = Geometry.PointFromGrid(gridPoint);
    }

    // подменить материал на выбранный

    public void SelectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;
    }

    // подменить материал на невыбранный 


    public void DeselectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = defaultMaterial;
    }
}