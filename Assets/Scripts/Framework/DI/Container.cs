using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Pools;
using UnityEngine;
using UnityEngine.Pool;

namespace Framework.DI
{
    public sealed class Container : IDisposable
    {
        private readonly Dictionary<Type, object> _registrations = new();
        private readonly List<IDisposable> _disposables = new();
        
        public Container Parent { get; private set; }

        public Container(Container parent = null)
        {
            Parent = parent;
            Parent?.AddDisposable(this);
        }

        public void Register<T>(T value)
        {
            var type = typeof(T);
            if (!_registrations.TryAdd(type, value))
                throw new ArgumentException("Trying to register already registered type!");
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            object value = Resolve(type);
            return value != null ? (T)value : default;
        }

        // Methods for automatic injection
        public object Resolve(Type type)
        {
            if (_registrations.TryGetValue(type, out object value))
                return value;

            return Parent?.Resolve(type);
        }
        
        public bool HasRegistration(Type type) => _registrations.ContainsKey(type) || (Parent != null && Parent.HasRegistration(type));

        private bool TryResolve(Type type, out object value)
        {
            value = default;
            if (!HasRegistration(type))
                return false;
            
            value = Resolve(type);
            return true;
        }

        public void AddDisposable(IDisposable disposable) => _disposables.Add(disposable);
        public void RemoveDisposable(IDisposable disposable) => _disposables.Remove(disposable);
        
        public void Dispose()
        {
            foreach (var registration in _registrations)
            {
                if (registration.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            _registrations.Clear();
            
            using var tempDisposablesPooled = ListPool<IDisposable>.Get(out var tempDisposables);
            tempDisposables.AddRange(_disposables);
            foreach (var disposable in tempDisposables)
            {
                disposable?.Dispose();
            }
            
            _disposables.Clear();
            Parent?.RemoveDisposable(this);
        }
        
        public void InjectToInstance(object instance)
        {
            var type = instance.GetType();

            while (type != null && !type.IsInterface)
            {
                InjectFields(instance, type);
                InjectMethods(type, instance);
                type = type.BaseType;
            }
        }

        public T Create<T>()
        {
            var type = typeof(T);
            var instance = Create(type);
            return (T)instance;
        }

        public T CreateAndRegister<T>()
        {
            var type = typeof(T);
            var instance = Create(type);
            var typedInstance = (T)instance;
            Register(typedInstance);
            return typedInstance;
        }

        public object Create(Type type)
        {
            var constructors = type.GetConstructors()
                .Where(x => x.IsDefined(typeof(InjectAttribute)));

            object instance;
            var constructor = constructors.FirstOrDefault();
            if (constructor == null)
            {
                instance = Activator.CreateInstance(type);
                InjectToInstance(instance);
                if (instance is IInitializable initializable)
                    initializable.Initialize();
                
                return instance;
            }

            var parameters = constructor.GetParameters();
            object[] parametersValues = ArrayPool<object>.New(parameters.Length);
            
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                if (!TryResolve(parameterType, out object value))
                    throw new ArgumentException(
                        $"Cannot create instance of {type.Name}: its first [Inject] constructor has unknown dependency: {parameterType.Name}");

                parametersValues[i] = value;
            }

            instance = constructor.Invoke(parametersValues);
            InjectToInstance(instance);
            {
                if (instance is IInitializable initializable)
                    initializable.Initialize();
            }
            
            return instance;
        }
        
        // Method for instantiating prefab and passing all [Inject] fields and methods in it
        public T InstantiateAndResolve<T>(T prefab) where T : MonoBehaviour
        {
            var spawnedObject = PrefabMonoPool<T>.GetPrefabInstance(prefab);
            InjectToInstance(spawnedObject);
            if (spawnedObject is IInitializable initializable)
                initializable.Initialize();
            
            return spawnedObject;
        }
        
        public T InstantiateAndResolve<T>(T prefab, Transform parent) where T : MonoBehaviour
        {
            var spawnedObject = PrefabMonoPool<T>.GetPrefabInstanceForParent(prefab, parent);
            InjectToInstance(spawnedObject);
            if (spawnedObject is IInitializable initializable)
                initializable.Initialize();
            
            return spawnedObject;
        }

        public T CreateGameObjectWithComponent<T>(string name) where T : MonoBehaviour
        {
            var gameObject = new GameObject(name);
            var instance = gameObject.AddComponent<T>();
            InjectToInstance(instance);
            if (instance is IInitializable initializable)
                initializable.Initialize();
            
            return instance;
        }

        private void InjectFields(object spawnedObject, Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsDefined(typeof(InjectAttribute)));
            
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                if (!TryResolve(fieldType, out object value))
                {
                    throw new ArgumentException($"Cannot inject type {type.Name}! There's no registration for {fieldType.Name}!");
                }
                
                field.SetValue(spawnedObject, value);
            }
        }

        private void InjectMethods(Type type, object spawnedObject)
        {
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.IsDefined(typeof(InjectAttribute)));
            
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var parametersValues = ArrayPool<object>.New(parameters.Length);
                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        var parameterType = parameter.ParameterType;
                        
                        if (!TryResolve(parameterType, out object value))
                        {
                            throw new ArgumentException($"Cannot inject type {type.Name}! There's no registration for {parameterType.Name}!");
                        }

                        parametersValues[i] = value;
                    }
                }

                method.Invoke(spawnedObject, parametersValues);
                ArrayPool<object>.Free(parametersValues);
            }
        }
    }
}