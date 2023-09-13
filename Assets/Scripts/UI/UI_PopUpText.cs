using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PopUpText : MonoBehaviour
{
    private TextMeshPro text;
    [SerializeField] private float speed;
    [SerializeField] private float desapearanceSpeed;
    [SerializeField] private float colorDesapearanceSpeed;

    [SerializeField] private float lifeTime;

    private float textTimer;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;
        if(textTimer < 0)
        {
            float alpha = text.alpha - colorDesapearanceSpeed * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.b, text.color.b, alpha);
            if(text.color.a < 50)
            {
                speed = desapearanceSpeed;
            }
            if(text.color.a <= 0)
                Destroy(gameObject);
        }
    }
}
