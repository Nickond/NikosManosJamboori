using UnityEngine;
using System.Collections;

public class AnimateTexture : MonoBehaviour 
{
    private Material animatedMat;
    private MeshRenderer mRend;
    private float current;

    // private int[] bounds = new int[2];
    public int frames = 6;
    public int rows;
    public int columns;
    public float step;
    public bool animate = true;
    public float rate = 0.02f;
    private float elapsedTime = 0f;
	// Use this for initialization
	void Start () 
    {
        mRend = GetComponent<MeshRenderer>();
        animatedMat = new Material(mRend.material);

        mRend.material = animatedMat;

        // Get the texture bounds
        // bounds[0] = animatedMat.mainTexture.width;
        // bounds[1] = animatedMat.mainTexture.height;

        animatedMat.SetTextureOffset("_MainTex", new Vector2(0, 0));
        animatedMat.SetTextureScale("_MainTex", new Vector2(1f / rows, 1f / columns));

        step = 1f / rows;
        current = 0;
	}


	// Update is called once per frame
	void Update () 
    {
        if (animate)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > rate)
            {

                if (current >= 1)
                    current = 0;

                animatedMat.SetTextureOffset("_MainTex", new Vector2(current, 0));

                current += step;
                elapsedTime = 0f;
            }
        }
	}
}
