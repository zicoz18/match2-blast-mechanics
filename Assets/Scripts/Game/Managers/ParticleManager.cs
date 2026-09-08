using System.Collections.Generic;
using Game.Core.Enums;
using Game.Core.ItemBase;
using UnityEngine;

namespace Game.Managers
{
    public class ParticleManager : MonoBehaviour, IProvidable
    {
        // TODO: Similar to rendering items, shouldnt we just have a single particle prefab and update it's "StartColor" property to have different versions? 
        [SerializeField] private ParticleSystem CubeYellowParticle;
        [SerializeField] private ParticleSystem CubeBlueParticle;
        [SerializeField] private ParticleSystem CubeGreenParticle;
        [SerializeField] private ParticleSystem CubeRedParticle;
        [SerializeField] private ParticleSystem ComboHintParticle;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public void PlayCubeParticle(Item item)
        {
            ParticleSystem particleSystemReference;

            switch (item.GetMatchType())
            {
                case MatchType.Green:
                    particleSystemReference = CubeGreenParticle;
                    break;
                case MatchType.Yellow:
                    particleSystemReference = CubeYellowParticle;
                    break;
                case MatchType.Blue:
                    particleSystemReference = CubeBlueParticle;
                    break;
                case MatchType.Red:
                    particleSystemReference = CubeRedParticle;
                    break;
                default:
                    return;
            }

            var particle = Instantiate(particleSystemReference, item.transform.position, Quaternion.identity,
                item.Cell.Board.ParticlesParent);
            particle.Play(true);
        }

        public ParticleSystem CreateComboHintParticle(Item item)
        {
            ParticleSystem particleSystemReference = ComboHintParticle;
            var particle = Instantiate(particleSystemReference, item.transform.position, Quaternion.identity, item.transform);
            particle.Play(true);
            return particle;
        }
    }
}