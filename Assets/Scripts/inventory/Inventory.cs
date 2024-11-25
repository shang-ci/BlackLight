using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public static Inventory _instance;
    
    private TweenPosition tween;//动画控制
    private int coinCount = 1000;//金币数量

    public List<InventoryItemGrid> itemGridList = new List<InventoryItemGrid>();//背包的格子
    public UILabel coinNumberLabel;//每个格子的数目标签
    public GameObject inventoryItem;

    void Awake() {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
    }

    void Update() {
        //获得
        if (Input.GetKeyDown(KeyCode.X)) {
            GetId(Random.Range(2001, 2023));
        }
    }

    //添加id物品，添加到背包里，处理购买功能
    public void GetId(int id,int count =1) {

        //第一步查找在所有的物品中是否存在该物品
        InventoryItemGrid grid = null;
        foreach (InventoryItemGrid temp in itemGridList) {
            if (temp.id == id) {
                grid = temp; break;
            }
        }
        if (grid != null) {//存在 
            grid.PlusNumber(count);//根据购买的个数来增加number
        } 
        else {
            foreach (InventoryItemGrid temp in itemGridList) {
                if (temp.id == 0) {//按顺序找到第一个空的格子
                    grid = temp; break;
                }
            }
            if (grid != null) {
                GameObject itemGo = NGUITools.AddChild(grid.gameObject, inventoryItem);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.GetComponent<UISprite>().depth = 4;
                grid.SetId(id,count);
            }
        }
    }

    //背包，数量是否足够
    public bool MinusId(int id, int count = 1) {
        InventoryItemGrid grid = null;
        foreach (InventoryItemGrid temp in itemGridList) {
            if (temp.id == id) {
                grid = temp; break;
            }
        }
        if (grid == null) {
            return false;
        } else {
            bool isSuccess = grid.MinusNumber(count);
            return isSuccess;
        }
    }

    private bool isShow = false;

    void Show() {
        isShow = true;
        tween.PlayForward();
    }

    void Hide() {
        isShow = false;
        tween.PlayReverse();
    }

    //通过按钮来控制背包的显示和隐藏
    public void TransformState() {
        if (isShow == false) {
            Show();
        } else {
            Hide();
        }
    }

    public void AddCoin(int count) {   
        coinCount += count;
        coinNumberLabel.text = coinCount.ToString();//更新金币的显示
    }

    //取款方法，消费，给购买武器药品的接口使用
    public bool GetCoin(int count) {
        if (coinCount >= count) {
            coinCount -= count;
            coinNumberLabel.text = coinCount.ToString();//更新金币显示
            return true;
        }
        return false;
    }
	
}
