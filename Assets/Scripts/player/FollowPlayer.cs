using UnityEngine;
using System.Collections;

//相机跟随
public class FollowPlayer : MonoBehaviour {

    private Transform player;
    private Vector3 offsetPosition;//位置偏移
    private bool isRotating = false;


    public float distance = 0;
    public float scrollSpeed = 10;
    public float rotateSpeed = 2;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        transform.LookAt(player.position);
        offsetPosition = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = offsetPosition + player.position;
        
        RotateView();
        
        ScrollView();
	}

    //处理视野的拉近和拉远效果，滚动鼠标
    void ScrollView() {
        //向后返回负值(拉近视野)
        distance = offsetPosition.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
        distance = Mathf.Clamp(distance, 2, 18);
        offsetPosition = offsetPosition.normalized * distance;
    }

    //处理视野的旋转，鼠标右键
    void RotateView() {
        if (Input.GetMouseButtonDown(1)) {
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1)) {
            isRotating = false;
        }

        if (isRotating) {
            transform.RotateAround(player.position,player.up, rotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            //影响的属性有两个 一个是position 一个是rotation
            transform.RotateAround(player.position,transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;

            //超出范围后将属性归位，让旋转无效，就是把他固定在哪里 
            if (x < 10 || x > 80) {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

        }

        offsetPosition = transform.position - player.position;
    }
}
