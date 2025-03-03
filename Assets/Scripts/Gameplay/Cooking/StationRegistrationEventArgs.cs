using Gameplay.Cooking.Monobehaviours;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class StationRegistrationEventArgs : System.EventArgs
    {
        public StationObject data;
        
        public StationRegistrationEventArgs(StationObject data)
        {
            this.data = data;
        }
        
    }
}