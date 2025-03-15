using System.Collections;
using UnityEngine;
using Util.Services;

namespace Gameplay.Cooking.Monobehaviours
{
    public class FollowBehaviour : MonoBehaviour
    {
        private GameObject target;

        [SerializeField] private float followSpeed = 5;

        private bool isFollowing = false;

        private TimeService ts;

        private void Awake()
        {
            ts = ServicesLocator.Instance.Get<TimeService>();
        }
        
        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        public void StartFollow()
        {
            isFollowing = true;
            StartCoroutine(Follow());
        }

        public void StopFollow()
        {
            isFollowing = false;
        }
        
        private IEnumerator Follow()
        {
            while (isFollowing)
            {
                transform.position = Vector2.Lerp(transform.position, target.transform.position,
                    followSpeed * ts.DeltaTime);
                yield return null;
            }
        }
    }
}