using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportPortal : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("The map data to apply when the player enters this portal. One will be chosen randomly if there are multiple.")]
    public BaseMapData[] targetMaps;
    public float fadeDuration = 3f;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !isTeleporting)
        {
            if (targetMaps == null || targetMaps.Length == 0)
            {
                Debug.LogWarning("[TeleportPortal] Không có targetMaps nào được gán!");
                return;
            }
            Debug.Log("[TeleportPortal] Đi vào portal, bắt đầu chuyển map.");
            
            // Ẩn Portal đi và ngắt va chạm để player không đụng 2 lần
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (var r in renderers) r.enabled = false;

            // Tắt script di chuyển để nó không bay vào vùng Hủy (Shredder/DeadZone) ở bên trái màn hình
            MonoBehaviour movement = GetComponent("PillarMovement") as MonoBehaviour;
            if (movement != null) movement.enabled = false;

            StartCoroutine(TeleportRoutine(collision.gameObject));
        }
    }

    private IEnumerator TeleportRoutine(GameObject playerObj)
    {
        isTeleporting = true;

        GameObject canvasGo = new GameObject("FadeCanvas");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; 

        GameObject imageGo = new GameObject("FadeImage");
        imageGo.transform.SetParent(canvasGo.transform, false);
        Image fadeImage = imageGo.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0); 

        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        if (MapManager.Instance != null && targetMaps != null && targetMaps.Length > 0)
        {
            BaseMapData selectedMap = targetMaps[Random.Range(0, targetMaps.Length)];
            
            // Đảm bảo không chọn random trùng map hiện tại nếu đang có nhiều lựa chọn
            if (targetMaps.Length > 1 && MapManager.Instance.currentMap == selectedMap)
            {
                for (int i = 0; i < targetMaps.Length; i++)
                {
                    if (targetMaps[i] != selectedMap)
                    {
                        selectedMap = targetMaps[i];
                        break;
                    }
                }
            }

            Debug.Log($"[TeleportPortal] Áp dụng map mới: {selectedMap.name}");
            MapManager.Instance.ApplyMap(selectedMap);
        }

        // Kích hoạt lại đoạn Intro của rồng và reset tốc độ game
        if (playerObj != null)
        {
            PlayerIntro intro = playerObj.GetComponent<PlayerIntro>();
            if (intro != null)
            {
                intro.enabled = true;
                intro.StartIntro();
            }
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameSpeed();
        }

        yield return new WaitForSeconds(0.1f);

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (timer / fadeDuration));
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        Debug.Log("[TeleportPortal] Đã hoàn thành fade, hủy Portal.");
        Destroy(canvasGo);
        Destroy(gameObject);
    }
}
