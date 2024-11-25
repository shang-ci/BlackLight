using UnityEngine;
using System.Collections;
using System.Diagnostics;

//控制player的方向
public class PlayerDir : MonoBehaviour {

    public float rotationSpeed = 500f; // 角色旋转速度
    public GameObject effect_click_prefab;
    public Vector3 targetPosition = Vector3.zero;
    public bool isMoving = false;//鼠标是否按下，长按会一直改变方向
    private PlayerMove playerMove;
    private PlayerAttack attack;

    void Start() {
        targetPosition = transform.position;
        playerMove = this.GetComponent<PlayerMove>();
        attack = this.GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if (attack.state == PlayerState.Death) return;

        //这一部分是应为鼠标点击地面不起作用，用键盘来改变状态达到移动效果，player move不做改动
        HandleKeyboardInput();

        if (attack.isLockingTarget == false && Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);

            if (isCollider && hitInfo.collider.tag == Tags.ground)
            {
                isMoving = true;
                ShowClickEffect(hitInfo.point);
                LookAtTarget(hitInfo.point);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }

        //鼠标不松手，也一直改变方向
        if (isMoving)
        {
            //得到目标位置，通过射线来获得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            bool isCollider = Physics.Raycast(ray, out hitInfo);

            if (isCollider && hitInfo.collider.tag == Tags.ground)
            {
                LookAtTarget(hitInfo.point);
            }

        }
        else
        {
            if (playerMove.isMoving)
            {
                LookAtTarget(targetPosition);
            }
        }
    }

    // 处理键盘输入的控制
    void HandleKeyboardInput()
    {
        float horizontal = -Input.GetAxis("Horizontal"); // A/D 或 左/右箭头
        float vertical = -Input.GetAxis("Vertical"); // W/S 或 上/下箭头

        // 计算移动方向
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // 目标方向
            targetPosition = transform.position + direction;

            // 设置朝向目标方向
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 移动
            playerMove.isMoving = true; // 启动移动
        }
        else
        {
            playerMove.isMoving = false; // 停止移动
        }
    }

    //实例化出来点击的效果
    void ShowClickEffect(Vector3 hitPoint)
    {
        hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.1f, hitPoint.z);
        GameObject.Instantiate(effect_click_prefab, hitPoint, Quaternion.identity);
    }

    //让主角朝向目标位置
    void LookAtTarget(Vector3 hitPoint)
    {
        targetPosition = hitPoint;
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.LookAt(targetPosition);
    }
}

