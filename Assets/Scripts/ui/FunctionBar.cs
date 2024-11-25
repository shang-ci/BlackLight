using UnityEngine;
using System.Collections;

//管理右下角的几个功能
public class FunctionBar : MonoBehaviour {

    public void OnStatusButtonClick() {
        Status._instance.TransformState();
    }

    public void OnBagButtonClick() {
        Inventory._instance.TransformState();
    }

    public void OnEquipButtonClick() {
        EquipmentUI._instance.TransformState();
    }

    public void OnSkillButtonClick() {
        SkillUI._instance.TransformState();
    }

    //暂时没有
    public void OnSettingButtonClick() {
    }


}
