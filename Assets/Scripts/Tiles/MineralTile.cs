#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Tilemaps;

namespace Tiles
{
    public class MineralTile: Tile
    {
        public MineralData data;
        
#if UNITY_EDITOR
        [MenuItem("Assets/Create/MineralTile")]
        public static void CreateMineralTile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Mineral Tile", "New Mineral Tile", "Asset", "Save Mineral Tile", "Assets");

            if (path == "")
            {
                return;
            }
            
            AssetDatabase.CreateAsset(CreateInstance<MineralTile>(), path);
        }
#endif
    }
}