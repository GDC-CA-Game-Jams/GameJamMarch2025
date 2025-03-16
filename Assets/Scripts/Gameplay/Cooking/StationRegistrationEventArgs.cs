using Gameplay.Cooking.Monobehaviours;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class StationRegistrationEventArgs : System.EventArgs
    {
        public StationObject data;

        public int openSlots;
        
        public StationRegistrationEventArgs(StationObject data, int openSlots)
        {
            this.data = data;
            this.openSlots = openSlots;
        }
        
    }
}