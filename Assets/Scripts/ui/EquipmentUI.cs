using UnityEngine;
using System.Collections;

public class EquipmentUI : MonoBehaviour {

    public static EquipmentUI _instance;
    private TweenPosition tween;
    private bool isShow = false;

    //装备的分类（穿戴类型）用来作为装备的父类
    private GameObject headgear;
    private GameObject armor;
    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject shoe;
    private GameObject accessory;

    private PlayerStatus ps;//用来获得当前角色的类型，判断装备是否可以穿带

    public GameObject equipmentItem;//预制件，就是个equipmentitem

    //装备的加成
    public int attack = 0;
    public int def = 0;
    public int speed = 0;

    //升级点数在这其效果
    public int attack_plus = 0;
    public int def_plus = 0;
    public int speed_plus = 0;


    void Awake() {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();

        //感觉这样子获取有点不够优雅，最好有个管理的，把获取的部分分离，逻辑分离
        headgear = transform.Find("Headgear").gameObject;
        armor = transform.Find("Armor").gameObject;
        rightHand = transform.Find("RightHand").gameObject;
        leftHand = transform.Find("LeftHand").gameObject;
        shoe = transform.Find("Shoe").gameObject;
        accessory = transform.Find("Accessory").gameObject;

        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }

    //这里处理装备的加成，后续的升级属性点也直接在这里使用
    void Update()
    {
        //这里一直更新装备加成，导致升级点数不起效果
        UpdateProperty();
        ps.attack_plus = attack + attack_plus;
        ps.def_plus = def + def_plus;
        ps.speed_plus = speed + speed_plus;
    }

    public void TransformState() {
        if (isShow == false) {
            tween.PlayForward();
            isShow = true;
        } else {
            tween.PlayReverse();
            isShow = false;
        }
    }

    //处理装备穿戴功能
    public bool Dress(int id) {
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        if (info.type != ObjectType.Equip) {
            return false;//穿戴失败
        }
        if (ps.heroType == HeroType.Magician) {
            if (info.applicationType == ApplicationType.Swordman) {
                return false;
            }
        }
        if (ps.heroType == HeroType.Swordman) {
            if (info.applicationType == ApplicationType.Magician) {
                return false;
            }
        }

        GameObject parent = null;//用来判断装备的类型，判断有没有同类型的装备在身上，方便替换

        switch (info.dressType) {
            case DressType.Headgear:
                parent = headgear;
                break;
            case DressType.Armor:
                parent = armor;
                break;
            case DressType.RightHand:
                parent = rightHand;
                break;
            case DressType.LeftHand:
                parent = leftHand;
                break;
            case DressType.Shoe:
                parent = shoe;
                break;
            case DressType.Accessory:
                parent = accessory;
                break;
        }
        EquipmentItem item = parent.GetComponentInChildren<EquipmentItem>();
        if (item != null) {//已经穿戴了同样类型的装备
            Inventory._instance.GetId(item.id);//把已经穿戴的装备卸下，放回背包
            item.SetInfo(info);//更新sprite的显示
        } else {
            GameObject itemGo =  NGUITools.AddChild(parent, equipmentItem);//在父类下新建个equipmentItem
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.GetComponent<EquipmentItem>().SetInfo(info);
        }
        //UpdateProperty();

        return true;
    }

    //脱下装备
    public void TakeOff(int id,GameObject go) {
        Inventory._instance.GetId(id);
        GameObject.Destroy(go);
        //UpdateProperty();

        ps.attack_plus -= attack;
        ps.def_plus -= def;
        ps.speed_plus -= speed;
    }
    
    //更新装备的加成属性
    void UpdateProperty() {
        this.attack = 0;
        this.def = 0;
        this.speed = 0;

        EquipmentItem headgearItem = headgear.GetComponentInChildren<EquipmentItem>();
        PlusProperty(headgearItem);
        EquipmentItem armorItem = armor.GetComponentInChildren<EquipmentItem>();
        PlusProperty(armorItem);
        EquipmentItem leftHandItem = leftHand.GetComponentInChildren<EquipmentItem>();
        PlusProperty(leftHandItem);
        EquipmentItem rightHandItem = rightHand.GetComponentInChildren<EquipmentItem>();
        PlusProperty(rightHandItem);
        EquipmentItem shoeItem = shoe.GetComponentInChildren<EquipmentItem>();
        PlusProperty(shoeItem);
        EquipmentItem accessoryItem = accessory.GetComponentInChildren<EquipmentItem>();
        PlusProperty(accessoryItem);


    }

    void PlusProperty(EquipmentItem item) {
        if (item != null) {
            ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoById(item.id);
            this.attack += equipInfo.attack;
            this.def += equipInfo.def;
            this.speed += equipInfo.speed;
        }
    }


}
