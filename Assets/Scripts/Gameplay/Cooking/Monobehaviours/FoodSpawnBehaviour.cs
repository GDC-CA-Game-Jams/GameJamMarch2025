using Gameplay.Cooking;
using Gameplay.Cooking.Monobehaviours;
using Gameplay.Cooking.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;
using Util;

public class FoodSpawnBehaviour : StationBehaviour
{
    [SerializeField] private FoodObject spawningFood;


    protected override void Start()
    {
        base.Start();
        es.Raise(EventNames.STATION_SPAWN_FOOD_EVENT, this, new StationSpawnFoodEventArgs(id, spawningFood));
    }
    
    protected override void Update()
    {
        if (Input.GetKeyDown(Constants.INTERACT_BUTTON) && playerInRange)
        {
            audioSource.PlayOneShot(playerdropoff);
            Debug.Log($"[{GetType().Name}] Activating Station {id}");
            es.Raise(EventNames.STATION_ACTIVATED_EVENT, this, new StationActivationEventArgs(id));
            es.Raise(EventNames.STATION_SPAWN_FOOD_EVENT, this, new StationSpawnFoodEventArgs(id, spawningFood));
        }
    }
    
}
