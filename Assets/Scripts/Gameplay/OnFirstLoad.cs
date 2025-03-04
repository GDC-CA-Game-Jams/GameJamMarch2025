using Gameplay.Cooking;
using Gameplay.Inventory;
using UnityEngine;
using Util.Services;

namespace Gameplay
{
    /// <summary>
    /// Things to run once at game start and never again
    /// </summary>
    public class OnFirstLoad : MonoBehaviour
    {
        private void Awake()
        {
            // TODO: Find a better spot for this
            ServicesLocator.Instance.Register(new InventoryService());
            ServicesLocator.Instance.Get<InventoryService>().Init();
            
            ServicesLocator.Instance.Register(new CookingService());
            ServicesLocator.Instance.Get<CookingService>().Init();
            
            Destroy(gameObject);
        }
    }
}