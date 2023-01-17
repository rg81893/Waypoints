using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] float GizmosRadius;
    private IEnumerator coroutine;
    private GameObject popedObject;
    private List<GameObject> waypoints;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint").OrderBy(x => x.name).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && popedObject is null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "PointA")
                {
                    Debug.Log("PointA");
                    // faire pop un cube
                    popedObject = Instantiate(hit.collider.gameObject, waypoints[0].transform.position, Quaternion.identity);
                    popedObject.GetComponent<Renderer>().material.color = Color.white;

                    StartCoroutine(MoveObject(popedObject));

                }
                else if (hit.collider.tag == "PointB")
                {
                    Debug.Log("PointB");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, GizmosRadius);
    }

    IEnumerator MoveObject(GameObject objectToMove)
    {
        foreach (var wp in waypoints)
        {
            objectToMove.transform.position = new Vector3(wp.transform.position.x, wp.transform.position.y, wp.transform.position.z);
            //waypoints.RemoveAt(0);
            yield return new WaitForSeconds(2);
            Debug.Log("On est à la position X : " + objectToMove.transform.position.x + ", Y : " + objectToMove.transform.position.y + ", Z : " + objectToMove.transform.position.z);
        }
        Destroy(popedObject);
        popedObject = null;
    }
}
