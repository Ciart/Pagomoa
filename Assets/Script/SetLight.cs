using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetLight : MonoBehaviour
{
    public Tilemap tilemap;
    public BoxCollider2D checkCollison;
    public GameObject LightPrefab;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        checkCollison = GetComponent<BoxCollider2D>();
        LightPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Light.prefab");
    }
    public void SetTilmapLightly(Vector3 Pos)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(Pos);


        GameObject instance = Instantiate(LightPrefab);

        instance.transform.position = cellPosition;
        instance.transform.rotation = Quaternion.identity;
    }
}
