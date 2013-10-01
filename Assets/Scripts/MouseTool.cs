using UnityEngine;
using System.Collections;

public class MouseTool : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public LayerMask mouseSelectLayer;
	// Update is called once per frame
	void Update () {
		
	 	if (Input.GetMouseButtonDown(0))
		{
			Camera mainCam = Camera.main;
			Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100, mouseSelectLayer.value))
			{
				Debug.Log(hit.transform.tag);
				if (hit.transform.CompareTag("Chunk"))
				{
					VoxelChunk chunk = hit.transform.GetComponent<VoxelChunk>();
					chunk.onSelected(hit.point);
				}
			}
		}	
	}
}
