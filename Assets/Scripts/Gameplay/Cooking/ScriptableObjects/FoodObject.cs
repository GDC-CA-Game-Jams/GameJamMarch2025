using UnityEngine;

namespace Gameplay.Cooking.ScriptableObjects
{
    /// <summary>
    /// Holds the information about the food. No logic is or should be done in this script
    /// </summary>
    [CreateAssetMenu(fileName = "FoodObject", menuName = "Scriptable Objects/FoodObject")]
    public class FoodObject : ScriptableObject
    {
        public Sprite sprite;

        public RuntimeAnimatorController controller;
        
        public AnimationClip clip;
        
        [SerializeField] private int pointsGained = 1;

        [SerializeField] private int timeGained;
        
        public int PointsGained => pointsGained;

        public int TimeGained => timeGained;
    }
}