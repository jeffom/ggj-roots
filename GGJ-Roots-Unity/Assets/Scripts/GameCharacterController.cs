using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacterController : MonoBehaviour
{   
    [SerializeField] private float m_horizontalSpeed;
    [SerializeField] private float m_verticalSpeed;

    Rigidbody m_rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));        
        Vector3 movement = new Vector3(0, direction.y * m_verticalSpeed * Time.deltaTime, direction.x * m_horizontalSpeed * Time.deltaTime);
        bool isMovementAllowed = !Physics.Raycast(transform.position, direction, movement.sqrMagnitude);
        if (isMovementAllowed)
        {
            transform.Translate(movement);
        }
    }
}
