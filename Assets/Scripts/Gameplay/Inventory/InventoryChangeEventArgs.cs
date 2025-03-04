using System;
using Gameplay.Cooking.ScriptableObjects;
using Util;

namespace Gameplay.Inventory
{
    public class InventoryChangeEventArgs : EventArgs
    {
        public FoodObject[] food;

        public Enums.INVENTORY_ACTIONS action;
        
        public InventoryChangeEventArgs(FoodObject[] food, Enums.INVENTORY_ACTIONS action)
        {
            this.food = food;
            this.action = action;
        }
    }
}