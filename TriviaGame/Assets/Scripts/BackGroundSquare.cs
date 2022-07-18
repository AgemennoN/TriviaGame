using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSquare : MonoBehaviour
{
    private OptionCanvas OptionCanvas;
    private float speed;
    private float rotatingSpeed;
    private float scaling;
    void Start()
    {
        OptionCanvas = GameObject.Find("OptionCanvas").GetComponent<OptionCanvas>();

        DontDestroyOnLoad(transform.gameObject);
        speed = Random.Range(0.5f, 2.5f);
        rotatingSpeed = Random.Range(-1.5f, 1.5f);

        scaling = Random.Range(0.5f, 2.5f);
        transform.localScale = transform.localScale * scaling;

        Color theColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 100f / 255f);
        gameObject.GetComponent<Renderer>().material.color = theColor;

    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * rotatingSpeed, Space.Self);

        if (transform.position.y > 8 || OptionCanvas.TglDynamicBackground == false)
        {
            Object.Destroy(gameObject);
        }
    }


}
