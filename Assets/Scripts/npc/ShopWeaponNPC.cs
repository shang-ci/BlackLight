using UnityEngine;
using System.Collections;

public class ShopWeaponNPC : NPC {
    //弹出武器商店
    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<AudioSource>().Play();
            ShopWeaponUI._instance.TransformState();
        }
    }
}
