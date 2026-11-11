using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapPainter : MonoBehaviour
{
    [Header("Ссылки на Tilemap")] public Tilemap groundTilemap;
    public Tilemap decorationTilemap;
    [Header("Настройки")] public TileBase selectedTile;
    public bool paintOnStart = false;
    [Header("Статистика")] public int tilesPainted = 0;
    void Start() { if (paintOnStart) FillTestLevel(); }
    public void PaintAt(Vector3 worldPos)
    {
        if (groundTilemap == null || selectedTile == null) return;
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        groundTilemap.SetTile(cell, selectedTile);
        tilesPainted++;
    }
    public void PaintAtCell(Vector3Int cell)
    {
        if (groundTilemap == null || selectedTile == null) return;
        groundTilemap.SetTile(cell, selectedTile);
        tilesPainted++;
    }
    public bool HasTileAt(Vector3 worldPos)
    {
        if (groundTilemap == null) return false;
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        return groundTilemap.GetTile(cell) != null;
    }
    public TileBase GetTileAt(Vector3 worldPos)
    {
        if (groundTilemap == null) return null;
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        return groundTilemap.GetTile(cell);
    }
    public void FillArea(Vector3Int start, int width, int height, TileBase tile)
    {
        if (groundTilemap == null || tile == null) return;
        BoundsInt bounds = new BoundsInt(start.x, start.y, 0, width, height, 1);
        TileBase[] tiles = new TileBase[width * height];
        for (int i = 0; i < tiles.Length; i++) tiles[i] = tile;
        groundTilemap.SetTilesBlock(bounds, tiles);
        tilesPainted += width * height;
        Debug.Log($"Заполнена область {width}x{height} тайлами {tile.name}");
    }
    public void ClearAllTiles()
    {
        if (groundTilemap != null)
        {
            groundTilemap.ClearAllTiles();
            tilesPainted = 0;
            Debug.Log("Все тайлы очищены");
        }
        if (decorationTilemap != null) decorationTilemap.ClearAllTiles();
    }
    public void FillTestLevel()
    {
        if (groundTilemap == null || selectedTile == null) return;
        for (int x = -10; x < 10; x++)
        {
            Vector3Int cell = new Vector3Int(x, -3, 0);
            groundTilemap.SetTile(cell, selectedTile);
            tilesPainted++;
        }
        for (int x = -8; x < -4; x++) groundTilemap.SetTile(new Vector3Int(x, 0, 0), selectedTile);
        for (int x = 2; x < 6; x++) groundTilemap.SetTile(new Vector3Int(x, 1, 0), selectedTile);
        Debug.Log("Тестовый уровень создан");
    }
    public bool IsSolid(Vector3 worldPos)
    {
        TileBase tile = GetTileAt(worldPos);
        return tile != null;
    }
}
