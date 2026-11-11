using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "TileDatabase", menuName = "PR17/Tile Database", order = 3)]
public class TileDatabase : ScriptableObject
{
    [Header("База тайлов")] public List<TileBase> allTiles = new List<TileBase>();
    public List<TileData> tileDataList = new List<TileData>();
    public TileBase GetTileByName(string name)
    {
        for (int i = 0; i < tileDataList.Count; i++)
            if (tileDataList[i].tileName == name && i < allTiles.Count) return allTiles[i];
        return null;
    }
    public TileData GetTileData(TileBase tile)
    {
        int index = allTiles.IndexOf(tile);
        if (index >= 0 && index < tileDataList.Count) return tileDataList[index];
        return null;
    }
    public List<TileBase> GetTilesByCategory(TileCategory category)
    {
        List<TileBase> result = new List<TileBase>();
        for (int i = 0; i < tileDataList.Count; i++)
            if (tileDataList[i].category == category && i < allTiles.Count) result.Add(allTiles[i]);
        return result;
    }
    public TileBase GetRandomTile()
    {
        if (allTiles.Count == 0) return null;
        return allTiles[Random.Range(0, allTiles.Count)];
    }
}
