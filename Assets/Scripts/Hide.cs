using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hide : MonoBehaviour
{
    public string playerTag = "Player";
    public float detectionRadius = 2f;
    public float fadeSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player && spriteRenderer != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            float targetAlpha = (distance <= detectionRadius) ? 1f : 0f;
            Color newColor = spriteRenderer.color;
            newColor.a = Mathf.Lerp(newColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
            spriteRenderer.color = newColor;
        }
    }
}
