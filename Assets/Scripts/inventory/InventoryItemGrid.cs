using UnityEngine;
using System.Collections;

public class InventoryItemGrid : MonoBehaviour {

    public  int id=0;
    private ObjectInfo info = null;
    public int num = 0;

    public UILabel numLabel;

	// Use this for initialization
	void Start () {
        numLabel = this.GetComponentInChildren<UILabel>();
	}

    //初始化一个格子，购买物品时要添加
    public void SetId(int id, int num = 1) {
        this.id = id;
        info = ObjectsInfo._instance.GetObjectInfoById(id);
        InventoryItem item = this.GetComponentInChildren<InventoryItem>();
        item.SetIconName(id,info.icon_name);
        numLabel.enabled = true;
        this.num = num;
        numLabel.text = num.ToString();
    }

    public void PlusNumber(int num = 1) {
        this.num += num;
        numLabel.text = this.num.ToString();
    }

    //默认减一，用来提供一个底层的方法，提供给背包使用，什么时候要减一
    public bool MinusNumber(int num = 1) {
        if (this.num >= num) {
            this.num -= num;
            numLabel.text = this.num.ToString();
            if (this.num == 0) {
                //要清空这个物品格子
                ClearInfo();//清空存储的信息 
                GameObject.Destroy(this.GetComponentInChildren<InventoryItem>().gameObject);//销毁物品格子
            }
            return true;
        }
        return false;
    }

    //清空格子物品信息
    public void ClearInfo() {
        id = 0;
        info = null;
        num = 0;
        numLabel.enabled = false;//使得组件失效
    }


}
