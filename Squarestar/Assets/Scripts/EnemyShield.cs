using UnityEngine;
using System.Collections;

public class EnemyShield : MonoBehaviour 
{
    public Camera shieldCam;
    public float expansionSpeed = 2f;
    public float reductionSpeed = 2f;
    private float[] modifier = new float[3] {1f, 1f, 1f};
    private Material shieldMat;
    private MeshRenderer mRend;
    [SerializeField]
    private float[] radius = new float[3];
    [SerializeField]
    private float radiusThreshold1 = 0.5f;
    private float radiusThreshold2;
    private bool[] hit = new bool[3];
    private int currentHit = 0;
    private int maxHit;

	// Use this for initialization
	void Start ()
    {
        mRend = GetComponent<MeshRenderer>();
        shieldMat = new Material(mRend.material);
        GetComponent<MeshRenderer>().material = shieldMat;

        maxHit = hit.Length;

        radiusThreshold2 = radiusThreshold1 * 10f;
        //mRend.enabled = false;
	}
	

    public void Hit(Vector2 hitUV, int hitIndex)
    {
        //mRend.enabled = true;
        // Reset
        radius[hitIndex] = 0f;
        modifier[hitIndex] = 1f;

        Vector4 centre = new Vector4(hitUV.x, hitUV.y, 0f, 0f);
        shieldMat.SetVector("_Center" + (hitIndex + 1).ToString(), centre);
        shieldMat.SetFloat("_Hit", 1f);

        hit[hitIndex] = true;
    }


	// Update is called once per frame
	void Update () 
    {
        MouseTest();

        for (int i = 0; i < 3; i++)
        {
            if (hit[i])
            {
                radius[i] += (expansionSpeed * modifier[i]) * Time.deltaTime;

                // shieldMat.SetFloat("_Radius" + (i + 1).ToString(), radius[i]);

                if (radius[i] >= radiusThreshold1)
                {
                    // hit = false;
                    hit[i] = false;
                    modifier[i] = 1f;
                    //modifier[i] = radiusThreshold2 / radiusThreshold1;
                    // shieldMat.SetFloat("_Hit", 0f);
                    // StartCoroutine(Fade());
                }

                if (radius[i] >= radiusThreshold2)
                {
                    hit[i] = false;
                    modifier[i] = 1f;
                }

            }
            else
            {
                if (radius[i] > 0f)
                    radius[i] -= (reductionSpeed * modifier[i]) * Time.deltaTime;
            }

            float _rad = radius[i];

            if (_rad < 0f)
                _rad = 0f;

            shieldMat.SetFloat("_Radius" + (i + 1).ToString(), _rad);
        }
	}

    private IEnumerator Fade()
    {
        Color fadeCol = mRend.material.color;
        float originalAlpha = fadeCol.a;

        while (fadeCol.a > 0f)
        {
            fadeCol.a -= 0.01f;

            mRend.material.color = fadeCol;
            yield return new WaitForEndOfFrame();
        }

        fadeCol.a = originalAlpha;
        mRend.material.color = fadeCol;

        mRend.enabled = false;
    }


    public void MouseTest()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Setup raycast information:
            RaycastHit _hit;
            Ray _ray = shieldCam.ScreenPointToRay(Input.mousePosition);

            // Raycast:
            if(Physics.Raycast(_ray, out _hit, 100f))
            {

                if(_hit.transform == transform)
                {
                    // Debug.Log("BOI " + _hit.textureCoord);
                    Vector2 hitCoords = _hit.textureCoord;

                    hitCoords.x *= shieldMat.GetTextureScale("_MainTex").x;
                    hitCoords.y *= shieldMat.GetTextureScale("_MainTex").y;

                    Hit(hitCoords, currentHit);

                    currentHit++;
                    
                    if(currentHit >= maxHit)
                    {
                        currentHit = 0; // Reset
                    }
                }
            }
        }
    }
}
