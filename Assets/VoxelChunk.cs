using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AtlasUV
{
	public Vector2 leftBottom;
	public Vector2 leftTop;
	public Vector2 rightTop;
	public Vector2 rightBottom;
}

[System.Serializable]
public class TypeAtlasPos
{
	public Voxel.Type typeName;
	public AtlasUV atlasUV;
}

public class Voxel
{
	public enum Type {Grass, Dirt};
	public bool isActive = true;
	public Vector3 voxelPos;
	public Type _type;
	VoxelChunk _parent;
	
	public Voxel(Vector3 pos, Type type, VoxelChunk parent)
	{
		voxelPos = pos;
		_type = type;
		_parent = parent;
	}
	
	public void addMeshCubeInfos(ref List<Vector3> vertices, ref List<Vector2> uv, ref List<Vector3> normals)
	{
		
		#region front face
		if (_parent
		vertices.Add(new Vector3(0,0,0));
		vertices.Add(new Vector3(0,1,0));
		vertices.Add(new Vector3(1,1,0));
		vertices.Add(new Vector3(1,0,0));
		
		for (int i = 0; i < 4; ++i)
			normals.Add(Vector3.forward);
		
		_parent.addUV(ref uv, Voxel.Type.Dirt);
		
		#endregion
		
		
		#region right face
		vertices.Add(new Vector3(1,0,0));
		vertices.Add(new Vector3(1,1,0));
		vertices.Add(new Vector3(1,1,1));
		vertices.Add(new Vector3(1,0,1));
		
		for (int i = 0; i < 4; ++i)
			normals.Add(Vector3.right);
		
		_parent.addUV(ref uv, Voxel.Type.Grass);
		
		#endregion
	}
	
}

public class VoxelChunk : MonoBehaviour {
	public List<TypeAtlasPos> typesAtlasPos = new List<TypeAtlasPos>();
	private static Dictionary<Voxel.Type, AtlasUV> _typesAtlasPosDic = new Dictionary<Voxel.Type, AtlasUV>();
	
	public int voxelsWidth = 16;
	public int voxelsHeight = 16;
	public int voxelsDepth = 16;
	
	MeshFilter _mf;
	Mesh _mesh;
	
			
			
	void Awake()
	{
		if ( _typesAtlasPosDic.Count < typesAtlasPos.Count )
		{
			for ( int i = 0; i < typesAtlasPos.Count; ++i )
			{
				_typesAtlasPosDic.Add( typesAtlasPos[ i ].typeName, typesAtlasPos[ i ].atlasUV );
			}
		}
		
		_mf = GetComponent<MeshFilter>();
		_mesh = new Mesh();
		_mesh.name = "VoxelChunk";
		_mf.mesh = _mesh;
		setup();
		renderCubes();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	public Voxel[, ,] getVoxels()
	{	
		return _voxels;	
	}
			
	Voxel[, ,] _voxels;
	
	void setup()
	{
		_voxels = new Voxel[voxelsWidth, voxelsHeight, voxelsDepth];
		for (int x =0; x < voxelsWidth; ++x)
		{
			for (int y =0; y < voxelsHeight; ++y)
			{
				for (int z = 0; z < voxelsDepth; ++z)
				{
					_voxels[x, y, z] = new Voxel(new Vector3(x,y,z), Voxel.Type.Dirt, this);
				}
			}
		}
	}
	
	public void addUV(ref List<Vector2> uvList, Voxel.Type type)
	{
		AtlasUV uvArray;
		_typesAtlasPosDic.TryGetValue(type, out uvArray); 
		
		if (uvArray == null)
			return;
		
		uvList.Add(uvArray.leftBottom);
		uvList.Add(uvArray.leftTop);
		uvList.Add(uvArray.rightTop);
		uvList.Add(uvArray.rightBottom);
	}
	
	void renderCubes()
	{
		List<Vector3> vertices = new List<Vector3>();
	
		List<Vector2> uv = new List<Vector2>();
		List<Vector3> normals = new List<Vector3>();
		
		_voxels[0, 0, 0].addMeshCubeInfos(ref vertices, ref uv, ref normals);
		
		List<int> triangles = new List<int>();
		int vIndex = 0;
		while (vIndex < vertices.Count)
		{
			int startIndex = vIndex;
			int nextIndex = vIndex + 3;
			
			for (vIndex = startIndex; vIndex < nextIndex; ++vIndex)
			{
				triangles.Add(vIndex);
			}
			
			triangles.Add( vIndex - 1 );
			triangles.Add( vIndex );
			triangles.Add( startIndex );
			vIndex += 1;
		}
		
		_mesh.vertices = vertices.ToArray();
		_mesh.triangles = triangles.ToArray();
		_mesh.normals = normals.ToArray();
		_mesh.uv = uv.ToArray();
	}
	
	
}
