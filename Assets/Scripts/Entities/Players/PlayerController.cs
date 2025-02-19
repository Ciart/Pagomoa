using System;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Entities.Players
{
    [RequireComponent(typeof(PlayerStatus))]
    public partial class PlayerController : EntityController
    {
        public PlayerState state = PlayerState.Idle;

        public bool isGrounded = false;

        public PlayerStatus status;

        public PlayerStatus initialStatus;

        public float groundDistance = 0.125f;

        public float sideWallDistance = 1.0625f;

        public Inventory inventory;

        public DrillController drill;

        public EntityController entityController;

        private Rigidbody2D _rigidbody;

        private PlayerInput _input;

        private PlayerMovement _movement;
        
        private Camera _camera;

        private WorldManager _world;

        private Direction _direction;

        private void Awake()
        {
            status = GetComponent<PlayerStatus>();
            initialStatus = status.copy();
            drill = GetComponentInChildren<DrillController>();
            entityController = GetComponent<EntityController>();

            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
            inventory = GetComponent<Inventory>();
            _camera = Camera.main;
            _world = Game.Instance.World;

            inventory.artifactChanged += OnArtifactChanged;
        }

        private void TryJump()
        {
            if (!_input.IsJump || !isGrounded || state is PlayerState.Climb or PlayerState.Fall or PlayerState.Jump)
            {
                return;
            }

            state = PlayerState.Jump;
            _movement.Jump();
        }

        private void OnChangedState(PlayerState state)
        {
            if (state == PlayerState.Fall)
            {
                _movement.Fall();
            }
        }

        private void Update()
        {
            UpdateState();
            UpdateSound();

            _movement.isClimb = state == PlayerState.Climb;
            _movement.directionVector = _input.Move;

            _direction = DirectionUtility.ToDirection(_camera.ScreenToWorldPoint(_input.Look) - transform.position);

            if (_input.IsDig && state != PlayerState.Climb)
            {
                drill.isDig = true;
                drill.direction = _direction;
            }
            else if (_input.DigDirection.magnitude > 0.001f)
            {
                drill.isDig = true;
                drill.direction = DirectionUtility.ToDirection(_input.DigDirection);
            }
            else
            {
                drill.isDig = false;
            }

            TryJump();

            EventManager.Notify(new PlayerMove(transform.position));
        }

        private void UpdateIsGrounded()
        {
            var position = transform.position;
            position.y -= 1f; // TODO: 플레이어 영점 변경하면서 수정해야 함.
            var hit = Physics2D.Raycast(position, Vector2.down, groundDistance, LayerMask.GetMask("Platform"));
            Debug.DrawRay(position, Vector2.down * groundDistance, Color.green);

            isGrounded = (bool)hit.collider;
        }

        private void UpdateIsSideWall()
        {
            var directionVector = _input.Move.x switch
            {
                <= -0.0001f => Vector2.left,
                >= 0.0001f => Vector2.right,
                _ => Vector2.zero
            };

            if (directionVector == Vector2.zero)
            {
                _movement.isSideWall = false;
                return;
            }

            var position = transform.position;

            var hit = Physics2D.Raycast(position, directionVector, sideWallDistance,
                LayerMask.GetMask("Platform"));
            Debug.DrawRay(position, directionVector * sideWallDistance, Color.green);

            if (!hit.collider || hit.collider.transform.position.y > 0f)
            {
                _movement.isSideWall = false;
                return;
            }

            _movement.isSideWall = true;
        }

        public bool Hungry(float value)
        {
            if (status.hungry - value < 0) return true;
            status.hungry -= value;
            status.hungryAlter.Invoke(status.hungry, status.maxHungry);
            return false;
        }

        private void FixedUpdate()
        {
            UpdateIsGrounded();
            UpdateIsSideWall();
        }

        public Direction GetDirection()
        {
            return _direction;
        }

        private void Respawn()
        {
            transform.position = FindAnyObjectByType<SpawnPoint>().transform.position;

            status.oxygen = status.maxOxygen;
        }

        private void LoseMoney(float percentage)
        {
            inventory.gold = (int)(inventory.gold * (1 - percentage));
        }

        // TODO : 사망 시 아이템 제거 기능 잠금 
        private void LoseItem(ItemType itemType, float probabilty)
        {
            /*List<string> deleteItems = new List<string>();*/

            foreach (var slot in inventory.GetSlots(SlotType.Inventory))
            {
                var item = slot.GetSlotItem();

                if (item is null) continue;

                var rand = Random.Range(0, 101) * 0.01f;
                if (probabilty < rand)
                {
                    Debug.Log("item not Losted by" + probabilty + "<" + rand);
                    continue;
                }

                if (item.type == itemType)
                {
                    for (int i = 0; i < slot.GetSlotItemCount(); i++)
                    {
                        var entity = Instantiate(Game.Instance.World.itemEntity, transform.position,
                            Quaternion.identity);
                        entity.Item = item;
                        entity.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5, 5), 100));
                    }

                    /*deleteItems.Add(item.GetSlotItem().id);*/
                }
            }

            /*var count = deleteItems.Count;
            for (int i = 0; i < count; i++)
                inventory.RemoveInventoryItem(ResourceSystem.instance.GetItem(deleteItems[i]));*/
        }

        private void OnArtifactChanged()
        {
            var slots = inventory.GetSlots(SlotType.Artifact);
            var statusModifier = new PlayerStatusModifier();

            foreach (var slot in slots)
            {
                var item = slot.GetSlotItem();
                
                if (item is null) continue;

                if (item.status is null) continue;

                statusModifier += item.status;
            }

            var entity = ResourceSystem.instance.GetEntity(entityController.entityId);

            maxHealth = (entity.baseHealth + statusModifier.health) * statusModifier.healthMultiplier;
            attack = (entity.attack + statusModifier.attack) * statusModifier.attackMultiplier;
            defense = (entity.defense + statusModifier.defense) * statusModifier.defenseMultiplier;
            speed = (entity.speed + statusModifier.speed) * statusModifier.speedMultiplier;
        }

        private void OnDied(EntityDiedEventArgs e)
        {
            e.AutoDespawn = false;

            LoseMoney(0.1f);
            LoseItem(ItemType.Mineral, 0.5f);

            Game.Instance.UI.ShowDaySummaryUI();
            Respawn();
        }

        private void OnEnable()
        {
            entityController.died += OnDied;
        }

        private void OnDisable()
        {
            entityController.died -= OnDied;
        }
    }
}