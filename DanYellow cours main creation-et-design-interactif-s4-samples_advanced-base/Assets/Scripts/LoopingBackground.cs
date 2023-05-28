using UnityEngine;

// https://www.youtube.com/watch?v=efYt5h2i_1A
// Unlit > texture
public class LoopingBackground : MonoBehaviour
{
    [Range(0.1f, 100)]
    public float speed = 0;

    public Renderer render;

    [Tooltip("Define on which axis the texture will loop")]
    public bool xAxis;
    public bool yAxis;

    // Update is called once per frame
    void Update()
    {
        render.material.mainTextureOffset += new Vector2(
            xAxis ? Time.deltaTime * speed : 0f, 
            yAxis ? Time.deltaTime * speed : 0f 
        );
    }
}
