using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//管理所有的物品
public class ObjectsInfo : MonoBehaviour {

    public static ObjectsInfo _instance;

    private Dictionary<int, ObjectInfo> objectInfoDict = new Dictionary<int, ObjectInfo>();//存放物品

    public TextAsset objectsInfoListText;//通过txt文件定义物品

    void Awake() {
        _instance = this;
        ReadInfo();
    }

    //通过id表示获取物品
    public ObjectInfo GetObjectInfoById(int id) {
        ObjectInfo info=null;

        objectInfoDict.TryGetValue(id, out info);

        return info;
    }

    //读取我们配置好的txt，把他们塞到字典里方便获取
    void ReadInfo() {
        string text = objectsInfoListText.text;
        string[] strArray = text.Split('\n');

        foreach (string str in strArray) {
            string[] proArray = str.Split(',');
            ObjectInfo info = new ObjectInfo();

            int id = int.Parse(proArray[0]);
            string name = proArray[1];
            string icon_name = proArray[2];
            string str_type = proArray[3];
            ObjectType type = ObjectType.Drug;

            switch (str_type) {
                case "Drug":
                    type = ObjectType.Drug;
                    break;
                case "Equip":
                    type = ObjectType.Equip;
                    break;
                case "Mat":
                    type = ObjectType.Mat;
                    break;
            }

            info.id = id;
            info.name = name;
            info.icon_name = icon_name;
            info.type = type;

            if (type == ObjectType.Drug) {
                int hp = int.Parse(proArray[4]);
                int mp = int.Parse(proArray[5]);
                int price_sell = int.Parse(proArray[6]);
                int price_buy = int.Parse(proArray[7]);
                info.hp = hp; info.mp = mp;
                info.price_buy = price_buy;
                info.price_sell = price_sell;
            } else if (type == ObjectType.Equip) {
                info.attack = int.Parse(proArray[4]);
                info.def = int.Parse(proArray[5]);
                info.speed = int.Parse(proArray[6]);
                info.price_sell = int.Parse(proArray[9]);
                info.price_buy = int.Parse(proArray[10]);
                string str_dresstype = proArray[7];//装备类型

                switch (str_dresstype) {
                    case "Headgear":
                        info.dressType = DressType.Headgear;
                        break;
                    case "Armor":
                        info.dressType = DressType.Armor;
                        break;
                    case "LeftHand":
                        info.dressType = DressType.LeftHand;
                        break;
                    case "RightHand":
                        info.dressType = DressType.RightHand;
                        break;
                    case "Shoe":
                        info.dressType = DressType.Shoe;
                        break;
                    case "Accessory":
                        info.dressType = DressType.Accessory;
                        break;
                }
                string str_apptype = proArray[8];//适用类型

                switch (str_apptype) {
                    case "Swordman":
                        info.applicationType = ApplicationType.Swordman;
                        break;
                    case "Magician":
                        info.applicationType = ApplicationType.Magician;
                        break;
                    case "Common":
                        info.applicationType = ApplicationType.Common;
                        break;
                }

            }

            objectInfoDict.Add(id, info);
        }
    }
	
}



/**
id
名称
icon名称
类型
血量值         伤害（攻击值）
魔法值         防御值
出售价         速度
购买价         穿戴类型
               适用类型
               出售价
               购买价
 */


//物品三个类型
public enum ObjectType {
    Drug,
    Equip,
    Mat
}

//装备类型
public enum DressType {
    Headgear,//帽子
    Armor,
    RightHand,
    LeftHand,
    Shoe,
    Accessory//饰品
}

//适用类型（玩家选择的角色）
public enum ApplicationType{
    Swordman,//剑士
    Magician,//魔法师
    Common//通用
}

//物品属性
public class ObjectInfo {
    public int id;//标识
    public string name;
    public string icon_name;//图集中的名字
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int attack;
    public int def;
    public int speed;
    public DressType dressType;
    public ApplicationType applicationType;
}