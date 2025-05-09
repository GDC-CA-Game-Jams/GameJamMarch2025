using UnityEngine;

namespace Gameplay.Cooking.ScriptableObjects
{
    /// <summary>
    /// Holds information about recipies - what ingredients make them, and what their output(s) are. No logic is or should be done here
    /// </summary>
    [CreateAssetMenu(fileName = "Recipie", menuName = "Scriptable Objects/Recipie")]
    public class RecipeObject : ScriptableObject
    {
        [SerializeField] private FoodObject[] ingredients;

        [SerializeField] private FoodObject[] results;

        public FoodObject[] Ingredients => ingredients;

        public FoodObject[] Results => results;
    }
}