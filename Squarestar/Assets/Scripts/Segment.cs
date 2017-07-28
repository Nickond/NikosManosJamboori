using UnityEngine;
using System.Collections;
using Global;

public class Segment : MonoBehaviour {

    private SegmentManager manager;
    private GameObject indicator;
    private float indicatorDuration;
    private Material indicatorMat;
    private Material[] elementMats;
    private bool indicatorHidden = true;
    private Color transparent;
	// Use this for initialization
	void Start ()
    {
        manager = GetComponentInParent<SegmentManager>(); // Find the manager

        elementMats = manager.indicatorMaterials;

        indicator = transform.GetChild(0).gameObject; // Get The indicator object
        indicatorMat = new Material(manager.indicatorMaterials[Random.Range(0, 3)]); // Choose a random one (testing)

        transparent = indicatorMat.GetColor("_Colour");
        transparent.a = 0f;

        if (indicatorHidden)
            indicatorMat.SetColor("_Colour", transparent);

        indicator.GetComponent<MeshRenderer>().material = indicatorMat; // Set the material

	}
	
    /// <summary>
    /// Launch the Indication Coroutine
    /// </summary>
    /// <param name="element"></param>
    public void Indicate(Elements element, float _dur)
    {
        if (indicatorHidden)
        {
            indicatorDuration = _dur;
            StartCoroutine(IndicateCoroutineTEST(element));
        }
    }

    private IEnumerator IndicateCoroutineTEST(Elements element)
    {
        // Falsify hidden flag
        indicatorHidden = false; 

        indicatorMat = new Material(elementMats[(int)element]); // Get the correct element

        Color a = indicatorMat.GetColor("_Colour");
        a.a = 0.0f; // set to half transparency


        float elapsed = 0f;
        float time = Time.time;

        while (elapsed < indicatorDuration)
        {
            elapsed += Time.deltaTime;

            indicatorMat.SetColor("_Colour", a); // Set the material colour

            a.a = Mathf.Lerp(0.0f, 1f, elapsed / indicatorDuration);  // increment Alpha

            //a.a += elapsed / indicatorDuration; // increment Alpha

            indicator.GetComponent<MeshRenderer>().material = indicatorMat; // Set the correct material

            yield return new WaitForEndOfFrame();
        }

        // Reset
        a.a = 0f;
        indicatorMat.SetColor("_Colour", a); // Set the material colour
        indicator.GetComponent<MeshRenderer>().material = indicatorMat; // Set the correct material
        indicatorHidden = true; // hide again
    }

	// Update is called once per frame
	void Update () {
	
	}

    // Setters / Getters
    #region properties

    public bool Hidden
    {
        get { return indicatorHidden; }
    }

    #endregion
}
