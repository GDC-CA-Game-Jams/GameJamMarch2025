using UnityEngine;

namespace Util.Services
{
    public class TimeService : IService
    {
        /// <summary>
        /// Custom timescale value. Allows pausing of the game without pausing all Unity functions
        /// </summary>
        private float timeScale = 1;

        /// <summary>
        /// Public getter and setter for the timescale
        /// </summary>
        public float TimeScale
        {
            get => timeScale;

            set => timeScale = value;
        }
    
        /// <summary>
        /// Scaled deltaTime since last frame
        /// </summary>
        public float DeltaTime
        {
            get => (Time.deltaTime * timeScale);
        }
    }
}