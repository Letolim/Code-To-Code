using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDmgTextManager : MonoBehaviour
{
    public GameObject player;
    public GameObject textTemplate;

    private List<FloatingDmgText> textObjects = new List<FloatingDmgText>();

    public void GeneratenDmgText(Vector3 origin, string text)
    {
        for (int i = 0; i < textObjects.Count; i++)
            if (!textObjects[i].gameObject.active)
            {
                SpawnDmgText(origin, text, textObjects[i]);
                return;
            }
        GameObject textObject = GameObject.Instantiate(textTemplate);
        textObjects.Add(textObject.GetComponent<FloatingDmgText>());
        SpawnDmgText(origin, text, textObjects[textObjects.Count -1]);
    }

    public void SpawnDmgText(Vector3 origin, string text, FloatingDmgText textObject)
    {
        textObject.gameObject.SetActive(true);
        textObject.StarTextDisplay(origin, text);              
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
