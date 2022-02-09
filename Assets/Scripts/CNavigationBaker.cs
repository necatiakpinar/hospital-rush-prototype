using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CNavigationBaker : MonoBehaviour {

    public List<NavMeshSurface> surfaces;

    // Use this for initialization
    public void BakeAll()
    {
        for (int i = 0; i < surfaces.Count; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }
    public void BakeLastSurface()
    {
        surfaces[surfaces.Count - 1].BuildNavMesh();
    }
    private void Start()
    {
        // BakeAll();
    }
    public void AddSurface(NavMeshSurface P_MeshSurface)
    {
        surfaces.Add(P_MeshSurface);
    }
}