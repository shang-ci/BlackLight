using UnityEngine;
using System.Collections;

public class EquipmentItem : MonoBehaviour {

    private UISprite sprite;
    public int id;
    private bool isHover = false;//检测到鼠标

    void Awake() {
        sprite = this.GetComponent<UISprite>();
    }

    void Update() {
        if (isHover) {
            if (Input.GetMouseButtonDown(1)) {
                EquipmentUI._instance.TakeOff(id,this.gameObject);//卸下装备
            }
        }
    }


    public void SetId(int id) {
        this.id = id;
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        SetInfo(info);
    }

    //更新显示
    public void SetInfo(ObjectInfo info) {
        this.id = info.id;

        sprite.spriteName = info.icon_name;//选择某图片集下的该名字的图片
    }

    //覆盖自动触发
    public void OnHover(bool isOver) {
        isHover = isOver;
    }

}
