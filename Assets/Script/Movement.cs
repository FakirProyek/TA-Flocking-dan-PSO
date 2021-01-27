using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 _speed = gameObject.transform.position;
            _speed.x -= 0.1f;
            gameObject.transform.position = _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 _speed = gameObject.transform.position;
            _speed.x += 0.1f;
            gameObject.transform.position = _speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 _speed = gameObject.transform.position;
            _speed.y += 0.1f;
            gameObject.transform.position = _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 _speed = gameObject.transform.position;
            _speed.y -= 0.1f;
            gameObject.transform.position = _speed;
        }
    }
}
