using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "TileData", menuName = "PR17/Tile Data", order = 1)]
public class TileData : ScriptableObject
{
    [Header("Основная информация")] public string tileName = "Новый тайл";
    [TextArea(3, 5)] public string description = "Описание тайла";
    [Header("Визуал")] public Sprite tileSprite;
    public Color tileColor = Color.white;
    [Header("Физика")] public Tile.ColliderType colliderType = Tile.ColliderType.Sprite;
    public bool isSolid = true;
    public PhysicsMaterial2D physicsMaterial;
    [Header("Категория")] public TileCategory category = TileCategory.Ground;
    public TileRarity rarity = TileRarity.Common;
    [Header("Стоимость размещения")] public int placementCost = 0;
    public string GetFullInfo()
    {
        string info = $"<b>{tileName}</b>\n";
        info += $"<i>{description}</i>\n\n";
        info += $"Категория: {GetCategoryName()}\n";
        info += $"Редкость: {GetRarityName()}\n";
        info += $"Тип коллайдера: {colliderType}\n";
        info += $"Твёрдый: {(isSolid ? "Да" : "Нет")}\n";
        info += $"Стоимость: {placementCost} золота";
        return info;
    }
    private string GetCategoryName()
    {
        switch (category)
        {
            case TileCategory.Ground: return "Земля";
            case TileCategory.Platform: return "Платформа";
            case TileCategory.Wall: return "Стена";
            case TileCategory.Hazard: return "Опасность";
            case TileCategory.Decoration: return "Декорация";
            default: return "Неизвестно";
        }
    }
    private string GetRarityName()
    {
        switch (rarity)
        {
            case TileRarity.Common: return "Обычный";
            case TileRarity.Rare: return "Редкий";
            case TileRarity.Epic: return "Эпический";
            case TileRarity.Legendary: return "Легендарный";
            default: return "Неизвестно";
        }
    }
}
public enum TileCategory { Ground, Platform, Wall, Hazard, Decoration }
public enum TileRarity { Common, Rare, Epic, Legendary }
