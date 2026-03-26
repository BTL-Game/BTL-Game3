using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    public Image topImage;
    public Image bottomImage;

    public Sprite[] storySprites;
    public float transitionTime = 1f;
    public string nextSceneName = "GameplayScene";

    private int nextSpriteIndex = 1;
    private bool isTransitioning = false;

    void Start()
    {
        if (storySprites.Length < 2)
        {
            Debug.LogError("Need at least 2 sprites in storySprites array for the cross-fade effect to work.");
            return;
        }

        topImage.sprite = storySprites[0];
        SetAlpha(topImage, 1f);
        bottomImage.sprite = storySprites[1];
        SetAlpha(bottomImage, 1f);
    }

    public void OnStory(InputValue value)
    {
        if (value.isPressed && !isTransitioning)
        {
            StartCoroutine(CrossFadeSequence());
        }
    }

    IEnumerator CrossFadeSequence()
    {
        isTransitioning = true;

        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionTime);
            SetAlpha(topImage, alpha);
            yield return null;
        }

        SetAlpha(topImage, 0f);

        nextSpriteIndex++;

        if (nextSpriteIndex >= storySprites.Length)
        {
            // Wait 5 seconds before loading the next scene.
            yield return new WaitForSeconds(5f);
            Debug.Log("Story finished. Loading next scene...");
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        topImage.sprite = bottomImage.sprite;
        SetAlpha(topImage, 1f);
        bottomImage.sprite = storySprites[nextSpriteIndex];
        isTransitioning = false;
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, alpha);
    }
}