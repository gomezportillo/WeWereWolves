using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectDisableWhenKeyPressed : MonoBehaviour
{

    [SerializeField]
    public string key = "space";
    public GameObject objectToDisable;
    public List<GameObject> objectsToEnable;

    void Start()
    {
        Flip(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Flip(false);
        }
    }

    void Flip(bool initial)
    {
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(initial);
        }

        foreach (GameObject go in objectsToEnable)
        {
            go.SetActive(!initial);
        }
    }
}
