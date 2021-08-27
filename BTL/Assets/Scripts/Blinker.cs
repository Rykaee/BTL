using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    // Blinking interval in seconds
    public float blinkIntervalInSeconds = 0.5f;

    // Array of colors. Defaults to white and black if empty.
    public Color[] colors;

    // States for blinking.
    private enum State { Sleeping, Blinking, Stopping };
    private State currentState;

    // Attributes for storing current state.
    private float accumulatorInSeconds = 0.0f;
    private int currentColorIndex = -1;
    private int nextColorIndex = -1;
    private SpriteRenderer rndr;

    // Start is called before the first frame update
    void Start()
    {
        // Use white and black colors if colors array is empty.
        if (colors == null)
        {
            colors = new Color[2] { new Color(1.0f, 1.0f, 1.0f), new Color(0.5f, 0.5f, 0.5f) };
        }

        accumulatorInSeconds = 0.0f;
        currentColorIndex = 0;
        nextColorIndex = 1;

        // Cache Renderer-component for GameObject to increase performance.
        rndr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = 0.0f;
        Color a, b, c;

        // Determine what to do in current state
        switch(currentState)
        {
            case State.Sleeping:
                // Do nothing in this state
                break;

            case State.Blinking:
                // Do blinking
                accumulatorInSeconds += Time.deltaTime;
                if (accumulatorInSeconds >= blinkIntervalInSeconds)
                {
                    // Modulate through all colors.
                    currentColorIndex = (currentColorIndex + 1) % colors.Length;
                    nextColorIndex = (currentColorIndex + 1) % colors.Length;
                    accumulatorInSeconds = 0.0f;
                }

                // Calculate interpolated color for this frame
                t = accumulatorInSeconds / blinkIntervalInSeconds;
                a = colors[currentColorIndex];
                b = colors[nextColorIndex];
                c = Color.Lerp(a, b, t);
                rndr.color = c;
                break;


            case State.Stopping:
                // Stopping.
                // When current color index is initial color index
                // then stop completely.
                accumulatorInSeconds += Time.deltaTime;
                if (accumulatorInSeconds >= blinkIntervalInSeconds)
                {
                    currentColorIndex = (currentColorIndex + 1) % colors.Length;
                    nextColorIndex = (currentColorIndex + 1) % colors.Length;
                    accumulatorInSeconds = 0.0f;

                    // Stop at initial color
                    if (currentColorIndex == 0)
                    {
                        currentState = State.Sleeping;
                    }
                }

                // Calculate interpolated color for this frame
                t = accumulatorInSeconds / blinkIntervalInSeconds;
                a = colors[currentColorIndex];
                b = colors[nextColorIndex];
                c = Color.Lerp(a, b, t);
                rndr.color = c;
                break;

            default:
                break;
        }
    }

    public bool IsSleeping()
    {
        return currentState == State.Sleeping;
    }

    public bool IsBlinking()
    {
        return (currentState == State.Blinking);
    }

    public bool IsStopping()
    {
        return (currentState == State.Stopping);
    }

    public void StartBlinking()
    {
        if(currentState == State.Sleeping || currentState == State.Stopping)
        {
            currentState = State.Blinking;
        }
    }

    public void StopBlinking()
    {
        if(currentState == State.Blinking)
        {
            currentState = State.Stopping;
        }
    }
}
