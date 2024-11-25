using UnityEngine;
using System.Collections;

//发放任务的npc
public class BarNPC : NPC {

    public static BarNPC _instance;
    public TweenPosition questTween;
    public UILabel desLabel;
    public GameObject acceptBtnGo;
    public GameObject okBtnGo;
    public GameObject cancelBtnGo;

    public bool isInTask = false;//是否在任务中
    public int killCount = 0;//杀死小野狼的数目

    private PlayerStatus status;

    void Awake() {
        _instance = this;
    }
    void Start() {
        status = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }

    //鼠标覆盖自动调用
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<AudioSource>().Play();
            if (isInTask) {
                ShowTaskProgress();
            } else {
                ShowTaskDes();
            }
            ShowQuest();
        }
    }

    //显示对话
    void ShowQuest() {
        questTween.gameObject.SetActive(true);
        questTween.PlayForward();
    }

    //隐藏对话
    void HideQuest() {
        questTween.PlayReverse();
    }

    public void OnKillWolf() {
        if (isInTask) {
            killCount++;
        }
    }

    //任务描述
    void ShowTaskDes(){
        desLabel.text = "任务：\n杀死了10只狼\n\n奖励：\n1000金币";
        okBtnGo.SetActive(false);
        acceptBtnGo.SetActive(true);
        cancelBtnGo.SetActive(true);
    }

    //任务进度
    void ShowTaskProgress(){
        desLabel.text = "任务：\n你已经杀死了" + killCount + "\\10只狼\n\n奖励：\n1000金币";
        okBtnGo.SetActive(true);
        acceptBtnGo.SetActive(false);
        cancelBtnGo.SetActive(false);
    }

    //点击X号
    public void OnCloseButtonClick() {
        HideQuest();
    }

    //接受按钮
    public void OnAcceptButtonClick() {
        ShowTaskProgress();
        isInTask = true;
    }

    //提交任务，完成任务会给经验值
    public void OnOkButtonClick() {
        if(killCount>=10){//完成任务
            Inventory._instance.AddCoin(1000);
            status.GetExp(killCount * 12);
            killCount = 0;
            ShowTaskDes();
        }else{
            //没有完成任务
            HideQuest();
        }
    }

    public void OnCancelButtonClick() {
        HideQuest();
    }
}
