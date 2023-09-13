using System;
using System.Linq;
using Worlds;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WorldDatabaseEditor : EditorWindow
    {
        private readonly string[] _tabStrings =
        {
            "Wall", "Ground", "Mineral", "Pivot"
        };

        private int _tabIndex = 1;

        private WorldDatabase _database;

        private int _width = 2;

        private int _height = 2;

        private int _selectWall;

        private int _selectGround;

        private int _selectMineral;

        [MenuItem("Window/World Database Editor")]
        private static void Init()
        {
            var window = (WorldDatabaseEditor)GetWindow(typeof(WorldDatabaseEditor), false, "World Database Editor");
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
            if (_database is not null)
            {
                return;
            }

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
                _database = (WorldDatabase)EditorGUILayout.ObjectField(_database, typeof(WorldDatabase), false);
                return;
            }

            var piece = _database.pieces[_database.selectIndex];

            _database.selectIndex =
                EditorGUILayout.Popup(_database.selectIndex, _database.pieces.Select(piece => piece.name).ToArray());

            piece.appearanceArea = (WorldAreaFlag)EditorGUILayout.EnumFlagsField(piece.appearanceArea);

            EditorGUILayout.BeginHorizontal();
            _width = Math.Clamp(EditorGUILayout.IntField("Width", _width), 1, 16);
            _height = Math.Clamp(EditorGUILayout.IntField("Height", _height), 1, 16);

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
                    _selectWall = EditorGUILayout.Popup(_selectWall,
                        _database.walls.Select(ground => ground.displayName).ToArray());
                    break;
                case 1:
                    _selectGround = EditorGUILayout.Popup(_selectGround,
                        _database.grounds.Select(ground => ground.displayName).ToArray());
                    break;
                case 2:
                    _selectMineral = EditorGUILayout.Popup(_selectMineral,
                        _database.minerals.Select(mineral => mineral.displayName).ToArray());
                    Repaint();
                    break;
            }

            var e = Event.current;

            for (var x = 0; x < piece.width; x++)
            {
                for (var y = 0; y < piece.height; y++)
                {
                    var brick = piece.GetBrick(x, y);
                    var xMin = x * 36 + 8;
                    var yMin = piece.height * 36 - y * 36 + 100;
                    var rect = Rect.MinMaxRect(xMin, yMin, xMin + 32, yMin + 32);

                    EditorGUI.DrawRect(rect, Color.gray);

                    if (rect.Contains(e.mousePosition))
                    {
                        if (e.button == 0 && e.isMouse)
                        {
                            switch (_tabIndex)
                            {
                                case 0:
                                    brick.wall = _database.walls[_selectWall];
                                    break;
                                case 1:
                                    brick.ground = _database.grounds[_selectGround];
                                    break;
                                case 2:
                                    brick.mineral = _database.minerals[_selectMineral];
                                    break;
                                case 3:
                                    piece.pivot = new Vector2Int(x, y);
                                    break;
                            }

                            EditorUtility.SetDirty(_database);
                            Repaint();
                        }
                        else if (e.button == 1 && e.isMouse)
                        {
                            switch (_tabIndex)
                            {
                                case 0:
                                    brick.wall = null;
                                    break;
                                case 1:
                                    brick.ground = null;
                                    break;
                                case 2:
                                    brick.mineral = null;
                                    break;
                                case 3:
                                    piece.pivot = new Vector2Int(x, y);
                                    break;
                            }

                            EditorUtility.SetDirty(_database);
                            Repaint();
                        }
                    }

                    if (_tabIndex == 0)
                    {
                        var wall = brick.wall;
                        if (wall)
                        {
                            GUI.DrawTextureWithTexCoords(rect, wall.sprite.texture, ComputeTexCoords(wall.sprite));
                        }
                    }
                    else
                    {
                        var ground = brick.ground;
                        if (ground)
                        {
                            GUI.DrawTextureWithTexCoords(rect, ground.sprite.texture, ComputeTexCoords(ground.sprite));
                        }

                        var mineral = brick.mineral;
                        if (mineral)
                        {
                            GUI.DrawTextureWithTexCoords(rect, mineral.sprite.texture,
                                ComputeTexCoords(mineral.sprite));
                        }
                    }
                }
            }

            var pivotX = piece.pivot.x * 36 + 8;
            var pivotY = piece.height * 36 - piece.pivot.y * 36 + 100;
            var pivotRect = Rect.MinMaxRect(pivotX, pivotY, pivotX + 32, pivotY + 32);

            foreach (var prefab in piece.prefabs)
            {
                var prefabX = prefab.x * 36 + 8;
                var prefabY = piece.height * 36 - prefab.y * 36 + 100;
                EditorGUI.DrawRect(Rect.MinMaxRect(prefabX + 12, prefabY + 12, prefabX + 20, prefabY + 20),
                    Color.red);
            }

            EditorGUI.DrawRect(pivotRect, Color.blue.WithAlpha(0.1f));
        }
    }
}