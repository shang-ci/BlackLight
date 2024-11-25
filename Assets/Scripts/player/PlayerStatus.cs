using UnityEngine;
using System.Collections;

public enum HeroType {
    Swordman,
    Magician
}

//player的状态
public class PlayerStatus : MonoBehaviour {

    public HeroType heroType;

    public int level = 1; // 100+level*30

    public string name = "默认名称";
    public int hp = 100;
    public int mp = 100;
    public float hp_remain = 100;
    public float mp_remain = 100;
    public float exp = 0;//当前已经获得的经验

    public float attack = 20;
    public int attack_plus = 0;
    public float def = 20;//防御值
    public int def_plus = 0;//加的点数
    public float speed = 20;
    public int speed_plus = 0;

    public int point_remain = 0;//剩余的点数，升级会获得点数，给属性加点

    void Start() {
        GetExp(0);
    }

    //治疗
    public void GetDrug(int hp,int mp) {
        hp_remain += hp;
        mp_remain += mp;
        if (hp_remain > this.hp) {
            hp_remain = this.hp;
        }
        if (mp_remain > this.mp) {
            mp_remain = this.mp;
        }
        HeadStatusUI._instance.UpdateShow();
    }

    //向外提供的功能，消耗一点属性点
    public bool GetPoint(int point=1) {
        if (point_remain >= point) {
            point_remain -= point;
            return true;
        }
        return false;
    }

    //获得经验值
    public void GetExp(int exp) {
        this.exp += exp;
        int total_exp = 100 + level * 30;//这里修改经验的逻辑可以变得更复杂
        while (this.exp >= total_exp) {
            //升级
            this.level++;
            point_remain += 10;
            this.exp -= total_exp;
            total_exp = 100 + level * 30;
        }

        ExpBar._instance.SetValue(this.exp/total_exp );
    }

    //耗蓝，使用技能时用
    public bool TakeMP(int count) {
        if (mp_remain >= count) {
            mp_remain -= count;
            HeadStatusUI._instance.UpdateShow();
            return true;
        } else {
            return false;
        }
    }
}
