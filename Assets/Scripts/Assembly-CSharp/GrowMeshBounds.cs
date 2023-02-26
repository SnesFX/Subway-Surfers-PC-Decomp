using System.Collections.Generic;
using UnityEngine;

public class GrowMeshBounds : MonoBehaviour
{
	public float growFactor = 5f;

	private static HashSet<Mesh> grownMeshes = new HashSet<Mesh>();

	private void Awake()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		Mesh sharedMesh = component.sharedMesh;
		sharedMesh.RecalculateBounds();
		if (!grownMeshes.Contains(sharedMesh))
		{
			sharedMesh.bounds = new Bounds(sharedMesh.bounds.center, sharedMesh.bounds.extents * growFactor);
			grownMeshes.Add(sharedMesh);
		}
		else
		{
			Debug.Log(sharedMesh.name + " allready grown.");
		}
	}
}
