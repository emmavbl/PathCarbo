using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PathCarbo
{
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class PathTrace : MonoBehaviour
    {
        public enum EPathTraceType { Agent, Positions };

         public EPathTraceType pathTraceType;
        public EPathTraceType PathTraceType
        {
            get { return pathTraceType; }
            private set { PathTraceType = value; }
        }
        // Visuals
        [SerializeField] public GameObject endMarkerPrefab;
        [SerializeField] public Camera renderCamera;
        [SerializeField] public bool showPathOnStart;
        [SerializeField] public float height;

        // Agent 
        [SerializeField] public NavMeshAgent agent;

        // Position      
        [SerializeField] public List<Transform> targets;

        private GameObject EndMarkerInstantiated;
        private LineRenderer LineRenderer;
        private List<Vector3> ComputedPath = new List<Vector3>();
        [SerializeField] public bool previewPathInEditor;

	    private void Awake()
	    {
            LineRenderer = GetComponent<LineRenderer>();
            if (!LineRenderer)
		    {
                Debug.LogError("[PathTrace] There is no LineRenderer component on " + transform.name + ".");
            }
        }


	    void Start()
        {
            LineRenderer.positionCount = 0;
		    foreach (Transform child in transform)
		    {
			    DestroyImmediate(child.gameObject);
		    }

            if (endMarkerPrefab)
            {
                EndMarkerInstantiated = Instantiate(endMarkerPrefab, transform.position, Quaternion.identity, transform);
                EndMarkerInstantiated.SetActive(false);
            }

		    if (Application.isPlaying && pathTraceType == EPathTraceType.Positions && showPathOnStart)
		    {
                DrawPath();
		    }
        }

        void Update()
        {
            if (Application.isPlaying)
		    {
                if (pathTraceType == EPathTraceType.Agent)
		        {
                    if (agent.hasPath)
			        {
                        DrawPath(agent.path);
			        }
		        }

                if (pathTraceType == EPathTraceType.Positions)
		        {
                    DrawPath();
		        }
		    } else if(previewPathInEditor){
                DrawPath();
		    }
        }

        public bool ComputePath()
	    {
            if (targets.Count < 2)
		    {
			    Debug.LogError("[PathTrace] There is to little positions to do a path trace.");
                return false;
		    }

            ComputedPath.Clear();
		    for (int positionId = 1; positionId < targets.Count; positionId++)
		    {
                NavMeshPath path = new NavMeshPath();
                if (!NavMesh.CalculatePath(targets[positionId - 1].position, targets[positionId].position, NavMesh.AllAreas, path))
			    {
                    return false;
			    }
                ComputedPath.AddRange(path.corners);
		    }
            return true;
	    }

        public void DrawPath()
        { 
            if (ComputePath())
		    {
                DrawPath(ComputedPath);
		    }
	    }

        public void DrawPath(NavMeshPath pathToDraw)
	    {
            DrawPath(new List<Vector3>(pathToDraw.corners));
	    }

        public void DrawPath(List<Vector3> pathToDraw)
        {
            if (endMarkerPrefab)
		    {
                if (!EndMarkerInstantiated)
			    {
                    EndMarkerInstantiated = Instantiate(endMarkerPrefab, transform.position, Quaternion.identity, transform);
                }
                EndMarkerInstantiated.SetActive(true);
                EndMarkerInstantiated.transform.position = pathToDraw[pathToDraw.Count - 1];
		    }

            LineRenderer.positionCount = pathToDraw.Count;

		    for (int i = 0; i < pathToDraw.Count; i++)
		    {
                pathToDraw[i] = pathToDraw[i] + new Vector3(0, height, 0);
            }

            LineRenderer.SetPositions(pathToDraw.ToArray());
        }

        public void RemovePath()
	    {
            LineRenderer.positionCount = 0;
            if (EndMarkerInstantiated)
		    {
                EndMarkerInstantiated.SetActive(false);
		    }
        }
	    private void OnDestroy()
	    {
            DestroyImmediate(EndMarkerInstantiated);
	    }
    }
}