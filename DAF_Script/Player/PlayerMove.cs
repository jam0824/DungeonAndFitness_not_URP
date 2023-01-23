using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    private Vector3 movement;
    private CharacterController controller;

    public GameObject cameraC;
    private Vector3 moveDir = Vector3.zero;
    private float gravity = 9.8f;
    private float moveH;
    private float moveV;
    private float rotateH;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToUseController();
        MoveToUseKeyboard();

    }

    private void MoveToUseController() {
        moveH = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        moveV = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
        rotateH = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        movement = new Vector3(moveH, 0, moveV);

        Vector3 desiredMove =
            cameraC.transform.forward * movement.z + cameraC.transform.right * movement.x;
        moveDir.x = desiredMove.x * 3f;
        moveDir.z = desiredMove.z * 3f;
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime * speed);

        //‰ñ“]
        transform.Rotate(new Vector3(0, rotateH, 0));
    }

    private void MoveToUseKeyboard() {
        moveH = 0.0f;
        moveV = 0.0f;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(new Vector3(0, -2, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(new Vector3(0, 2, 0));
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            moveV = -1.0f;
        }
        else if (Input.GetKey(KeyCode.UpArrow)) {
            moveV = 1.0f;
        }

        movement = new Vector3(moveH, 0, moveV);

        Vector3 desiredMove =
            cameraC.transform.forward * movement.z + cameraC.transform.right * movement.x;
        moveDir.x = desiredMove.x * 3f;
        moveDir.z = desiredMove.z * 3f;
        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime * speed);

    }
}
