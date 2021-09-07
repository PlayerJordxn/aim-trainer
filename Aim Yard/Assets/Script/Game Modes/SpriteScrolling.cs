using UnityEngine;

public class SpriteScrolling : MonoBehaviour
{
    public float ScrollX;
    public float ScrollY;

    // Update is called once per frame
    void FixedUpdate()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
