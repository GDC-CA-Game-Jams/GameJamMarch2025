using System.Collections.Generic;
using Gameplay.Cooking.ScriptableObjects;

namespace Gameplay.Cooking
{
    public class RecipieEventArgs : System.EventArgs
    {
        public RecipeObject recipie;
        
        public RecipieEventArgs(RecipeObject recipie)
        {
            this.recipie = recipie;
        }
    }
}