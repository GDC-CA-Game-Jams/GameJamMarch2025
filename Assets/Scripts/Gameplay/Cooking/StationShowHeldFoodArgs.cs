using System;
using System.Collections.Generic;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class StationShowHeldFoodArgs : EventArgs
    {
        public string id;

        public List<FoodObject> heldFood;

        public StationShowHeldFoodArgs(string id, List<FoodObject> heldFood)
        {
            this.id = id;
            this.heldFood = heldFood;
        }
    }
}