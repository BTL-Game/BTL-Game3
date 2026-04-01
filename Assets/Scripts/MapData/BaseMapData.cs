using UnityEngine;
public class BaseMapData : ScriptableObject 
{
    [Header("--- COMMON SHARED DATA ---")]
    public string mapName;
    public AudioClip backgroundMusic;
    [Header("Parallax Textures")]
    public Texture2D farBG;
    public Texture2D midBG;
    public Texture2D nearBG;
    [Header("Basic Obstacles")]
    public GameObject pillarPrefab; 
}
