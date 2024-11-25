using UnityEngine;
using System.Collections;

public class ShopDrugNPC : NPC {

    //弹出药品列表
    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<AudioSource>().Play();
            ShopDrug._instance.TransformState();
        }
    }
	
}
