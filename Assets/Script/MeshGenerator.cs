using System;
using System.Threading.Tasks;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
	
	[SerializeField] private int _xSize = 512;
	[SerializeField] private int _zSize = 512;

	[SerializeField] private float _spacing = 0.5f;
	
	[SerializeField] private float _maxHeight = 20f;

	private Vector3[] _vertices;
	[SerializeField] private MeshFilter _terrainFilter;
	[SerializeField] private MeshFilter _waterFilter;

	private void Awake()
	{
		Mesh mesh = Generate();            
		_terrainFilter.sharedMesh = mesh;
		_waterFilter.sharedMesh = mesh;
	}
	
	
	private Mesh Generate () {
		Mesh mesh = new Mesh();
		mesh.name = "Procedural Grid";
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		    
		_vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];
		Vector2[] uv = new Vector2[_vertices.Length];
		Vector4[] tangents = new Vector4[_vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, z = 0; z <= _zSize; z++) {
			for (int x = 0; x <= _xSize; x++, i++) {
				_vertices[i] = new Vector3(x * _spacing, 0, z * _spacing);
				tangents[i] = tangent;
				uv[i] = new Vector2((x + 0.5f) / (_xSize + 1), (z + 0.5f) / (_zSize + 1));
			}
		}
		mesh.vertices = _vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;
   
		int[] triangles = new int[_xSize * _zSize * 6];
		for (int ti = 0, vi = 0, z = 0; z < _zSize; z++, vi++) {
			for (int x = 0; x < _xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + _xSize + 1;
				triangles[ti + 5] = vi + _xSize + 2;
			}
		}
		mesh.triangles = triangles;
		float w = _xSize * 0.5f, l = _zSize * 0.5f;
        mesh.bounds = new Bounds(new Vector3(w / 2, _maxHeight / 2, l / 2), new Vector3(w, _maxHeight, l));
		mesh.RecalculateNormals();

		return mesh;
	}
}
