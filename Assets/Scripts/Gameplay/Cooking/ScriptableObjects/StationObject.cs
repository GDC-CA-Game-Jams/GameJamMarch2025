using UnityEngine;

namespace Gameplay.Cooking.ScriptableObjects
{
    /// <summary>
    /// Holds information about specific cooking stations. Does not and should not contain any logic
    /// </summary>
    [CreateAssetMenu(fileName = "CookingStation", menuName = "Scriptable Objects/CookingStation")]
    public class StationObject : ScriptableObject
    {
        [Tooltip("Recipes the cooking station can cook")]
        [SerializeField] private RecipeObject[] recipes;

        public RecipeObject[] Recipes => recipes;
    }
}