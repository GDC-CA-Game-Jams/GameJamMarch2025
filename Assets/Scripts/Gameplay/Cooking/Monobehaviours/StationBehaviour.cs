using System;
using Gameplay.Cooking.ScriptableObjects;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Cooking.Monobehaviours
{
    public class StationBehaviour : MonoBehaviour
    {
        [SerializeField] private StationObject stationData;

        private EventService es;

        private string id = "";

        public StationObject StationData => stationData;

        private void Start()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            es.Raise(EventNames.STATION_REGISTRATION_EVENT, this, new StationRegistrationEventArgs(stationData));
        }

        public void SetId(string id)
        {
            if (id == "")
            {
                Debug.LogWarning($"[{GetType().Name}] Station ID already set, not re-setting");
                return;
            }
            this.id = id;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                es.Raise(EventNames.STATION_ACTIVATED_EVENT, this, new StationActivationEventArgs(id));
            }
        }
    }
}