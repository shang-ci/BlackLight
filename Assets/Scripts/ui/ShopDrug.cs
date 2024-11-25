using UnityEngine;
using System.Collections;

public class ShopDrug : MonoBehaviour {


    public static ShopDrug _instance;
    private TweenPosition tween;
    private bool isShow = false;

    private GameObject numberDialog;//数量输入框，当点击buy按钮会弹出
    private UIInput numberInput;
    private int buy_id = 0;//标识

    void Awake() {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        numberDialog = this.transform.Find("NumberDialog").gameObject;
        numberInput = transform.Find("NumberDialog/NumberInput").GetComponent<UIInput>();
        numberDialog.SetActive(false);
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

    public void OnCloseButtonClick() {
        TransformState();
    }


    public void OnBuyId1001() {
        Buy(1001);
    }

    public void OnBuyId1002() {
        Buy(1002);
    }

    public void OnBuyId1003() {
        Buy(1003);
    }

    void Buy(int id) {
        ShowNumberDialog();
        buy_id = id;
    }

    //处理购买的逻辑
    public void OnOKButtonClick() {
        int count = int.Parse(numberInput.value );
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(buy_id);
        int price = info.price_buy;
        int price_total = price * count;

        bool success = Inventory._instance.GetCoin(price_total);//消费

        if (success) {
            if (count > 0) {//当数量多于一个才添加到背包
                Inventory._instance.GetId(buy_id, count);
            }
        }

        numberDialog.SetActive(false);
    }

    //控制输入数量框的显示
    void ShowNumberDialog() {
        numberDialog.SetActive(true);
        numberInput.value = "0";
    }

}
