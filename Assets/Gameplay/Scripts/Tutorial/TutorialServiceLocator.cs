using System;
using System.Collections.Generic;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialServiceLocator
    {
        private readonly Dictionary<Type, object> _servicesContainer;
        
        public TutorialServiceLocator()
        {
            _servicesContainer = new Dictionary<Type, object>();
        }

        public void SetService<T>(T service) where T : class
        {
            var type = typeof(T);
            _servicesContainer[type] = service;
        }
        
        public void SetService(Type type, object service)
        {
            _servicesContainer[type] = service;
        }

        public T GetService<T>()
        {
            var type = typeof(T);

            try
            {
                return (T)_servicesContainer[type];
            }
            catch (Exception e)
            {
                throw new Exception($"Cant get service {type}\n{e.Message}");
            }
        }
    }
}