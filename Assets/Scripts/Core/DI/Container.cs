﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityDI.Providers;
using UnityDI.Providers.UnityDI.Providers;

namespace UnityDI {
    /// <summary>
    /// Класс DI-контейнера
    /// </summary>
    public class Container {
        public Container ParentContainer;

        public event Action<string> OnErrorLog = (m) => { };
        public event Action<Exception> OnInjectedException = (m) => { };

        public bool IgnoreResolveErrors;

        private readonly Dictionary<ContainerKey, IProviderWrapper> _providers = new Dictionary<ContainerKey, IProviderWrapper>();

        /// <summary>
        /// Зарегистрировать тип
        /// При каждом обращении Resolve&lt;T&gt;() будет создаваться новый объект типа T
        /// </summary>
        public Container RegisterType<T>(string name = null) where T : class, new() {
            return RegisterProvider(new ActivatorObjectProvider<T>(), name);
        }

        /// <summary>
        /// Зарегистрировать тип под видом базового
        /// При каждом обращении Resolve&lt;TBase&gt;() будет создаваться новый объект TDerived
        /// </summary>
        public Container RegisterType<TBase, TDerived>(string name = null) where TDerived : class, TBase, new() {
            return RegisterProvider<TBase, TDerived>(new ActivatorObjectProvider<TDerived>(), name);
        }

        /// <summary>
        /// Зарегистрировать синглтон
        /// При каждом обращении Resolve&lt;T&gt;() будет возвращаться ссылка на один и тот же объект типа T
        /// </summary>
        public Container RegisterSingleton<T>(string name = null) where T : class, new() {
            return RegisterProvider(new SingletonProvider<T>(), name);
        }

        /// <summary>
        /// Зарегистрировать синглтон
        /// При каждом обращении Resolve&lt;TBase&gt;() будет возвращаться ссылка на один и тот же объект типа TDerived
        /// </summary>
        public Container RegisterSingleton<TBase, TDerived>(string name = null) where TDerived : class, TBase, new() {
            return RegisterProvider<TBase, TDerived>(new SingletonProvider<TDerived>(), name);
        }

        /// <summary>
        /// Зарегистрировать объект типа T
        /// При каждом обращении Resolve&lt;T&gt;() будет возвращаться ссылка на переданный объект
        /// </summary>
        public Container RegisterInstance<T>(T obj, string name = null) where T : class {
            return RegisterProvider(new InstanceProvider<T>(obj), name);
        }

        /// <summary>
        /// Зарегистрировать объект типа TDerived
        /// При каждом обращении Resolve&lt;TBase&gt;() будет возвращаться ссылка на переданный объект
        /// </summary>
        public Container RegisterInstance<TBase, TDerived>(TDerived obj, string name = null) where TDerived : class, TBase {
            return RegisterProvider<TBase, TDerived>(new InstanceProvider<TDerived>(obj), name);
        }

        public Container RegisterCustom<T>(Func<T> provider, string name = null) where T : class {
            return RegisterProvider(new CustomProvider<T>(provider), name);
        }

        public Container RegisterCustom<TBase, TDerived>(Func<TDerived> provider, string name = null)
            where TDerived : class, TBase {
            return RegisterProvider<TBase, TDerived>(new CustomProvider<TDerived>(provider), name);
        }


        public Container RegisterProvider<T>(IObjectProvider<T> provider, string name = null) where T : class {
            var key = new ContainerKey(typeof(T), name);
            _providers[key] = new ProviderWrapper<T>(provider);
            return this;
        }

        public Container RegisterTypeProviderWithInheritance(Type type, IProviderWrapper provider, string name = null) {
            for (var t = type; t != null && t != typeof(object); t = t.BaseType) {
                var key = new ContainerKey(t, name);
                _providers[key] = provider;
            }

            return this;
        }

        public Container RegisterTypeProvider(Type type, IProviderWrapper provider, string name = null) {
            var key = new ContainerKey(type, name);
            _providers[key] = provider;
            return this;
        }

        public Container RegisterProvider<TBase, TDerived>(IObjectProvider<TDerived> provider, string name = null) where TDerived : class, TBase {
            var key = new ContainerKey(typeof(TBase), name);
            _providers[key] = new ProviderWrapper<TDerived>(provider);
            return this;
        }

        /// <summary>
        /// Получить объект нужного типа
        /// </summary>
        public T Resolve<T>(string name = null) {
            return (T)Resolve(typeof(T), name);
        }

        public bool IsRegistred<T>(string name = null) {
            IProviderWrapper provider;
            return _providers.TryGetValue(new ContainerKey(typeof(T), name), out provider);
        }

        /// <summary>
        /// Получить объект нужного типа
        /// </summary>
        public object Resolve(Type type, string name = null) {
            IProviderWrapper provider;
            var key = new ContainerKey();
            key.Type = type;
            key.Name = name;
            if (!_providers.TryGetValue(key, out provider)) {
                if (ParentContainer != null) {
                    return ParentContainer.Resolve(type, name);
                }

                //throw new ContainerException("Can't resolve type " + type.FullName + (name == null ? "" : " registered with name \"" + name + "\""));
                if (IgnoreResolveErrors)
                    return null;
                var msg = "Cannot resolve dependency for type " + type.Name;
                OnErrorLog(msg);
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning(msg);
#endif
                return null;
            }
            try {
                return provider.GetObject(this);
            }
            catch (Exception e) {
                var msg = "Cannot resolve dependency for type " + type.Name + " " + e.Message + "\n" + e.StackTrace;
                OnErrorLog(msg);
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning(msg);
#endif
                return null;
            }
        }

        public bool Unregister<T>(string name = null) {
            return Unregister(typeof(T), name);
        }

        public bool Unregister(Type type, string name = null) {
            return _providers.Remove(new ContainerKey(type, name));
        }

        public void BuildUp(Type type, object obj) {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

            FieldInfo[] fields = type.GetFields(flags);

            //This is much more faster, but we will have a problem with inherited members.
            //Consider to uncomment it and write more complicated logic if there will be problems with perfomance
            //FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields) {
                var attrs = field.GetCustomAttributes(typeof(DependencyAttribute), true);
                if (!attrs.Any())
                    continue;

                var attrib = (DependencyAttribute)attrs[0];
                object valueObj;

                try {
                    valueObj = Resolve(field.FieldType, attrib.Name);

                    /*
                    //Obsolete code to use [Dependency] instead of [SerializeField]
                    MonoBehaviour component = obj as MonoBehaviour;
                    if (valueObj == null && component != null)
                    {
                        if (field.FieldType.IsSubclassOf(typeof(Component)))
                        {
                            valueObj = component.GetComponent(field.FieldType);
                        }
                        else
                        {
                            Debug.LogWarning("Can't resolve dependency: " + field.FieldType + " in " + obj);
                        }
                    }
                    */
                }
                catch (ContainerException ex) {
                    throw new ContainerException("Can't resolve property \"" + field.Name + "\" of class \"" + type.FullName + "\"", ex);
                }

                field.SetValue(obj, valueObj);
            }

            var dependent = obj as IDependent;
            if (dependent != null) {
                try {
                    dependent.OnInjected();
                }
                catch (Exception e) {
                    if (OnInjectedException != null) {
                        OnInjectedException(e);
                    }
                    throw;
                }
            }
        }

        public void BuildUp<T>(T obj) {
            Type type = typeof(T);
            BuildUp(type, obj);
        }
    }
}
