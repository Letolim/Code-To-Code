using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDmgText : MonoBehaviour
{
    public GameObject player;
    public TextMesh textMesh = new TextMesh();
    public Color textColor;
    private Timer textTimer = new Timer();
    private Vector3 offset;

    public void StarTextDisplay(Vector3  origin,string text)
    {
        this.transform.position = origin + Vector3.up;
        textTimer.Start(3f);
        textMesh.text = text;
    }


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(Random.value * 2f, 2f, Random.value * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        textTimer.Update();
        textColor.a = 1f - textTimer.Delta();
        textMesh.color = textColor;
        this.transform.position += offset * Time.deltaTime * 3f;

        this.transform.rotation = Quaternion.LookRotation(player.transform.position - Camera.main.transform.position);

        if (textTimer.finished)
            this.gameObject.SetActive(false);
    }
}
