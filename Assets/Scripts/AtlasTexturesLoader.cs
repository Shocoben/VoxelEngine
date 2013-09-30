using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class AtlasInfo
{
	public Texture2D defaultTexture;
	public string name;
	public Material affectedMaterial;
}

public class AtlasTexturesLoader : MonoBehaviour 
{
	public string repository = "/Atlases";
	public List<AtlasInfo> atlasesInfo;

	private string _repoPath;
	
	// Use this for initialization
	void Awake () 
	{
		_repoPath = Application.persistentDataPath + repository;
		
		
		if(!Directory.Exists(_repoPath)) //Create the repo if it doesn't exists
        	Directory.CreateDirectory(_repoPath);
		
		
		foreach(AtlasInfo info in atlasesInfo)
		{
			string filePathNameOnly = _repoPath+"/"+info.name;
			
			string format = "";
			Texture2D textureToAffect;
			
			if (!imageExist(filePathNameOnly, ref format))
			{
				File.WriteAllBytes(filePathNameOnly+".png", info.defaultTexture.EncodeToPNG());
				textureToAffect = info.defaultTexture;
			}
			else
			{
				textureToAffect = new Texture2D(0,0);
				textureToAffect.LoadImage(File.ReadAllBytes(filePathNameOnly+"."+format));
			}
			
			info.affectedMaterial.mainTexture = textureToAffect;
		}
	}
	
	bool imageExist(string filePathNameOnly, ref string formatFound)
	{
		string[] formatsToTry = new string[] {"png", "jpg", "bmp"};
		for (int i = 0; i < formatsToTry.Length; ++i)
		{
			if (File.Exists(filePathNameOnly +"."+ formatsToTry[i]))
			{
				formatFound = formatsToTry[i];
				return true;
			}
		}
		return false;
	}
	
	
	
}
