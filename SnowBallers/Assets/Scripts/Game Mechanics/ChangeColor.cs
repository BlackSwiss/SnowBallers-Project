using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public GameObject gameObject;
    private Renderer objectRenderer; 

    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
    }

    public void ChangeObjectColor()
    {
        Color newColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1f);
        objectRenderer.material.SetColor("_Color",newColor);
    }
}
