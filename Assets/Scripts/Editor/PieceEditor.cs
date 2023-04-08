using System;
using System.Linq;
using Maps;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PieceEditor : EditorWindow
    {
        private int _tabIndex;

        private string[] _tabStrings =
        {
            "Mineral", "Ground", "Pivot"
        };

        private MapDatabase _database;

        private int _selectMineral;

        private int _selectGround;

        [MenuItem("Window/Piece Editor")]
        private static void Init()
        {
            var window = (PieceEditor)GetWindow(typeof(PieceEditor), false, "Piece Editor");
            window.Show();
        }

        private static Rect ComputeTexCoords(Sprite sprite)
        {
            var rect = sprite.textureRect;
            var width = sprite.texture.width;
            var height = sprite.texture.height;

            return Rect.MinMaxRect(rect.xMin / width, rect.yMin / height, rect.xMax / width, rect.yMax / height);
        }

        private void OnGUI()
        {
            _database = (MapDatabase)EditorGUILayout.ObjectField("Map Database", _database, typeof(MapDatabase), false);

            EditorGUILayout.BeginHorizontal();
            var piece = _database.pieces[_database.selectIndex];
            piece.Width = Math.Clamp(EditorGUILayout.IntField("Width", piece.Width), 1, 10);
            piece.Height = Math.Clamp(EditorGUILayout.IntField("Height", piece.Height), 1, 10);
            EditorGUILayout.EndHorizontal();

            _tabIndex = GUILayout.Toolbar(_tabIndex, _tabStrings);

            switch (_tabIndex)
            {
                case 0:
                    _selectMineral = EditorGUILayout.Popup(_selectMineral,
                        _database.minerals.Select(mineral => mineral.mineralName).ToArray());                    Repaint();
                   
                    break;
                case 1:
                    _selectGround = EditorGUILayout.Popup(_selectGround,
                        _database.grounds.Select(ground => ground.groundName).ToArray());
                    break;
            }

            var e = Event.current;

            for (var x = 0; x < piece.Width; x++)
            {
                for (var y = 0; y < piece.Height; y++)
                {
                    var xMin = x * 36 + 8;
                    var yMin = y * 36 + 100;
                    var rect = Rect.MinMaxRect(xMin, yMin, xMin + 32, yMin + 32);
                    
                    EditorGUI.DrawRect(rect, Color.gray);

                    if (rect.Contains(e.mousePosition))
                    {
                        if (e.button == 0 && e.isMouse)
                        {
                            switch (_tabIndex)
                            {
                                case 0:
                                    piece.Bricks[x, y].mineral = _database.minerals[_selectMineral];
                                    break;
                                case 1:
                                    piece.Bricks[x, y].ground = _database.grounds[_selectGround];
                                    break;
                                case 2:
                                    piece.Pivot = new Vector2Int(x, y);
                                    break;
                            }
                            
                            EditorUtility.SetDirty(_database);
                            Repaint();
                        }
                    }

                    var ground = piece.Bricks[x, y].ground;
                    if (ground)
                    {
                        GUI.DrawTextureWithTexCoords(rect, ground.sprite.texture, ComputeTexCoords(ground.sprite));
                    }
                    
                    var mineral = piece.Bricks[x, y].mineral;
                    if (mineral)
                    {
                        GUI.DrawTextureWithTexCoords(rect, mineral.sprite.texture, ComputeTexCoords(mineral.sprite));
                    }
                    
                }
            }
            
            var pivotX = piece.Pivot.x * 36 + 8;
            var pivotY = piece.Pivot.y * 36 + 100;
            var pivotRect = Rect.MinMaxRect(pivotX, pivotY, pivotX + 32, pivotY + 32);
            
            EditorGUI.DrawRect(pivotRect, Color.blue.WithAlpha(0.1f));
        }
    }
}