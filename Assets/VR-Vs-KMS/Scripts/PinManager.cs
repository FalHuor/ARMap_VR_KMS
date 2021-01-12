using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PinManager : MonoBehaviour
{
    Camera ARCamera;
    public Transform LevelTm;
    public Object PinRed;
    public Object PinBlue;
    public Object PinGreen;
    private Object selectedPin;
    public Transform PinsCategories;
    public Transform ContaminationTm;
    public Transform ThrowableTm;
    public Transform SpawnpointTm;
    public InputField InputField;
    private Transform selectedTm;
    private string json = "";
    private string fileName;

    // Start is called before the first frame update
    void Start()
    {
        ARCamera = GetComponent<Camera>();
        selectedPin = PinGreen;
        selectedTm = SpawnpointTm;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (!EventSystem.current.IsPointerOverGameObject()) 
                GetMousePosition();
    }

    public void GetMousePosition()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        Ray ray = ARCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Pin"))
            {
                Destroy(hit.transform.parent.gameObject);
            } 
            else if (hit.transform.CompareTag("Floor"))
            {
                Vector3 localHit = LevelTm.InverseTransformPoint(hit.point);
                //Debug.Log("Hit position 'local' : " + localHit);
                //Transform T = new Transform(localHit, Vector3.zero, new Vector3(1, 1, 1));
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                GameObject newPin = (GameObject)Instantiate(selectedPin, hit.point, LevelTm.rotation);
                newPin.transform.SetParent(selectedTm);
                //Debug.Log("coord g : " + newPin.transform.position);
                //Debug.Log("pin at : " + LevelTm.transform.InverseTransformPoint(newPin.transform.position));
            } 
            else
            {
                Debug.Log("Unvalid Location");
            }
        }
        else
        {
            Debug.DrawRay(ARCamera.transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
        }

    }

    public void ChangeValue(string toggle)
    {
        switch (toggle)
        {
            case "red":
                selectedPin = PinRed;
                selectedTm = ContaminationTm;
                break;
            case "blue":
                selectedPin = PinBlue;
                selectedTm = ThrowableTm;
                break;
            case "green":
                selectedPin = PinGreen;
                selectedTm = SpawnpointTm;
                break;
        }

                
    }

    public void ResetClicked()
    {
        foreach(Transform PinCategorie in PinsCategories)
        {
            foreach(Transform child in PinCategorie.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ExportClicked()
    {

        Container container = new Container();

        foreach (Transform child in ContaminationTm.transform)
        {
            Vector3 localposition = LevelTm.InverseTransformPoint(child.position);
            Pin pin = new Pin(localposition.x, localposition.y, localposition.z);
            container.ContaminationArea.Add(pin);
        }

        foreach (Transform child in ThrowableTm.transform)
        {
            Vector3 localposition = LevelTm.InverseTransformPoint(child.position);
            Pin pin = new Pin(localposition.x, localposition.y, localposition.z);
            container.ThrowableObject.Add(pin);
        }

        foreach (Transform child in SpawnpointTm.transform)
        {
            Vector3 localposition = LevelTm.InverseTransformPoint(child.position);
            Pin pin = new Pin(localposition.x, localposition.y, localposition.z);
            container.SpawnPoint.Add(pin);
        }

        Debug.Log(InputField.text);

        if (InputField.text == null || InputField.text == "")
        {
            fileName = "Level";
        } 
        else 
        {
            fileName = InputField.text;
        }

        string json = JsonUtility.ToJson(container, true);
        Debug.Log(Application.streamingAssetsPath + "/" + fileName + ".json");
        File.WriteAllText(Application.streamingAssetsPath + "/" + fileName + ".json", json);

        Debug.Log(json);
    }
}
