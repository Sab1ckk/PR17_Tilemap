using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "TilemapConfig", menuName = "PR17/Tilemap Config", order = 2)]
public class TilemapConfig : ScriptableObject
{
    [Header("Общие настройки")] public string configName = "Основная конфигурация";
    public DifficultyLevel difficulty = DifficultyLevel.Normal;
    [Header("Настройки сетки")] public Vector3 cellSize = new Vector3(1, 1, 0);
    public Vector3 cellGap = Vector3.zero;
    public GridLayout.CellLayout cellLayout = GridLayout.CellLayout.Rectangle;
    [Header("Настройки рендера")] public int sortingOrder = 0;
    public TilemapRenderer.Mode renderMode = TilemapRenderer.Mode.Chunk;
    [Header("Настройки коллизий")] public bool useCompositeCollider = true;
    public bool autoGenerateColliders = true;
    [Header("Лимиты")] public int maxTilesPerLevel = 10000;
    public int maxLayers = 5;
    public string GetFullInfo()
    {
        string info = "<b>КОНФИГУРАЦИЯ TILEMAP</b>\n";
        info += $"Название: {configName}\n";
        info += $"Сложность: {GetDifficultyName()}\n";
        info += $"Размер ячейки: ({cellSize.x:F1}, {cellSize.y:F1})\n";
        info += $"Layout: {cellLayout}\n";
        info += $"Sorting Order: {sortingOrder}\n";
        info += $"Render Mode: {renderMode}\n";
        info += $"Composite Collider: {(useCompositeCollider ? "Да" : "Нет")}\n";
        info += $"Макс. тайлов: {maxTilesPerLevel}\n";
        info += $"Макс. слоёв: {maxLayers}";
        return info;
    }
    private string GetDifficultyName()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy: return "Лёгкая";
            case DifficultyLevel.Normal: return "Обычная";
            case DifficultyLevel.Hard: return "Сложная";
            case DifficultyLevel.Nightmare: return "Кошмар";
            default: return "Неизвестно";
        }
    }
    public void CycleDifficulty()
    {
        int next = ((int)difficulty + 1) % 4;
        difficulty = (DifficultyLevel)next;
    }
}
public enum DifficultyLevel { Easy, Normal, Hard, Nightmare }
