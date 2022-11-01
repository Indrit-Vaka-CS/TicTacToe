using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class Box : MonoBehaviour
{
    public byte index;
    public TeamMark mark;
    /// <summary>
    /// If the box is used one or not
    /// </summary>
    public bool isMarked;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        index = (byte)transform.GetSiblingIndex();
        mark = TeamMark.None;
        isMarked = false;
    }

    public void SetAsMarked(Sprite sprite, TeamMark mark, Color color)
    {

        isMarked = true;
        this.mark = mark;
        
        spriteRenderer.color = color;
        spriteRenderer.sprite = sprite;

        //disable the CircleCollider2D (to avoid moving twice)
        GetComponent<CircleCollider2D>().enabled = false;
    }

}
