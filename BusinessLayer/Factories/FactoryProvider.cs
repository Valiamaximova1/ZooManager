using BusinessLayer.DTOs;
using BusinessLayer.Factories.Interfaces;

namespace BusinessLayer.Factories
{
    public class FactoryProvider
    {
        private readonly Dictionary<Type, object> _factories = new();

        public FactoryProvider()
        {
            _factories[typeof(AnimalDto)] = new AnimalFactory();
            _factories[typeof(EventDto)] = new EventFactory();
        }

        public IFactory<T> GetFactory<T>()
        {
            return (IFactory<T>)_factories[typeof(T)];
        }
    }

}
