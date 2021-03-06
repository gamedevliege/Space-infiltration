﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heroinv2 : MonoBehaviour
{
    #region Public Members

    [Range(0,100)]
    public float m_thrust;
    [Range(0,500)]
    public float m_jumpThrust;
    public int m_maxJumpCount;
    public Camera m_camera;
    public bool m_debugActive;
    public bool m_brake;
    public float m_distanceToGround;
    

    public enum e_CharacterState
    {
        INVALID = -1,
        STANDING,
        MOVING,
        FORWARD,
        BACKWARD,
        STRAFELEFT,
        STRAFERIGHT,
        ROTATELEFT,
        ROTATERIGHT,

        JUMPING,
        DOUBLEJUMPING,
        DIVING,
        DUCKING,

        MAX
    }

    public e_CharacterState m_characterState = e_CharacterState.STANDING;

    #endregion

    #region Public void

    #endregion

    #region System

    void Awake()
    {
        m_transform = GetComponent<Transform>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_jumpCountLeft = m_maxJumpCount;
    }

    void Update()
    {
        switch (m_characterState)
        {
            case e_CharacterState.STANDING:
                IsItMoving();
                IsItJumping();
                IsItDucking();
                break;

            case e_CharacterState.MOVING:

                break;

            case e_CharacterState.FORWARD:
                // toutes actions liés à standing
                IsItMoving();
                IsItDucking();
                m_rigidbody.AddForce(m_transform.forward * m_thrust);
                // ClampMagnitude limite la velocity à m_maxSpeed
                m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_maxSpeed);

                //Vérifier si on change d'etat.
                if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Z))
                {
                    Debug.Log("up arrow key is held up");
                    m_characterState = e_CharacterState.STANDING;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space arrow key is held down");
                    m_characterState = e_CharacterState.JUMPING;
                }
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.BACKWARD:
                // toutes actions liés à standing
                IsItMoving();
                IsItDucking();
                m_rigidbody.AddForce(m_transform.forward * - m_thrust);
                m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_maxSpeed);

                //Vérifier si on change d'etat.
                if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
                {
                    Debug.Log("down arrow key is held up");
                    m_characterState = e_CharacterState.STANDING;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space arrow key is held down");
                    m_characterState = e_CharacterState.JUMPING;
                }
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.STRAFELEFT:
                // toutes actions liés à standing
                IsItMoving();
                IsItDucking();
                m_rigidbody.AddForce(m_transform.right * - m_thrust);
                m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_maxSpeed);

                //Vérifier si on change d'etat.
                if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.Q))
                {
                    Debug.Log("left arrow key is held up");
                    m_characterState = e_CharacterState.STANDING;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space arrow key is held down");
                    m_characterState = e_CharacterState.JUMPING;
                }
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.STRAFERIGHT:
                // toutes actions liés à standing
                IsItMoving();
                IsItDucking();
                m_rigidbody.AddForce(m_transform.right * m_thrust);
                m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_maxSpeed);

                //Vérifier si on change d'etat.
                if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
                {
                    Debug.Log("right arrow key is held up");
                    m_characterState = e_CharacterState.STANDING;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space arrow key is held down");
                    m_characterState = e_CharacterState.JUMPING;
                }
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.JUMPING:
                // toutes actions liés à jumping
                /*if (m_isJumping == false)
                {
                    m_rigidbody.AddForce(m_transform.up * m_jumpThrust);
                    m_isJumping = true;
                }*/
                //Vérifier si on change d'etat.
                IsItJumping();
                IsItMoving();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space arrow key is held down");
                    m_characterState = e_CharacterState.DOUBLEJUMPING;
                }

                else if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))){
                    Debug.Log("dive key is held down");
                    m_characterState = e_CharacterState.DIVING;
                }

                
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.DOUBLEJUMPING:
                // toutes actions liés à jumping
                if (m_isDoubleJumping == false)
                {
                    m_rigidbody.AddForce(m_transform.up * 500);
                    m_isDoubleJumping = true;
                }

                if((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))){
                    Debug.Log("dive key is held down");
                    m_characterState = e_CharacterState.DIVING;
                }


                //Vérifier si on change d'etat.
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.ROTATELEFT:
                // toutes actions liés à standing
                m_transform.Rotate(Vector3.down * 2);
                IsItDucking();
                //Vérifier si on change d'etat.
                if (Input.GetKeyUp(KeyCode.A))
                {
                    Debug.Log("rotate left key is held up");
                    m_characterState = e_CharacterState.STANDING;
                }
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.ROTATERIGHT:
                // toutes actions liés à standing
                m_transform.Rotate(Vector3.up * 2);
                IsItDucking();
                //Vérifier si on change d'etat.
                if (Input.GetKeyUp(KeyCode.E))
                {
                    Debug.Log("rotate right key is held up");
                    m_characterState = e_CharacterState.STANDING;
                }
                // -> alors e_characterState : new characterState
                break;

            case e_CharacterState.DIVING:
                // toutes actions liés à diving
                m_rigidbody.AddForce(m_transform.up * -100);

                //Vérifier si on change d'etat.
                // -> alors m_characterState : new characterState
                break;
        }
    }

    #endregion

    #region Tools Debug And Utility

    private void OnGUI()
    {
        {
            if (m_debugActive)
            {
                GUILayout.Button("Player Debug");
                GUILayout.Button("Position : " + m_transform.position);
                GUILayout.Button("Velocity : " + m_rigidbody.velocity);
                GUILayout.Button("State : " + m_characterState);
            }
        }
    }

    #endregion

    #region

    private void OnCollisionEnter(Collision plane)
    {
        if (plane.gameObject.tag == ("Floor"))
        {
            // Equivalent "Any State" -> Le trigger collision déclanche l'état STANDING
            m_isJumping = false;
            m_isDoubleJumping = false;
            m_characterState = e_CharacterState.STANDING;
        }
    }

    private void IsItMoving()
    {
        Vector3 newPos = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        newPos *= m_thrust;
        Debug.Log("Move");

        if (m_brake && newPos == Vector3.zero)
        {
            Vector3 preserveYVelocity = m_rigidbody.velocity;
            preserveYVelocity.x = 0;
            preserveYVelocity.z = 0;
            m_rigidbody.velocity = Vector3.zero;
        }

        m_rigidbody.AddForce(newPos);

        Vector3 velocity = m_rigidbody.velocity;
        if(Mathf.Abs(velocity.x) > m_maxSpeed)
        {
            velocity.x = Mathf.Sign(velocity.x) * m_maxSpeed;
        }

        if(!m_isJumping && m_rigidbody.velocity != Vector3.zero)
        {
            m_characterState = e_CharacterState.MOVING;
        }
        else
        {
            m_characterState = e_CharacterState.STANDING;
        }
    }

    private void IsItJumping()
    {

        if (Input.GetButtonDown("Jump") && m_jumpCountLeft > 0)
        {
            m_jumpCountLeft--;
            m_rigidbody.AddForce(new Vector3(0, m_jumpThrust, 0));
        }

        RaycastHit hit;
        if(Physics.Raycast(m_transform.position, Vector3.down, out hit, m_distanceToGround))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                m_jumpCountLeft = m_maxJumpCount;
                m_characterState = e_CharacterState.JUMPING;
            }
        }
        else
        {
            m_characterState = e_CharacterState.STANDING;
        }
{

        }

    }

    /*private void IsItWalking()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            m_maxSpeed = 1;
        }
        else if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt))
        {
            m_maxSpeed = 6;
        }
        else
        {
            m_maxSpeed = 3;
        }
    }*/

    private void IsItDucking()
    {
        if (Input.GetKey(KeyCode.LeftControl) || (Input.GetKey(KeyCode.RightControl)))
        {
            if (m_isDucking == false)
            {
                Debug.Log("ctrl arrow key is held down");
                transform.localScale = new Vector3(1, 0.5f, 1);
                m_transform.position += new Vector3(0, -.25f, 0);
                m_isDucking = true;
                // m_camera.transform.position = new Vector3(m_camera.transform.position.x, 0.5f, m_camera.transform.position.z);
            }
        }
        else
        {
            if (m_isDucking == true)
            {
                Debug.Log("ctrl arrow key is held up");
                transform.localScale = new Vector3(1, 1, 1);
                m_transform.position += new Vector3(0, .25f, 0);
                m_isDucking = false;
                // m_camera.transform.position = new Vector3(m_camera.transform.position.x, 1f, m_camera.transform.position.z);
            }
        }
    }

    #endregion

    #region Private an Protected Members

    private Transform m_transform;
    private Rigidbody m_rigidbody;
    private Collider m_collider;
    private bool m_isJumping = false;
    private bool m_isDoubleJumping = false;
    private int m_jumpCountLeft;
    private bool m_isDucking = false;
    private float m_maxSpeed = 3;

    #endregion
}