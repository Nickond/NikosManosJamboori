using UnityEngine;
using System.Collections;
using Global;
/// <summary>
/// Bolt Launching test
/// </summary>
public class BoltLauncher : MonoBehaviour
{
    [SerializeField]
    private SegmentManager segmentManager;

    public GameObject[] boltPrefabs;

	// Use this for initialization
	void Start () 
    {
        InvokeRepeating("Launch", 0f, 1f);
	}
	
    void Launch()
    {
        int selection = Random.Range(0, 3); // Select a prefab

        GameObject _boltObj = Instantiate(boltPrefabs[selection], transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;

        // Select a random segment
        Segment _segment = segmentManager.GetRandomSegment();

        if (_segment != null)
        {
            Bolt _bolt = _boltObj.GetComponent<Bolt>();

            _bolt.Initialise(_segment.transform.position);
            //_bolt.target = _segment.transform.position;
            //_bolt.centre = (transform.position + _bolt.target) * 0.5f;
            _segment.Indicate((Elements)selection, _bolt.Duration); // Show the indicator
        }
    }

	// Update is called once per frame
	void Update () 
    {
	
	}
}
