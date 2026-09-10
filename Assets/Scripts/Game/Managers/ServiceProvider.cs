using System;
using System.Collections.Generic;
using Game.Core.ItemBase;

namespace Game.Managers
{
    public static class ServiceProvider
    {
        private static readonly Dictionary<Type, IProvidable> RegisterDictionary = new Dictionary<Type, IProvidable>();

        public static T GetManager<T>() where T : class, IProvidable
        {
            if (RegisterDictionary.TryGetValue(typeof(T), out IProvidable manager))
            {
                return (T)manager;
            }

            throw new InvalidOperationException(
                $"{typeof(T).Name} was never registered with ServiceProvider. " +
                "Is the object that registers it present in this scene?");
        }

        public static T Register<T>(T target) where T : class, IProvidable
        {
            RegisterDictionary[typeof(T)] = target;
            return target;
        }

        public static ItemFactory GetItemFactory
        {
            get { return GetManager<ItemFactory>(); }
        }

        public static ParticleManager GetParticleManager
        {
            get { return GetManager<ParticleManager>(); }
        }

        public static ImageLibrary GetImageLibrary
        {
            get { return GetManager<ImageLibrary>(); }
        }

        public static LevelProgressManager GetLevelProgressManager
        {
            get { return GetManager<LevelProgressManager>(); }
        }
    }
}