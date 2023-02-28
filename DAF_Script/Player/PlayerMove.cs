using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    private Vector3 movement;
    private CharacterController controller;
    private PlayerView playerView;

    public GameObject cameraC;
    private Vector3 moveDir = Vector3.zero;
    private float gravity = 9.8f;
    private float moveH;
    private float moveV;
    private float rotateH;
    public float walkWidth = 0f;
    private float walkMax = 1.5f;
    private Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerView = GetComponent<PlayerView>();
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToUseController(PlayerView.instance.canControll);
        MoveToUseKeyboard(PlayerView.instance.canControll);
        WalkSE();

        DebugSave();
    }

    void FixPlayerPosition() {
        Vector3 posC = cameraC.transform.position;

        Vector3 playerPos = gameObject.transform.position;

        playerPos.x = posC.x;
        playerPos.z = posC.z;

        gameObject.transform.position = playerPos;
        cameraC.transform.position = posC;

        Vector3 cameraLocalPosition = cameraC.transform.localPosition;
        cameraLocalPosition.x = 0;
        cameraLocalPosition.z = 0;
        cameraC.transform.localPosition = cameraLocalPosition;
    }

    private void WalkSE() {
        //‘«‰¹—p
        float x = Mathf.Abs(transform.position.x - oldPos.x);
        float z = Mathf.Abs(transform.position.z - oldPos.z);
        walkWidth += x + z;
        if(walkWidth > walkMax) {
            SingletonGeneral.instance.PlayOneShot(playerView.audioSource, "NormalFoot");
            walkWidth = 0f;
        }
        oldPos = transform.position;
    }

    private void MoveToUseController(bool isControll) {
        if (!isControll) return;

        moveH = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        moveV = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
        rotateH = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        movement = new Vector3(moveH, 0, moveV);

        Vector3 desiredMove =
            cameraC.transform.forward * movement.z + 
            cameraC.transform.right * movement.x;
        moveDir.x = desiredMove.x * 3f;
        moveDir.z = desiredMove.z * 3f;
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime * speed);

        //‰ñ“]
        transform.Rotate(new Vector3(0, rotateH, 0));

    }

    private void MoveToUseKeyboard(bool isControll) {
        if (!isControll) return;

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

    void DebugSave() {
        if (Input.GetKeyUp(KeyCode.S)) {
            SingletonGeneral.instance.saveLoadSystem.Save();
        }
        if (Input.GetKeyUp(KeyCode.L)) {
            SingletonGeneral.instance.saveLoadSystem.Load();
        }
    }
}
