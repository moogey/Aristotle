using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sphere : MonoBehaviour
{
    SphereInputProfile inputProfile;
    Rigidbody2D rigidbody2D;
    Vector2 oldForce;

    private float movementForce = 10;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        inputProfile = new SphereInputProfile();
        
        inputProfile.addListener(InputEvent.Key, SphereInputProfile.moveUp, moveUp);
        inputProfile.addListener(InputEvent.Key, SphereInputProfile.moveDown, moveDown);
        inputProfile.addListener(InputEvent.Key, SphereInputProfile.moveLeft, moveLeft);
        inputProfile.addListener(InputEvent.Key, SphereInputProfile.moveRight, moveRight);
        Controller.instance.AddEventListener(GameEventType.ENGINE_STATE_CHANGE, onStateChange);

    }

    private void onStateChange()
    {
        if (Controller.instance.state == Gamestate.MENU)
        {
            this.enabled = false;
            oldForce = rigidbody2D.velocity;
            rigidbody2D.Sleep(); // important store velocity BEFORE sleeping

        }
        else if (Controller.instance.state == Gamestate.ACTIVE)
        {
            this.enabled = true;
            rigidbody2D.WakeUp();
            rigidbody2D.velocity = oldForce; // apply velocity AFTER waking
        }
       
    }


    // Update is called once per frame
    void Update()
    {
        inputProfile.checkInput();

        if(Input.GetKeyUp(KeyCode.LeftControl))
            Controller.instance.RemoveEventListener(GameEventType.ENGINE_STATE_CHANGE, onStateChange);

    }

    private void moveUp()
    {
        rigidbody2D.AddForce(Vector2.up * movementForce);
        
    }

    private void moveDown()
    { 
        rigidbody2D.AddForce(Vector2.down * movementForce);
    }

    private void moveLeft()
    {
        rigidbody2D.AddForce(Vector2.left * movementForce);
    }

    private void moveRight()
    {
        rigidbody2D.AddForce(Vector2.right * movementForce);
    }
}
