#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Tilemaps;

namespace Tiles
{
    public class DiggableTile: Tile
    {
        public float strength;

        // public MineralData mineral;
        
#if UNITY_EDITOR
        [MenuItem("Assets/Create/DiggableTile")]
        public static void CreateDiggableTile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Diggable Tile", "New Diggable Tile", "Asset", "Save Diggable Tile", "Assets");

            if (path == "")
            {
                return;
            }
            
            AssetDatabase.CreateAsset(CreateInstance<DiggableTile>(), path);
        }
#endif
    }
}