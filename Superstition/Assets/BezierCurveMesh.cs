using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class BezierCurveMesh : MonoBehaviour
{
	public Transform ER_Target;

	public Mesh meshToExtrude;
	[HideInInspector]
	public Mesh _createdMesh;

	[HideInInspector]
	public ExtrudeShape ex;

	[HideInInspector]
	List<OrientedPoint> path;

	private MeshCollider col;
	private MeshFilter filt;

	private float timeToAddOPoint;
	private float addPointRate = .15f;

	// so the handles go away in playmode
	public void Awake()
    {
		path = new List<OrientedPoint>();
		ex = new ExtrudeShape(meshToExtrude);
		col = GetComponent<MeshCollider>();
		filt = GetComponent<MeshFilter>();
	}

    void Update()
	{
		Vector3 pos = transform.InverseTransformPoint(ER_Target.transform.position);
		Vector3 rot = transform.InverseTransformDirection(ER_Target.transform.rotation.eulerAngles);
		if (timeToAddOPoint <= 0f)
		{
			//add a new oriented point to the list instead of just updating the most recent
			OrientedPoint dummy = new OrientedPoint(pos, Quaternion.Euler(rot));
			path.Add(dummy);
			timeToAddOPoint = addPointRate;

			//also update the mesh collider
			col.sharedMesh = filt.sharedMesh;
		
		}
		//other just update the most recent point
		else
		{
			path[path.Count-1] = new OrientedPoint(pos, Quaternion.Euler(rot));
		}

		_createdMesh = GetComponent<MeshFilter>().sharedMesh = new Mesh();	
		Extrude(_createdMesh, ex, path.ToArray());

		timeToAddOPoint -= Time.deltaTime;
	}
    public void Extrude(Mesh mesh, ExtrudeShape shape, OrientedPoint[] path)
	{

		int vertsInShape = shape.vert2Ds.Length;
		int segments = path.Length - 1;
		int edgeLoops = path.Length;
		int vertCount = vertsInShape * edgeLoops;
		int triCount = shape.lines.Length * segments;
		int triIndexCount = triCount * 3;


		int[] triangleIndices = new int[triIndexCount];
		Vector3[] vertices = new Vector3[vertCount];
		Vector3[] normals = new Vector3[vertCount];
		Vector2[] uvs = new Vector2[vertCount];

		for (int i = 0; i < path.Length; i++)
		{
			int offset = i * vertsInShape;
			for (int j = 0; j < vertsInShape; j++)
			{
				int id = offset + j;
				vertices[id] = path[i].LocalToWorld(shape.vert2Ds[j].point);
				normals[id] = path[i].LocalToWorldDirection(shape.vert2Ds[j].normal);
				uvs[id] = new Vector2(shape.vert2Ds[j].uCoord, i / ((float)edgeLoops));
			}
		}
		int[] lines = shape.lines;

		int ti = 0;
		for (int i = 0; i < segments; i++)
		{
			int offset = i * vertsInShape;
			for (int l = 0; l < lines.Length; l += 2)
			{
				int a = offset + lines[l] + vertsInShape;
				int b = offset + lines[l];
				int c = offset + lines[l + 1];
				int d = offset + lines[l + 1] + vertsInShape;
				triangleIndices[ti] = a; ti++;
				triangleIndices[ti] = b; ti++;
				triangleIndices[ti] = c; ti++;
				triangleIndices[ti] = c; ti++;
				triangleIndices[ti] = d; ti++;
				triangleIndices[ti] = a; ti++;
			}
		}
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangleIndices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
	}
}
//==========================================================================================================================
//ExtrudeShape
//==========================================================================================================================
public class ExtrudeShape
{
	public struct Vert2D
	{
		public Vert2D(Vector3 _point, Vector3 _normal, float _uCoord)
		{
			point = _point;
			normal = _normal;
			uCoord = _uCoord;
		}
		public Vector3 point;
		public Vector3 normal;
		public float uCoord;
	}
	public Vert2D[] vert2Ds;
	public int[] lines;

	public ExtrudeShape(Mesh _mesh)
	{ 
		List<Vert2D> pendingVert2Ds = new List<Vert2D>();
		List<int> pendingLines = new List<int>();

		//do the first vertex seperate. becuase it only needs to add itself to lines once
		//Vert2D first = new Vert2D(_mesh.vertices[0],_mesh.normals[0],_mesh.uv[0].x);
		//pendingVert2Ds.Add(first);
		//assign the connections between the verts. this only works if the verts are in the connected order in this array
		//pendingLines.Add(0);
		//loop over every vertex
		for (int i = 0; i < _mesh.vertexCount; i++)
		{
			Vert2D dummy = new Vert2D(_mesh.vertices[i],
										_mesh.normals[i],
										_mesh.uv[i].x);
			pendingVert2Ds.Add(dummy);
			//assign the connections between the verts. this only works if the verts are in the connected order in this array
			if (i == 0)
			{
				pendingLines.Add(i);
			}
			else
			{
				pendingLines.Add(i);
				pendingLines.Add(i);
			}
		}
		pendingLines.Add(0);		//gotta connect back to the first vert
		//initialize these arrays by the temp lists we were just using


		//vert2Ds = pendingVert2Ds.ToArray();
		lines = pendingLines.ToArray();
		//lines = _mesh.triangles;


		//lines = new int[]
		//{
		//	0,1,
		//	1,2,
		//	2,3,
		//	3,0
		//};
		//Vert2D a = new Vert2D(new Vector3(10, 1, 0), Vector3.right, 0f);
		//Vert2D b = new Vert2D(new Vector3(-10, 1, 0), Vector3.right, 0f);
		//Vert2D c = new Vert2D(new Vector3(-10, -1, 0), Vector3.right, 0f);
		//Vert2D d = new Vert2D(new Vector3(10, -1, 0), Vector3.right, 0f);

		//shitty hardcoding boys
		Vert2D vtx0 = new Vert2D(new Vector3(1f, 0f), -Vector3.up, 0f);
		Vert2D vtx1 = new Vert2D(new Vector3(.92388f, .382683f), -Vector3.up, 0f);
		Vert2D vtx2 = new Vert2D(new Vector3(.707107f, .707107f), -Vector3.up, 0f);
		Vert2D vtx3 = new Vert2D(new Vector3(.382683f, .92388f), -Vector3.up, 0f);
		Vert2D vtx4 = new Vert2D(new Vector3(0f, 1f), -Vector3.up, 0f);
		Vert2D vtx5 = new Vert2D(new Vector3(-0.382683f, .92388f), -Vector3.up, 0f);
		Vert2D vtx6 = new Vert2D(new Vector3(-0.707107f, .707107f), -Vector3.up, 0f);
		Vert2D vtx7 = new Vert2D(new Vector3(-0.92388f, .382683f), -Vector3.up, 0f);
		Vert2D vtx8 = new Vert2D(new Vector3(-1f, 0f), -Vector3.up, 0f);
		Vert2D vtx9 = new Vert2D(new Vector3(-0.92388f, -0.382683f), -Vector3.up, 0f);
		Vert2D vtx10 = new Vert2D(new Vector3(-0.707107f, -0.707107f), -Vector3.up, 0f);
		Vert2D vtx11 = new Vert2D(new Vector3(-0.382683f, -0.92388f), -Vector3.up, 0f);
		Vert2D vtx12 = new Vert2D(new Vector3(0f, -1f), -Vector3.up, 0f);
		Vert2D vtx13 = new Vert2D(new Vector3(0.382683f, -0.92388f), -Vector3.up, 0f);
		Vert2D vtx14 = new Vert2D(new Vector3(0.707107f, -0.707107f), -Vector3.up, 0f);
		Vert2D vtx15 = new Vert2D(new Vector3(0.92388f, -0.382683f), -Vector3.up, 0f);
		vert2Ds = new Vert2D[] { vtx0, vtx1,vtx2,vtx3,vtx4,vtx5,vtx6,vtx7,vtx8,vtx9,vtx10,vtx11,vtx12,vtx13,vtx14, vtx15};
		//vert2Ds = new Vert2D[] { vtx15, vtx14, vtx13, vtx12, vtx11, vtx10, vtx9, vtx8, vtx7, vtx6, vtx5, vtx4, vtx3, vtx2, vtx1, vtx0};
	}
}

public struct OrientedPoint
{
	public Vector3 position;
	public Quaternion rotation;

	public OrientedPoint(Vector3 pos, Quaternion rot)
	{
		this.position = pos;
		this.rotation = rot;
	}
	public Vector3 LocalToWorld(Vector3 point)
	{
		return position + rotation * point;
	}
	public Vector3 WorldToLocal(Vector3 point)
	{
		return Quaternion.Inverse(rotation) * (point - position);
	}
	public Vector3 LocalToWorldDirection(Vector3 dir)
	{
		return rotation * dir;
	}
}
//==========================================================================================================================
//CubicBezier3D
//==========================================================================================================================
public class CubicBezier3D
{
	private Vector3[] pts;

	[Range(0, 1)]
	public float myT;

	public CubicBezier3D(Vector3 _startPoint, Vector3 _startHandle, Vector3 _endHandle, Vector3 _endPoint)
	{
		pts = new Vector3[] { _startPoint, _startHandle, _endHandle, _endPoint };
	}
 
	public Vector3 GetPoint(float t)
	{
		float omt = 1f - t;
		float omt2 = omt * omt;
		float t2 = t * t;
		return pts[0] * (omt2 * omt) +
			pts[1] * (3f * omt2 * t) +
			pts[2] * (3f * omt * t2) +
			pts[3] * (t2 * t);
	}
	public Vector3 GetTangent(float t)
	{
		float omt = 1f - t;
		float omt2 = omt * omt;
		float t2 = t * t;
		Vector3 tangent =
			pts[0] * (-omt2) +
			pts[1] * (3 * omt2 - 2 * omt) +
			pts[2] * (-3 * t2 + 2 * t) +
			pts[3] * (t2);
		return tangent.normalized;
	}
	public Vector3 GetNomal3D(float t, Vector3 up)
	{
		Vector3 tng = GetTangent(t);
		Vector3 binormal = Vector3.Cross(up, tng).normalized;
		return Vector3.Cross(tng, binormal);
	}
	public Quaternion GetOrientation3D(float t, Vector3 up)
	{
		Vector3 tangent = GetTangent(t);
		Vector3 normal = GetNomal3D(t, up);
		return Quaternion.LookRotation(tangent, normal);
	}
}