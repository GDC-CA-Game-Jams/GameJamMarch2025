using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Services
{
    /// <summary>
    /// Used to store and manage universally accessible services, which use the IService interface. Ensures only one instance of each service exists and manages access to them.
    /// Must be loaded by calling Initialize() before first use. Once initialized, will stay initialized for the life of the program
    /// </summary>
    public class ServicesLocator
    {
        /// <summary>
        /// Dictionary of the services, stored by type name as key
        /// </summary>
        private readonly Dictionary<string, IService> services = new Dictionary<string, IService>();

        private ServicesLocator() { }
        
        /// <summary>
        /// Get the instance, if initialized, of the ServicesLocator. Does NOT lazy load
        /// </summary>
        public static ServicesLocator Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initialize the singleton so it can be used.
        /// </summary>
        public static void Initialize()
        {
            // If instance is null, set it to a new instance
            Instance ??= new ServicesLocator();
        }
        
        /// <summary>
        /// Gets a service of type T
        /// </summary>
        /// <typeparam name="T">Type of service to get. Must implement IService</typeparam>
        /// <returns>The instance registered with the locator, strongly-typed to T</returns>
        /// <exception cref="InvalidOperationException">Thrown when a service of the requested type has not been registered</exception>
        public T Get<T>() where T : IService
        {
            string key = typeof(T).Name;

            if (services.TryGetValue(key, out var service))
            {
                return (T)service;
            }
            else
            {
                Debug.LogError($"[{GetType().Name}] {key} not registered");
                throw new InvalidOperationException();
            }
        }
        
        
        /// <summary>
        /// Register a service of type T to the ServicesLocator. Duplicates will log an error, but not throw one
        /// </summary>
        /// <param name="service">The instance of the service to register</param>
        /// <typeparam name="T">The type of the service to register. Must implement IService</typeparam>
        public void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;

            if (!services.TryAdd(key, service))
            {
                Debug.LogError($"[{GetType().Name}] Attempted to register service of type {key} which is already registered");
                return;
            }
        }
        
        /// <summary>
        /// Unregister a service of type T to the ServicesLocator. If the type was not registered, will  log an error but not throw one
        /// </summary>
        /// <typeparam name="T">The type of service to unregister. Must implement IService.</typeparam>
        public void Unregister<T>() where T : IService
        {
            string key = typeof(T).Name;

            if (services.ContainsKey(key))
            {
                services[key].Dispose();
                services.Remove(key);
            }
            else
            {
                Debug.LogError($"[{GetType().Name}] Attempted to unregister service of type {key} which is not registered");
            }
                
        }
    }
}