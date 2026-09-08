using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Managers;
using UnityEngine;

namespace Game.Core.ItemBase
{
    public abstract class SpecialItem : Item
    {
        private ParticleSystem _hintParticle;
        protected abstract List<Cell> GetBlastArea();
        protected abstract Sprite GetSprite();

        public void PrepareSpecialItem(ItemBase itemBase)
        {
            CanActivate = true;
            Prepare(itemBase, GetSprite());
        }


        public override void Activate()
        {
            // Capture the area first, then remove self BEFORE propagating:
            // self-removal is what terminates chains, and it's also what keeps
            // a blast area that includes our own cell from re-entering us.
            List<Cell> blastArea = GetBlastArea();
            Damage();
            foreach (var cell in blastArea)
            {
                if (cell.HasItem()) cell.Item.TryBlastExecute();
            }
        }

        public override void TryBlastExecute() => Activate();

        public override MatchType GetMatchType()
        {
            return MatchType.Special;
        }

        public override void SetHint(int groupCount)
        {
            bool shouldShow = groupCount >= Board.MinimumComboCount;

            if (shouldShow && _hintParticle == null)
            {
                _hintParticle = ServiceProvider.GetParticleManager.CreateComboHintParticle(this);
            }
            else if (!shouldShow && _hintParticle != null)
            {
                DestroyParticle();
            }
        }

        private void DestroyParticle()
        {
            Destroy(_hintParticle.gameObject);
            _hintParticle = null;
        }
    }
}