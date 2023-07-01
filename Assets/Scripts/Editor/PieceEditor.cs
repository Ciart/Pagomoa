using System;
using System.Linq;
using Worlds;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PieceEditor : EditorWindow
    {
        private readonly string[] _tabStrings =
        {
            "Mineral", "Ground", "Pivot", "Wall"
        };

        private int _tabIndex;

        private WorldDatabase _database;

        private int _width = 2;

        private int _height = 2;

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

        private void OnSelectionChange()
        {
            if (Selection.activeObject is not WorldDatabase)
            {
                _database = null;
                return;
            }

            _database = (WorldDatabase)Selection.activeObject;

            Repaint();
        }

        private void OnGUI()
        {
            if (_database is null)
            {
                return;
            }

            var piece = _database.pieces[_database.selectIndex];

            EditorGUILayout.BeginHorizontal();
            _width = Math.Clamp(EditorGUILayout.IntField("Width", _width), 1, 10);
            _height = Math.Clamp(EditorGUILayout.IntField("Height", _height), 1, 10);

            if (GUILayout.Button("Resize"))
            {
                piece.width = _width;
                piece.height = _height;
                piece.ResizeBricks();
            }

            EditorGUILayout.EndHorizontal();

            _tabIndex = GUILayout.Toolbar(_tabIndex, _tabStrings);

            switch (_tabIndex)
            {
                case 0:
                    _selectMineral = EditorGUILayout.Popup(_selectMineral,
                        _database.minerals.Select(mineral => mineral.mineralName).ToArray());
                    Repaint();
                    break;
                case 1:
                    _selectGround = EditorGUILayout.Popup(_selectGround,
                        _database.grounds.Select(ground => ground.groundName).ToArray());
                    break;
            }

            var e = Event.current;

            for (var x = 0; x < piece.width; x++)
            {
                for (var y = 0; y < piece.height; y++)
                {
                    var brick = piece.GetBrick(x, y);
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
                                    brick.mineral = _database.minerals[_selectMineral];
                                    break;
                                case 1:
                                    brick.ground = _database.grounds[_selectGround];
                                    break;
                                case 2:
                                    piece.pivot = new Vector2Int(x, y);
                                    break;
                            }

                            EditorUtility.SetDirty(_database);
                            Repaint();
                        }
                    }

                    var ground = brick.ground;
                    if (ground)
                    {
                        GUI.DrawTextureWithTexCoords(rect, ground.sprite.texture, ComputeTexCoords(ground.sprite));
                    }

                    var mineral = brick.mineral;
                    if (mineral)
                    {
                        GUI.DrawTextureWithTexCoords(rect, mineral.sprite.texture, ComputeTexCoords(mineral.sprite));
                    }
                }
            }

            var pivotX = piece.pivot.x * 36 + 8;
            var pivotY = piece.pivot.y * 36 + 100;
            var pivotRect = Rect.MinMaxRect(pivotX, pivotY, pivotX + 32, pivotY + 32);

            EditorGUI.DrawRect(pivotRect, Color.blue.WithAlpha(0.1f));
        }
    }
}