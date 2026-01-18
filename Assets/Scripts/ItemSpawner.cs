using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{
    [Header("生成设置")]
    [Tooltip("生成的物品")]
    public GameObject[] itemPrefabs;

    [Tooltip("生成的数量")]
    public int itemCount = 8;

    [Tooltip("生成范围使用BoxCollider2D框住整个范围")]
    public BoxCollider2D spawnArea;

    [Tooltip("墙壁Tilemap避免生成到墙壁里")]
    public Tilemap wallTilemap;
    // Start is called before the first frame update
    private void Start()
    {
        wallTilemap = GameObject.FindGameObjectWithTag("WallTilemap").GetComponent<Tilemap>();
        SpawnItems();
    }

    // 生成全部物品
    public void SpawnItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            SpawnSingleItem();
        }
    }

    // 生成单个物品
    private void SpawnSingleItem()
    {
        // 尝试多次找到合适的位置
        for (int attempts = 0; attempts < 10; attempts++)
        {
            Vector2 randomPos = GetRandomPosition();
            Vector3Int cellPos = wallTilemap.WorldToCell(randomPos);
            // 检查位置是否在墙壁Tilemap中
            // 检查位置是否远离墙面
            if (wallTilemap.HasTile(cellPos) == false)
            {
                Vector3 spawnPos = wallTilemap.GetCellCenterWorld(cellPos);
                GameObject spawn = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                Instantiate(spawn, spawnPos, Quaternion.identity);
                return;
            }
        }
    }
    // 获取随机位置
    private Vector2 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
    
}
