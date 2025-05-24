using System;
using System.Linq;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
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

        private string _selectWallId;

        private string _selectGroundId;

        private string _selectMineralId;

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

            var resourceManager = ResourceSystem.Instance;

            if (resourceManager is null)
            {
                return;
            }

            var walls = resourceManager.GetWalls();
            var grounds = resourceManager.GetGrounds();
            var minerals = resourceManager.GetMinerals();

            switch (_tabIndex)
            {
                case 1:
                    var wallIndex = EditorGUILayout.Popup(walls.FindIndex((wall) => wall.id == _selectWallId),
                        walls.Select(wall => wall.name ?? wall.name).ToArray(),
                        GUILayout.Width(width));
                    _selectWallId = wallIndex != -1 ? walls[wallIndex].id : "";
                    break;
                case 2:
                    var groundIndex = EditorGUILayout.Popup(grounds.FindIndex((ground) => ground.id == _selectGroundId),
                        grounds.Select(ground => ground.name ?? ground.name).ToArray(),
                        GUILayout.Width(width));
                    _selectGroundId = groundIndex != -1 ? grounds[groundIndex].id : "";
                    break;
                case 3:
                    var mineralIndex = EditorGUILayout.Popup(
                        minerals.FindIndex((mineral) => mineral.id == _selectMineralId),
                        minerals.Select(mineral => mineral.name ?? mineral.name).ToArray(),
                        GUILayout.Width(width));
                    _selectMineralId = mineralIndex != -1 ? minerals[mineralIndex].id : "";
                    break;
                // case 4:
                //     _selectEntity = EditorGUILayout.Popup(_selectEntity,
                //         _database.entities.Select(entity => entity.name ?? ((Object)entity).name).ToArray(),
                //         GUILayout.Width(width));
                //     break;
            }

            GUILayout.Space(8);

            _database.selectIndex =
                GUILayout.SelectionGrid(_database.selectIndex, _database.pieces.Select(p => p.name).ToArray(), 1);
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
                                    brick.wallId = _selectWallId;
                                    break;
                                case 2:
                                    brick.groundId = _selectGroundId;
                                    break;
                                case 3:
                                    brick.mineralId = _selectMineralId;
                                    break;
                                // case 4:
                                //     // TODO: Piece에서는 int 좌표를 사용하는게 좋을 듯 합니다.
                                //     piece.AddEntity(x, y, _database.entities[_selectEntity]);
                                //     break;
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
                                    brick.wallId = null;
                                    break;
                                case 2:
                                    brick.groundId = null;
                                    break;
                                case 3:
                                    brick.mineralId = null;
                                    break;
                            }

                            EditorUtility.SetDirty(_database);
                            Repaint();
                        }
                    }

                    if (_tabIndex == 1)
                    {
                        var wall = brick.wall;
                        if (wall != null)
                        {
                            var sprite = wall.sprite;
                            GUI.DrawTextureWithTexCoords(rect, sprite.texture, ComputeTexCoords(sprite));
                        }
                    }
                    else
                    {
                        var ground = brick.ground;
                        if (ground != null)
                        {
                            var sprite = ground.sprite;
                            GUI.DrawTextureWithTexCoords(rect, sprite.texture, ComputeTexCoords(sprite));
                        }

                        var mineral = brick.mineral;
                        if (mineral != null)
                        {
                            var sprite = mineral.sprite;
                            GUI.DrawTextureWithTexCoords(rect, sprite.texture, ComputeTexCoords(sprite));
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

            var color = Color.blue;
            color.a = 0.1f;

            EditorGUI.DrawRect(pivotRect, color);
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
