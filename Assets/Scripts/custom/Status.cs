using UnityEngine;
using System.Collections;

//status面板
public class Status : MonoBehaviour {

    public static Status _instance;
    private TweenPosition tween;
    private bool isShow = false;

    private UILabel attackLabel;
    private UILabel defLabel;
    private UILabel speedLabel;
    private UILabel pointRemainLabel;
    private UILabel summaryLabel;

    //三个加号，在剩余点数还有的时候会显示
    private GameObject attackButtonGo;
    private GameObject defButtonGo;
    private GameObject speedButtonGo;

    private PlayerStatus ps;

    void Awake() {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();

        attackLabel = transform.Find("attack").GetComponent<UILabel>();
        defLabel = transform.Find("def").GetComponent<UILabel>();
        speedLabel = transform.Find("speed").GetComponent<UILabel>();
        pointRemainLabel = transform.Find("point_remain").GetComponent<UILabel>();
        summaryLabel = transform.Find("summary").GetComponent<UILabel>();
        attackButtonGo = transform.Find("attack_plusbutton").gameObject;
        defButtonGo = transform.Find("def_plusbutton").gameObject;
        speedButtonGo = transform.Find("speed_plusbutton").gameObject;

    }

    void Start() {
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }

    void Update() {
        UpdateShow();
    }

    public void TransformState() {
        if (isShow == false) {
            UpdateShow();
            tween.PlayForward();
            isShow = true;
        } else {
            tween.PlayReverse();
            isShow = false;
        }
    }

    // 更新显示
    void UpdateShow() {
        attackLabel.text = ps.attack + " + " + ps.attack_plus;
        defLabel.text = ps.def + " + " + ps.def_plus;
        speedLabel.text = ps.speed + " + " + ps.speed_plus;

        pointRemainLabel.text = ps.point_remain.ToString();

        summaryLabel.text = "伤害：" + (ps.attack + ps.attack_plus)
            + "  " + "防御：" + (ps.def + ps.def_plus)
            + "  " + "速度：" + (ps.speed + ps.speed_plus);

        if (ps.point_remain > 0) {
            attackButtonGo.SetActive(true);
            defButtonGo.SetActive(true);
            speedButtonGo.SetActive(true);
        } else {
            attackButtonGo.SetActive(false);
            defButtonGo.SetActive(false);
            speedButtonGo.SetActive(false);
        }

    }

    //三个加号逻辑
    public void OnAttackPlusClick() {
        bool success = ps.GetPoint();
        if (success) {
            EquipmentUI._instance.attack_plus++; 
            UpdateShow();
        }
    }

    public void OnDefPlusClick() {
        bool success = ps.GetPoint();
        if (success) {
            EquipmentUI._instance.def_plus++;
            UpdateShow();
        }
    }

    public void OnSpeedPlusClick() {
        bool success = ps.GetPoint();
        if (success) {
            EquipmentUI._instance.speed_plus++;
            UpdateShow();
        }
    }

}
