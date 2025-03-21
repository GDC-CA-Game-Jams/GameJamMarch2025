using System;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class StationSpawnFoodEventArgs : EventArgs
    {
        public string id;

        public FoodObject food;

        public StationSpawnFoodEventArgs(string id, FoodObject food)
        {
            this.id = id;
            this.food = food;
        }
    }
}