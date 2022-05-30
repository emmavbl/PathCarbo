using UnityEngine;
using UnityEngine.AI;


namespace PathCarbo.Test
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent myNavMeshAgent;
        [SerializeField] private PathTrace myPathTrace;
    
        void Start()
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
			    if (GetMouseRayHit(out Vector3 destination))
			    {
                    SetDestination(destination);
			    }
		    }

        }
    
        private bool GetMouseRayHit(out Vector3 targetPosition)
	    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            targetPosition = new Vector3();
            if (hasHit)
            {
                targetPosition = hit.point;
            }
            return hasHit;
        }

        private void SetDestination(Vector3 target)
	    {
            myNavMeshAgent.SetDestination(target);
	    }
    }
}
