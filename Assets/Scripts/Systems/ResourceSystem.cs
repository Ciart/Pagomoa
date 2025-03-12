using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

[Serializable]
public class JsonData<T>
{
    public T[] data;
}

// TODO: JsonData<T>로 통합해야 함
[Serializable]
public class BrickJsonData
{
    public Wall[] walls;

    public Ground[] grounds;

    public Mineral[] minerals;
}

// TODO: Json 파싱 방식을 변경하고 아래 record를 사용하도록 해야 함함
public record ItemId(string id);
public record WallId(string id);
public record GroundId(string id);
public record MineralId(string id);
public record EntityId(string id);

namespace Ciart.Pagomoa.Systems
{
    [ExecuteInEditMode]
    public class ResourceSystem : MonoBehaviour
    {
        private Dictionary<string, Item> items = new();

        private Dictionary<string, Wall> walls = new();

        private Dictionary<string, Ground> grounds = new();

        private Dictionary<string, Mineral> minerals = new();

        private Dictionary<string, Entity> entities = new();
        
        private Dictionary<string, List<QuestData>> matchQuests = new();

        public static ResourceSystem instance { get; private set; }

        private Dictionary<string, NewItemEffect> LoadItemEffects()
        {
            var result = new Dictionary<string, NewItemEffect>();

            foreach (var assembly in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (assembly.IsSubclassOf(typeof(NewItemEffect)))
                {
                    if (Activator.CreateInstance(assembly) is not NewItemEffect effect) continue;

                    result.Add(assembly.Name.Replace("Effect", ""), effect);
                }
            }

            return result;
        }

        private void LoadItems()
        {
            var text = Resources.Load<TextAsset>("Items");
            var itemEffects = LoadItemEffects();

            foreach (var item in JsonUtility.FromJson<JsonData<Item>>(text.ToString()).data)
            {
                items.Add(item.id, item);

                if (itemEffects.TryGetValue(item.id, out var effect))
                {
                    item.Init(effect);
                    continue;
                }

                item.Init();
            }
        }

        private void LoadBricks()
        {
            var text = Resources.Load<TextAsset>("Bricks");
            var data = JsonUtility.FromJson<BrickJsonData>(text.ToString());

            foreach (var wall in data.walls)
            {
                walls.Add(wall.id, wall);
                wall.Init();
            }

            foreach (var ground in data.grounds)
            {
                grounds.Add(ground.id, ground);
                ground.Init();
            }

            foreach (var mineral in data.minerals)
            {
                minerals.Add(mineral.id, mineral);
                mineral.Init();
            }
        }

        private void LoadEntities()
        {
            var text = Resources.Load<TextAsset>("Entities");

            foreach (var entity in JsonUtility.FromJson<JsonData<Entity>>(text.ToString()).data)
            {
                entities.Add(entity.id, entity);
                entity.Init();
            }
        }

        private void LoadMatchQuests()
        {
            var text = Resources.Load<TextAsset>("Quests");
            var data = JsonUtility.FromJson<MatchData>(text.text);
            var questData = Resources.LoadAll<QuestData>("Quests");
            var prevMatch = string.Empty;
            
            foreach (var match in data.data)
            {
                if (prevMatch != match.owner)
                    matchQuests.Add(match.owner, new List<QuestData>());
                prevMatch = match.owner;
                
                foreach (var quest in questData)
                {
                    if (quest.id == match.id)
                        matchQuests[match.owner].Add(quest);
                }
            }
        }

        public List<string> GetItemIds()
        {
            return items.Keys.ToList();
        }

        public List<string> GetWallIds()
        {
            return walls.Keys.ToList();
        }

        public List<string> GetGroundIds()
        {
            return grounds.Keys.ToList();
        }

        public List<string> GetMineralIds()
        {
            return minerals.Keys.ToList();
        }

        public List<string> GetEntityIds()
        {
            return entities.Keys.ToList();
        }

        public List<Item> GetItems()
        {
            return items.Values.ToList();
        }

        public List<Wall> GetWalls()
        {
            return walls.Values.ToList();
        }

        public List<Ground> GetGrounds()
        {
            return grounds.Values.ToList();
        }

        public List<Mineral> GetMinerals()
        {
            return minerals.Values.ToList();
        }

        public List<Entity> GetEntities()
        {
            return entities.Values.ToList();
        }

        public Item GetItem(string id)
        {
            if (items.TryGetValue(id, out var item))
            {
                return item;
            }

            throw new Exception($"ResourceSystem: GetItem - '{id}' is not found");
        }

        public Wall GetWall(string id)
        {
            if (walls.TryGetValue(id, out var wall))
            {
                return wall;
            }

            throw new Exception($"ResourceSystem: GetWall - '{id}' is not found");
        }

        public Ground GetGround(string id)
        {
            if (grounds.TryGetValue(id, out var ground))
            {
                return ground;
            }

            throw new Exception($"ResourceSystem: GetGround - '{id}' is not found");
        }

        public Mineral GetMineral(string id)
        {
            if (minerals.TryGetValue(id, out var mineral))
            {
                return mineral;
            }

            throw new Exception($"ResourceSystem: GetMineral - '{id}' is not found");
        }

        public Entity GetEntity(string id)
        {
            if (entities.TryGetValue(id, out var entity))
            {
                return entity;
            }

            throw new Exception($"ResourceSystem: GetEntity - '{id}' is not found");
        }

        public QuestData[]? GetQuests(string ownerId)
        {
            if (matchQuests.TryGetValue(ownerId, out var quests))
            {
                return quests.ToArray();
            }

            return null;
        }

        public QuestData GetQuest(string questId)
        {
            foreach (var quests in matchQuests.Values)
            {
                foreach (var quest in quests)
                {
                    if (quest.id == questId)
                    {
                        return quest;
                    }
                }
            }

            throw new Exception($"ResourceSystem: GetQuest - '{questId}' is not found");
        }
        
        private void Init()
        {
            items = new Dictionary<string, Item>();
            walls = new Dictionary<string, Wall>();
            grounds = new Dictionary<string, Ground>();
            minerals = new Dictionary<string, Mineral>();
            entities = new Dictionary<string, Entity>();
            matchQuests = new Dictionary<string, List<QuestData>>();

            LoadItems();
            LoadBricks();
            LoadEntities();
            LoadMatchQuests();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (instance != this)
            {
                Destroy(gameObject);
            }

            Init();
        }

        private void OnValidate()
        {
            Init();
        }
    }
}
