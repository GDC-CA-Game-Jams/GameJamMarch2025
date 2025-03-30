using System.Collections.Generic;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class FoodObjectEventArgs : System.EventArgs
    {
        public FoodObject food;
        
        public FoodObjectEventArgs(FoodObject food)
        {
            this.food = food;
        }
    }
}