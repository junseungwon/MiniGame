using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRope : MonoBehaviour
{
    [SerializeField]private GameObject centerPos = null;
    [SerializeField] private GameObject rope = null;
    [SerializeField] private float ropeSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotationRope();
    }
    private void RotationRope()
    {
        rope.transform.RotateAround(centerPos.transform.position, Vector3.left, ropeSpeed * Time.deltaTime);
    }
}
