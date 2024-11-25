using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    //点击NPC鼠标改变样式
    void OnMouseEnter() {
        CursorManager._instance.SetNpcTalk();
    }

    void OnMouseExit() {
        CursorManager._instance.SetNormal();
    }

}
