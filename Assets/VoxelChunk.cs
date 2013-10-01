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

public struct IntVector3
{
	
	public int x;
	public int y;
	public int z;
}

public class Voxel
{

	
	public enum Type {Grass, Dirt};
	public enum FaceID {front, back, top, bottom, right, left};
	
	private bool _active = true;
	public bool isActive
	{
	  get
      { 
         return _active; 
      }
      set
      {
        _active = value; 
      }
	}
	
	private Vector3 voxelPos;
	public Type _type;
	VoxelChunk _parent;
	
	Voxel[] neighbourhoods = new Voxel[6];
	IntVector3 intPos = new IntVector3();
	
		
	public Voxel(Vector3 pos, Type type, VoxelChunk parent)
	{
	
		voxelPos = pos;
		_type = type;
		_parent = parent;
		
		for (int i = 0; i < 6; ++i)
			neighbourhoods[i] = null;
		
		intPos.x = Mathf.FloorToInt(pos.x);
		intPos.y = Mathf.FloorToInt(pos.y);
		intPos.z = Mathf.FloorToInt(pos.z);
	}
	
	public void addMeshCubeInfos(ref List<Vector3> vertices, ref List<Vector2> uv, ref List<Vector3> normals)
	{
		#region front face
		if (neighbourhoods[(int) FaceID.front] == null || !neighbourhoods[(int) FaceID.front].isActive)
		{
			
			vertices.Add(new Vector3(voxelPos.x,voxelPos.y,voxelPos.z));
			vertices.Add(new Vector3(voxelPos.x,voxelPos.y + 1,voxelPos.z));
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y + 1,voxelPos.z));
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y,voxelPos.z));
			
			for (int i = 0; i < 4; ++i)
				normals.Add(Vector3.forward);
			
			_parent.addUV(ref uv, Voxel.Type.Dirt);
		}
		#endregion
		
		//Debug.DrawRay(new Vector3(voxelPos.x, voxelPos.y, voxelPos.z), Vector3.forward, Color.green, 5);
		
		#region back face
		if (neighbourhoods[(int) FaceID.back] == null || !neighbourhoods[(int) FaceID.back].isActive)
		{
			vertices.Add(new Vector3(voxelPos.x,voxelPos.y,voxelPos.z + 1));
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y,voxelPos.z + 1));
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y + 1,voxelPos.z + 1));
			vertices.Add(new Vector3(voxelPos.x,voxelPos.y + 1,voxelPos.z + 1));
			
			
			
			for (int i = 0; i < 4; ++i)
				normals.Add(Vector3.back);
			
			_parent.addUV(ref uv, Voxel.Type.Dirt);
			
		}
		#endregion
		
		#region right face
		if (neighbourhoods[(int) FaceID.right] == null || !neighbourhoods[(int) FaceID.right].isActive)
		{
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y,voxelPos.z));
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y + 1,voxelPos.z));
			vertices.Add(new Vector3(voxelPos.x + 1, voxelPos.y + 1, voxelPos.z + 1));
			vertices.Add(new Vector3(voxelPos.x + 1,voxelPos.y,voxelPos.z + 1));
			
			for (int i = 0; i < 4; ++i)
				normals.Add(Vector3.right);
			
			_parent.addUV(ref uv, Voxel.Type.Dirt);
		}
		#endregion
		
		#region left face
		if (neighbourhoods[(int) FaceID.left] == null || !neighbourhoods[(int) FaceID.left].isActive)
		{
			
			vertices.Add(new Vector3(voxelPos.x, voxelPos.y,voxelPos.z));
			vertices.Add(new Vector3(voxelPos.x, voxelPos.y,voxelPos.z + 1));
			vertices.Add(new Vector3(voxelPos.x, voxelPos.y + 1, voxelPos.z + 1));
			vertices.Add(new Vector3(voxelPos.x, voxelPos.y + 1,voxelPos.z));
			
			for (int i = 0; i < 4; ++i)
				normals.Add(Vector3.left);
			
			_parent.addUV(ref uv, Voxel.Type.Dirt);
		}
		#endregion
		
		#region bottom face
		if (neighbourhoods[(int) FaceID.bottom] == null || !neighbourhoods[(int) FaceID.bottom].isActive)
		{
			
				vertices.Add(new Vector3(voxelPos.x, voxelPos.y,voxelPos.z));
				vertices.Add(new Vector3(voxelPos.x + 1, voxelPos.y,voxelPos.z));
				vertices.Add(new Vector3(voxelPos.x + 1, voxelPos.y, voxelPos.z + 1));
				vertices.Add(new Vector3(voxelPos.x, voxelPos.y,voxelPos.z + 1));
	
				for (int i = 0; i < 4; ++i)
					normals.Add(Vector3.down);
				
				_parent.addUV(ref uv, Voxel.Type.Dirt);
		}
		#endregion
		
		#region top face
		if (neighbourhoods[(int) FaceID.top] == null || !neighbourhoods[(int) FaceID.top].isActive)
		{
			
				vertices.Add(new Vector3(voxelPos.x, voxelPos.y + 1,voxelPos.z));
				vertices.Add(new Vector3(voxelPos.x, voxelPos.y + 1,voxelPos.z + 1));
				vertices.Add(new Vector3(voxelPos.x + 1, voxelPos.y + 1, voxelPos.z + 1));
				vertices.Add(new Vector3(voxelPos.x + 1, voxelPos.y + 1,voxelPos.z));
				
				for (int i = 0; i < 4; ++i)
					normals.Add(Vector3.up);
				
				_parent.addUV(ref uv, Voxel.Type.Grass);
		}
		#endregion
		
		
	}
	
	public void setNeighbourhoods()
	{
		Voxel[,,] parentVoxels = _parent.getVoxels();
		
		if (voxelPos.x > 0) //left neighbourhood
			neighbourhoods[(int) FaceID.left] = parentVoxels[intPos.x - 1, intPos.y, intPos.z];
		
		if (voxelPos.x < _parent.voxelsWidth - 1) //right neighbourhood
			neighbourhoods[(int) FaceID.right] = parentVoxels[intPos.x + 1, intPos.y, intPos.z];
		
		if (voxelPos.y < _parent.voxelsHeight - 1)
			neighbourhoods[(int) FaceID.top] = parentVoxels[intPos.x , intPos.y + 1, intPos.z];
		
		if (voxelPos.y > 0)
			neighbourhoods[(int) FaceID.bottom] = parentVoxels[intPos.x , intPos.y - 1, intPos.z];
		
		if (voxelPos.z < _parent.voxelsDepth -1 )
			neighbourhoods[(int) FaceID.back] = parentVoxels[intPos.x , intPos.y , intPos.z + 1];
		
		if (voxelPos.z > 0 )
			neighbourhoods[(int) FaceID.front] = parentVoxels[intPos.x , intPos.y , intPos.z - 1];
		
		/*
		if (intPos.x == 0 && intPos.y == 0 && intPos.z == 0)
		{
			for (int i = 0; i < 6; ++i)
			{
				if (neighbourhoods[i] == null)
				{
					Debug.Log("face " +  i + " is null");
					continue;
				}
				Debug.Log("face " + i + " " + neighbourhoods[i].voxelPos);
				
			}
		}*/
	}
	
}

public class VoxelChunk : MonoBehaviour {
	public List<TypeAtlasPos> typesAtlasPos = new List<TypeAtlasPos>();
	private static Dictionary<Voxel.Type, AtlasUV> _typesAtlasPosDic = new Dictionary<Voxel.Type, AtlasUV>();
	
	public int voxelsWidth = 16;
	public int voxelsHeight = 16;
	public int voxelsDepth = 16;
	
	MeshFilter _mf;
	MeshCollider _mc;
	Mesh _mesh;
	
	Voxel[, ,] _voxels;		
			
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
		_mc = GetComponent<MeshCollider>();
		
		_mesh = new Mesh();
		_mesh.name = "VoxelChunk";
		_mf.mesh = _mesh;
		_mc.sharedMesh = _mesh;
		setup();
		renderCubes();
	}
	
	public Voxel[, ,] getVoxels()
	{	
		return _voxels;	
	}
			
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
		
		for (int x =0; x < voxelsWidth; ++x)
		{
			for (int y =0; y < voxelsHeight; ++y)
			{
				for (int z = 0; z < voxelsDepth; ++z)
				{
					_voxels[x, y, z].setNeighbourhoods();
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
		_mesh.Clear();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uv = new List<Vector2>();
		List<Vector3> normals = new List<Vector3>();
		
		for (int x =0; x < voxelsWidth; ++x)
		{
			for (int y =0; y < voxelsHeight; ++y)
			{
				for (int z = 0; z < voxelsDepth; ++z)
				{
					if (!_voxels[x, y , z].isActive)
						continue;
					_voxels[x, y, z].addMeshCubeInfos(ref vertices, ref uv, ref normals);
				}
			}
		}
		
		Debug.Log("nbrVertices " + vertices.Count);
		
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
		
		_mc.sharedMesh = _mesh;
		
	}
	
	public void onSelected(Vector3 point)
	{
		Vector3 relativePoint = new Vector3(Mathf.Abs(transform.position.x -point.x), Mathf.Abs(transform.position.y - point.y), Mathf.Abs(transform.position.z - point.z));

		//IntVector3 flooredPoint = new IntVector3();
		
		int pointX = (relativePoint.x >= voxelsWidth)? voxelsWidth - 1 : Mathf.FloorToInt(relativePoint.x);
		int pointY = (relativePoint.y >= voxelsHeight)? voxelsHeight - 1 : Mathf.FloorToInt(relativePoint.y);
		int pointZ = (relativePoint.z >= voxelsDepth)? voxelsDepth - 1 : Mathf.FloorToInt(relativePoint.z);
		
		_voxels[pointX, pointY, pointZ].isActive = false;
		
		renderCubes();
		
	}
	
}
