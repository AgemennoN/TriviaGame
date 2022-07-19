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
        // Finds OptionCanvas to check if TglDynamicBackground is true
        OptionCanvas = GameObject.Find("OptionCanvas").GetComponent<OptionCanvas>();

        // Squares can live even the scenes change
        DontDestroyOnLoad(transform.gameObject);

        SquareRandomizer();

        
    }

    private void SquareRandomizer() // Randomize the square's speed, size and color
    {
        speed = Random.Range(0.5f, 2.5f);
        rotatingSpeed = Random.Range(-1.5f, 1.5f);

        scaling = Random.Range(0.5f, 2.5f);
        transform.localScale = transform.localScale * scaling;

        Color theColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 100f / 255f);
        gameObject.GetComponent<Renderer>().material.color = theColor;

    }

    void FixedUpdate()  
    {
        // Make the square rise and rotate
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * rotatingSpeed, Space.Self);

        // Destroy the square when it's out of the screen
        if (transform.position.y > 8 || OptionCanvas.TglDynamicBackground == false)
        {
            Object.Destroy(gameObject);
        }
    }


}
