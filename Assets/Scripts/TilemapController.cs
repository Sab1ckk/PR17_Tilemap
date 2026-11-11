using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class TilemapController : MonoBehaviour
{
    [Header("UI элементы")] public TextMeshProUGUI statusText;
    public TextMeshProUGUI tileInfoText;
    public TextMeshProUGUI configText;
    public Transform tilesContainer;
    [Header("Кнопки")] public Button showConfigButton;
    public Button clearTilemapButton;
    public Button toggleColliderButton;
    public Button fillTestButton;
    public GameObject tileButtonPrefab;
    [Header("ScriptableObject ссылки")] public TileDatabase tileDatabase;
    public TilemapConfig tilemapConfig;
    public TilemapPainter tilemapPainter;
    [Header("Tilemap ссылки")] public Tilemap groundTilemap;
    private TileBase selectedTile;
    private bool collidersEnabled = true;
    void Start()
    {
        if (showConfigButton != null) showConfigButton.onClick.AddListener(ShowConfig);
        if (clearTilemapButton != null) clearTilemapButton.onClick.AddListener(ClearTilemap);
        if (toggleColliderButton != null) toggleColliderButton.onClick.AddListener(ToggleCollider);
        if (fillTestButton != null) fillTestButton.onClick.AddListener(FillTestLevel);
        PopulateTileList();
        UpdateStatus("Выберите тайл для рисования");
        UpdateConfigText();
    }
    void PopulateTileList()
    {
        if (tileDatabase == null || tilesContainer == null) return;
        foreach (Transform child in tilesContainer) Destroy(child.gameObject);
        for (int i = 0; i < tileDatabase.allTiles.Count; i++)
        {
            TileBase tile = tileDatabase.allTiles[i];
            TileData data = i < tileDatabase.tileDataList.Count ? tileDatabase.tileDataList[i] : null;
            GameObject buttonObj = Instantiate(tileButtonPrefab, tilesContainer);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                string tileName = data != null ? data.tileName : tile.name;
                buttonText.text = tileName;
            }
            Button button = buttonObj.GetComponent<Button>();
            TileBase capturedTile = tile;
            TileData capturedData = data;
            button.onClick.AddListener(() => SelectTile(capturedTile, capturedData));
        }
    }
    void SelectTile(TileBase tile, TileData data)
    {
        selectedTile = tile;
        if (tilemapPainter != null) tilemapPainter.selectedTile = tile;
        if (tileInfoText != null && data != null) tileInfoText.text = data.GetFullInfo();
        else if (tileInfoText != null) tileInfoText.text = $"Тайл: {tile.name}";
        UpdateStatus($"Выбран: {(data != null ? data.tileName : tile.name)}");
    }
    void ShowConfig()
    {
        if (tilemapConfig == null) return;
        string configInfo = tilemapConfig.GetFullInfo();
        if (configText != null) configText.text = configInfo.Replace("\n", " | ");
        UpdateStatus("Конфигурация отображена");
        Debug.Log(configInfo);
    }
    void ClearTilemap()
    {
        if (tilemapPainter != null) tilemapPainter.ClearAllTiles();
        UpdateStatus("Тайлмап очищен!");
        if (configText != null) configText.text = "Тайлмап очищен";
    }
    void ToggleCollider()
    {
        collidersEnabled = !collidersEnabled;
        if (groundTilemap != null)
        {
            TilemapCollider2D tilemapCollider = groundTilemap.GetComponent<TilemapCollider2D>();
            
            CompositeCollider2D composite = groundTilemap.GetComponent<CompositeCollider2D>();
            if (composite != null) composite.enabled = collidersEnabled;
            if (tilemapCollider != null) tilemapCollider.enabled = collidersEnabled;
            Rigidbody2D body = groundTilemap.GetComponent<Rigidbody2D>();
            if (body != null) body.simulated = collidersEnabled;
        }
        UpdateStatus($"Коллизии: {(collidersEnabled ? "Включены" : "Отключены")}");
    }
    void FillTestLevel()
    {
        if (tilemapPainter != null) tilemapPainter.FillTestLevel();
        UpdateStatus("Тестовый уровень создан!");
    }
    void UpdateStatus(string message) { if (statusText != null) statusText.text = message; }
    void UpdateConfigText()
    {
        if (configText != null && tilemapConfig != null)
            configText.text = $"Конфигурация: {tilemapConfig.configName} | Layout: {tilemapConfig.cellLayout}";
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && selectedTile != null && Camera.main != null &&
            (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (tilemapPainter != null) tilemapPainter.PaintAt(mousePos);
        }
        if (Input.GetKeyDown(KeyCode.C)) ClearTilemap();
        if (Input.GetKeyDown(KeyCode.F)) FillTestLevel();
        if (Input.GetKeyDown(KeyCode.T)) ToggleCollider();
        if (Input.GetKey(KeyCode.LeftShift) && Camera.main != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (groundTilemap != null)
            {
                Vector3Int cell = groundTilemap.WorldToCell(mousePos);
                UpdateStatus($"Ячейка: ({cell.x}, {cell.y}) | Тайл: {(tilemapPainter.HasTileAt(mousePos) ? "Да" : "Нет")}");
            }
        }
    }
}


