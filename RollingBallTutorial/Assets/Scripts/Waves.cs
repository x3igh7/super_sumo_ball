using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour {

    public float scale = 0.5f;
    public float speed = 1.0f;

    private Vector3[] baseHeight;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        var mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
        {
            baseHeight = mesh.vertices;
        }

        var verticies = new Vector3[baseHeight.Length];

        for (int i = 0; i < verticies.Length; i++)
        {
            var vertex = baseHeight[i];
            vertex.y +=
                Mathf.Sin(Time.time
                * speed
                + baseHeight[i].x
                + baseHeight[i].y
                + baseHeight[i].z)
                * scale;
            verticies[i] = vertex;
        }

        mesh.vertices = verticies;
        mesh.RecalculateNormals();

        var collider = GetComponent<MeshCollider>();

        // set to null to force collision to recalculate. 
        collider.sharedMesh = null;
        collider.sharedMesh = mesh;
    }
}
