using System;
using System.Linq;
using Ciart.Pagomoa.Worlds;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Ciart.Pagomoa.Editor
{
    public class WorldDatabaseEditor : EditorWindow
    {
        private readonly string[] _tabStrings =
        {
            "Pivot", "Wall", "Ground", "Mineral", "Entity"
        };

        private int _tabIndex = 1;

        private WorldDatabase _database;

        private int _width = 2;

        private int _height = 2;

        private int _selectWall;

        private int _selectGround;

        private int _selectMineral;

        private int _selectEntity;

        private Vector2 _workspaceScrollPosition;

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

        private void DrawSideBar()
        {
            const float width = 300;

            _database = (WorldDatabase)EditorGUILayout.ObjectField(_database, typeof(WorldDatabase), false,
                GUILayout.Width(width));

            if (_database is null)
            {
                return;
            }

            var piece = _database.pieces[_database.selectIndex];

            piece.appearanceArea =
                (WorldAreaFlag)EditorGUILayout.EnumFlagsField(piece.appearanceArea, GUILayout.Width(width));
            
            GUILayout.Label($"현재: {piece.width}x{piece.height}");

            _width = Math.Clamp(EditorGUILayout.IntField("Width", _width, GUILayout.Width(width)), 1, 1000);
            _height = Math.Clamp(EditorGUILayout.IntField("Height", _height, GUILayout.Width(width)), 1, 1000);

            if (GUILayout.Button("Resize"))
            {
                piece.ResizeBricks(_width, _height);
            }
            
            GUILayout.Space(8);

            _tabIndex = GUILayout.Toolbar(_tabIndex, _tabStrings, GUILayout.Width(width));

            switch (_tabIndex)
            {
                case 1:
                    _selectWall = EditorGUILayout.Popup(_selectWall,
                        _database.walls.Select(wall => wall.displayName ?? wall.name).ToArray(),
                        GUILayout.Width(width));
                    break;
                case 2:
                    _selectGround = EditorGUILayout.Popup(_selectGround,
                        _database.grounds.Select(ground => ground.displayName ?? ground.name).ToArray(),
                        GUILayout.Width(width));
                    break;
                case 3:
                    _selectMineral = EditorGUILayout.Popup(_selectMineral,
                        _database.minerals.Select(mineral => mineral.displayName ?? mineral.name).ToArray(),
                        GUILayout.Width(width));
                    break;
                case 4:
                    _selectEntity = EditorGUILayout.Popup(_selectEntity,
                        _database.entities.Select(entity => entity.displayName ?? entity.name).ToArray(),
                        GUILayout.Width(width));
                    break;
            }
            
            GUILayout.Space(8);
            
            _database.selectIndex = GUILayout.SelectionGrid(_database.selectIndex, _database.pieces.Select(p => p.name).ToArray(), 1);
        }

        private void DrawWorkspace()
        {
            var piece = _database.pieces[_database.selectIndex];

            if (!piece.isValid)
            {
                GUILayout.Label("크기가 유효하지 않습니다. 재설정 해주세요.");
                return;
            }

            var e = Event.current;

            for (var x = 0; x < piece.width; x++)
            {
                for (var y = 0; y < piece.height; y++)
                {
                    var brick = piece.GetBrick(x, y);
                    var xMin = x * 36 + 8;
                    var yMin = piece.height * 36 - (y + 1) * 36 + 8;
                    var rect = Rect.MinMaxRect(xMin, yMin, xMin + 32, yMin + 32);

                    EditorGUI.DrawRect(rect, Color.gray);

                    if (rect.Contains(e.mousePosition))
                    {
                        if (e.button == 0 && e.isMouse)
                        {
                            switch (_tabIndex)
                            {
                                case 0:
                                    piece.pivot = new Vector2Int(x, y);
                                    break;
                                case 1:
                                    brick.wall = _database.walls[_selectWall];
                                    break;
                                case 2:
                                    brick.ground = _database.grounds[_selectGround];
                                    break;
                                case 3:
                                    brick.mineral = _database.minerals[_selectMineral];
                                    break;
                                case 4:
                                    // TODO: Piece에서는 int 좌표를 사용하는게 좋을 듯 합니다.
                                    piece.AddEntity(x, y, _database.entities[_selectEntity]);
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
                                    piece.pivot = new Vector2Int(x, y);
                                    break;
                                case 1:
                                    brick.wall = null;
                                    break;
                                case 2:
                                    brick.ground = null;
                                    break;
                                case 3:
                                    brick.mineral = null;
                                    break;
                            }

                            EditorUtility.SetDirty(_database);
                            Repaint();
                        }
                    }

                    if (_tabIndex == 1)
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
            var pivotY = piece.height * 36 - (piece.pivot.y + 1) * 36;
            var pivotRect = Rect.MinMaxRect(pivotX, pivotY, pivotX + 32, pivotY + 32);

            foreach (var prefab in piece.entities)
            {
                var prefabX = prefab.x * 36 + 8;
                var prefabY = piece.height * 36 - (prefab.y + 1) * 36;
                EditorGUI.DrawRect(Rect.MinMaxRect(prefabX + 12, prefabY + 12, prefabX + 20, prefabY + 20),
                    Color.red);
            }

            EditorGUI.DrawRect(pivotRect, Color.blue.WithAlpha(0.1f));
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            _workspaceScrollPosition = EditorGUILayout.BeginScrollView(_workspaceScrollPosition, true, true);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(1000);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(1000);
            DrawWorkspace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(50));
            DrawSideBar();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}