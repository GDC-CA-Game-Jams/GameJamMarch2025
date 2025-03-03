using UnityEngine;
using Util.Input;
using Util.Services;

namespace Util
{
    
    public static class BeforeLoadBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // Initialize the ServicesLocator
            ServicesLocator.Initialize();
            
            // Initialize statics that will be used across the entire game
            ServicesLocator.Instance.Register(new EventService());
            ServicesLocator.Instance.Register(new TimeService());
            ServicesLocator.Instance.Register(new InputService());
            
            ServicesLocator.Instance.Get<InputService>().Init();
            
            // Initialize the GameManager last, after all the services it depends on are done
            // Commented out as the GameManager has been moved to the Gameplay assembly. Potentially look into how to get it back here, if needed
            //ServicesLocator.Instance.Register(new GameManager());
        }
    }
}