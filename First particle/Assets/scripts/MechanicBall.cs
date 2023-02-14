using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicBall : MonoBehaviour
{
    public enum States { Bounce, Float, Expand, Reduce }
    [SerializeField] private States state;
    private Rigidbody rgbody;
    private Renderer render;
    private Vector3 sizeDefault;
    private Vector3 sizeExpand;
    private Vector3 sizeReduce;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve curve2;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color floatColor;
    private float time;
    void Awake()
    {
        rgbody = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        state = States.Bounce;
        render.material.color = defaultColor;
        sizeDefault = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.Bounce:
                transform.localScale = sizeDefault;
                render.material.color = defaultColor;
                sizeExpand = sizeDefault;


                if (Input.GetKeyDown(KeyCode.Space))
                    state = States.Float;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                    state = States.Expand;

                }
                break;
            case States.Float:
                rgbody.isKinematic = true;
                render.material.color = floatColor;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    state = States.Bounce;
                    rgbody.isKinematic = false;
                    render.material.color = defaultColor;
                }
                break;
            case States.Expand:
                time += Time.deltaTime;
                sizeExpand = new Vector3(curve.Evaluate(time), curve.Evaluate(time), curve.Evaluate(time));
                transform.localScale = sizeExpand;
                if (Input.GetKeyUp(KeyCode.E))
                {
                    state = States.Reduce;
                }
                break;
            case States.Reduce:

                    time += Time.deltaTime;
                    sizeReduce = new Vector3(curve2.Evaluate(time), curve2.Evaluate(time), curve2.Evaluate(time));
                    transform.localScale = sizeReduce;
                    state = States.Bounce;
                
                break;
        }
    }
}
