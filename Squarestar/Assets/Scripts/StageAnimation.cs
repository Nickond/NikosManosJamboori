using UnityEngine;
using System.Collections;

public class StageAnimation : MonoBehaviour 
{

    private Material stageMat;
    private float prevOffset;
    private float b = 0.1f;
    public float offsetX;

    public bool move = true; // texture move flag

	// Use this for initialization
	void Start () 
    {
        stageMat = GetComponent<MeshRenderer>().material;

        if(move)
            StartCoroutine(MethodB());
	}
	
	// Update is called once per frame
	void Update () 
    {
        //MethodA();
	}

    IEnumerator MethodB()
    {
        float a = 0f;
        float fullStep = 0.125f;
        float step = 0.001f;

        float current = stageMat.GetTextureOffset("_DetailAlbedoMap").x;

        // Loop through A
        while(a < fullStep)
        {
            a += step;

            stageMat.SetTextureOffset("_DetailAlbedoMap", new Vector2(current - step, 0f)); // move offset

            if(move)
                current = stageMat.GetTextureOffset("_DetailAlbedoMap").x; // Reset current

            yield return new WaitForEndOfFrame();
        }
        

        yield return new WaitForSeconds(1f);

        StartCoroutine(MethodB());
    }


    void MethodA()
    {
        float _offset = (10 * (Mathf.Sin((offsetX * Time.deltaTime) / b)));

        stageMat.SetTextureOffset("_DetailAlbedoMap", new Vector2(prevOffset - _offset, 0f));

        Debug.Log("OFF: " + b);

        prevOffset = stageMat.GetTextureOffset("_DetailAlbedoMap").x;
    }
}
