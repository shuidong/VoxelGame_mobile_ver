using UnityEngine;
using System.Collections;

public class HighLightBlock : MonoBehaviour {
	
	[SerializeField]
	private Camera mainCamera;

	private LineRenderer lineRender;
	private Vector3 p0;
	private Vector3 p1;
	private Vector3 p2;
	private Vector3 p3;

	void Start () {	
		lineRender = gameObject.GetComponent("LineRenderer") as LineRenderer;
		lineRender.SetColors(Color.red,Color.red);
		lineRender.SetWidth(0.1f,0.1f);
		lineRender.SetVertexCount(5);
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		if (!Physics.Raycast(mainCamera.transform.position,
		                     mainCamera.transform.forward,
		                     out hit))
			return;

		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (meshCollider == null || meshCollider.sharedMesh == null)
			return;
		
		Mesh mesh = meshCollider.sharedMesh;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
		p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
		p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
		p3 = new Vector3();
		
		if (p1.x != p0.x) {
			p3 = p2;
			p3.x = p0.x;
		}
		
		if (p1.y != p0.y) {
			p3 = p2;
			p3.y = p0.y;
		}
		
		if (p1.z != p0.z) {
			p3 = p2;
			p3.z = p0.z;
		}
		
		
		Transform hitTransform = hit.collider.transform;
		p0 = hitTransform.TransformPoint(p0);
		p1 = hitTransform.TransformPoint(p1);
		p2 = hitTransform.TransformPoint(p2);
		p3 = hitTransform.TransformPoint(p3);
		
		
		lineRender.SetPosition(0,p0);
		lineRender.SetPosition(1,p1);
		lineRender.SetPosition(2,p2);
		lineRender.SetPosition(3,p3);
		lineRender.SetPosition(4,p0);
		
		
		
		
		
		
		
	}
}
