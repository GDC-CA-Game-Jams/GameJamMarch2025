using System.Collections.Generic;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class StationActivationEventArgs : System.EventArgs
    {
        public string id;
        
        public StationActivationEventArgs(string id)
        {
            this.id = id;
        }
    }
}