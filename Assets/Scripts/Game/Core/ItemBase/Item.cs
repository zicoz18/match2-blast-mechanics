using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Mechanics;
using Game.Managers;
using UnityEngine;

namespace Game.Core.ItemBase
{
    public abstract class Item : MonoBehaviour
    {
        private const int BaseSortingOrder = 10;
        protected bool CanFall = true;

        public SpriteRenderer SpriteRenderer;
        public FallAnimation FallAnimation;
        public ItemType ItemType;

        private int _childSpriteOrder;

        private Cell _cell;
        protected int Health = 1;
        protected bool CanActivate = false;

        public Cell Cell
        {
            get { return _cell; }
            set
            {
                if (_cell == value) return;

                var oldCell = _cell;
                _cell = value;

                if (oldCell != null && oldCell.Item == this)
                {
                    oldCell.Item = null;
                }

                if (value != null)
                {
                    value.Item = this;
                    gameObject.name = _cell.gameObject.name + " " + GetType().Name;
                }

            }
        }

        public void Prepare(ItemBase itemBase, Sprite sprite, ItemType itemType)
        {
            SpriteRenderer = AddSprite(sprite);
            FallAnimation = itemBase.FallAnimation;
            FallAnimation.Item = this;
            ItemType = itemType;
        }

        public SpriteRenderer AddSprite(Sprite sprite)
        {
            var spriteRenderer = new GameObject("Sprite_" + _childSpriteOrder).AddComponent<SpriteRenderer>();
            spriteRenderer.transform.SetParent(transform);
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Item");
            spriteRenderer.sortingOrder = BaseSortingOrder + _childSpriteOrder++;

            return spriteRenderer;
        }

        public void RemoveSprite(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer == SpriteRenderer)
            {
                SpriteRenderer = null;
            }

            Destroy(spriteRenderer.gameObject);
        }

        public void UpdateSprite(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
        }

        public virtual MatchType GetMatchType()
        {
            return MatchType.None;
        }

        public bool IsFalling()
        {
            return FallAnimation.IsFalling;
        }

        public void TryFall()
        {
            if (CanFall)
            {
                FallAnimation.FallTo(Cell.GetFallTarget());
            }
        }

        public void Damage()
        {
            Health--;
            if (Health <= 0)
            {
                OnDeath();
            }
            else
            {
                OnNonLethalDamage();
            }
        }

        public virtual void OnDeath()
        {
            RemoveItem();
        }

        public virtual void OnNonLethalDamage()
        {
        }

        public virtual void TryMatchExecute()
        {
            Damage();
        }

        public virtual void TryNeighbourExecute(MatchType matchType)
        {
        }

        public virtual void TryBlastExecute()
        {
            Damage();
        }

        public bool GetCanActivate()
        {
            return CanActivate;
        }

        public virtual void Activate()
        {
        }

        public virtual void SetHint(int groupCount) { }

        public void RemoveItem()
        {
            ServiceProvider.GetLevelProgressManager.ItemTypeDestroyed(ItemType);

            Cell.Item = null;
            Cell = null;

            Destroy(gameObject);
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}