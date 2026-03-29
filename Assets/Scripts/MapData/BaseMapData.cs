using UnityEngine;

// Lớp này không cần CreateAssetMenu vì chúng ta sẽ không tạo "Map Chung Chung"
public class BaseMapData : ScriptableObject 
{
    [Header("--- DỮ LIỆU CHUNG (AI CŨNG CÓ) ---")]
    public string mapName;
    public AudioClip backgroundMusic;
    
    [Header("Parallax Textures")]
    public Texture2D farBG;
    public Texture2D midBG;
    public Texture2D nearBG;

    [Header("Vật cản cơ bản")]
    public GameObject pillarPrefab; 
}