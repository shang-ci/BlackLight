using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum PlayerState {
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}

//攻击时候的状态
public enum AttackState {
    Moving,
    Idle,
    Attack
}

public class PlayerAttack : MonoBehaviour {

    public static PlayerAttack _instance;

    public PlayerState state = PlayerState.ControlWalk;
    public AttackState attack_state = AttackState.Idle;

    public string aniname_normalattack;//普通攻击的动画
    public string aniname_idle;
    public string aniname_now;
    public float time_normalattack;//普通攻击的间隔就是攻击动画的时间
    public float rate_normalattack = 1;
    private float timer = 0;//普通攻击的计时器
    public float min_distance = 5;//攻击最小距离
    private Transform target_normalattack;//敌人位置

    private PlayerMove move;
    public GameObject effect;
    private bool showEffect = false;//是否展示特效，即是否攻击完了
    private PlayerStatus ps;
    public float miss_rate = 0.25f;//闪避率
    public GameObject hudtextPrefab;
    private GameObject hudtextFollow;
    private GameObject hudtextGo;
    private HUDText hudtext;
    public AudioClip miss_sound;
    public GameObject body;
    private Color normal;
    public string aniname_death;

    public GameObject[] efxArray;//存特效
    private Dictionary<string, GameObject> efxDict = new Dictionary<string, GameObject>();

    public bool isLockingTarget = false;//是否正在选择目标，释放指定位置技能时用
    private SkillInfo info = null;

    void Awake() {
        _instance = this;
        move = this.GetComponent<PlayerMove>();
        ps = this.GetComponent<PlayerStatus>();
        normal = body.GetComponent<Renderer>().material.color;

        hudtextFollow = transform.Find("HUDText").gameObject;

        foreach (GameObject go in efxArray) {
            efxDict.Add(go.name, go);
        }
    }

    void Start() {

        hudtextGo = NGUITools.AddChild(HUDTextParent._instance.gameObject, hudtextPrefab);

        hudtext = hudtextGo.GetComponent<HUDText>();
        UIFollowTarget followTarget = hudtextGo.GetComponent<UIFollowTarget>();
        followTarget.target = hudtextFollow.transform;
        followTarget.gameCamera = Camera.main;

    }

    void Update() {


        //这一部分是应为鼠标点击地面不起作用，用键盘来改变状态达到移动效果
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 
        Vector3 direction = transform.forward * vertical + transform.right * horizontal;

        // 如果有键盘输入，进入 ControlWalk 状态
        if (direction.magnitude >= 0.1f)
            state = PlayerState.ControlWalk; // 切换到控制移动状态


        if ( isLockingTarget==false&& Input.GetMouseButtonDown(0) &&  state != PlayerState.Death ) {
            //做射线检测
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);

            if (isCollider && hitInfo.collider.tag == Tags.enemy) {
                //当点击一个敌人时
                target_normalattack = hitInfo.collider.transform;
                state = PlayerState.NormalAttack;
                timer = 0;
                showEffect = false;
            } else {//当点击别的东西时，退出攻击状态
                state = PlayerState.ControlWalk;
                target_normalattack = null;
            }
        }

        if (state == PlayerState.NormalAttack) {
            if (target_normalattack == null) {
                state = PlayerState.ControlWalk;
            } else {//确保目标不为空
                float distance = Vector3.Distance(transform.position, target_normalattack.position);
                if (distance <= min_distance) {//进行攻击
                    transform.LookAt(target_normalattack.position);//一直朝向小怪
                    attack_state = AttackState.Attack;

                    timer += Time.deltaTime;
                    GetComponent<Animation>().CrossFade(aniname_now);//播放对应动画

                    //在NormalAttack角色会持续攻击
                    if (timer >= time_normalattack) {//进行了一次攻击，在里面要切换成静止状态，释放攻击特效，造成伤害
                        aniname_now = aniname_idle;
                        if (showEffect == false) {
                            showEffect = true;
                            //播放攻击特效
                            GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity);
                            target_normalattack.GetComponent<WolfBaby>().TakeDamage(GetAttack());
                        }

                    }
                    if (timer >= (1f / rate_normalattack)) {
                        timer = 0;
                        showEffect = false;
                        aniname_now = aniname_normalattack;
                    }

                } else {//走向敌人
                    attack_state = AttackState.Moving;
                    move.SimpleMove(target_normalattack.position);
                }
            }
        } else if (state == PlayerState.Death) {
            GetComponent<Animation>().CrossFade(aniname_death);//播放死亡动画
        }

        if (isLockingTarget && Input.GetMouseButtonDown(0)) {
            OnLockTarget();
        }
    }


    public int GetAttack() {
        return (int)(ps.attack + ps.attack_plus);
    }


    public void TakeDamage(int attack) {
        if (state == PlayerState.Death) return;

        //计算伤害
        float def = ps.def + ps.def_plus;
        float temp = attack * ((200 - def) / 200);

        if (temp < 1) temp = 1;

        float value = Random.Range(0f, 1f);

        //MISS
        if (value < miss_rate) {
            AudioSource.PlayClipAtPoint(miss_sound, transform.position);
            hudtext.Add("MISS", Color.gray, 1);
        } else {
            hudtext.Add("-" + temp, Color.red, 1);
            ps.hp_remain -= (int)temp;
            StartCoroutine(ShowBodyRed());
            if (ps.hp_remain <= 0) {
                state = PlayerState.Death;
            }
        }
        HeadStatusUI._instance.UpdateShow();
    }

    //受伤效果
    IEnumerator ShowBodyRed() {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<Renderer>().material.color = normal;
    }

    void OnDestroy() {
        GameObject.Destroy(hudtextGo);

    }

    //这里把技能放在了Attack里，不是很好
    public void UseSkill(SkillInfo info) {
        //排除错误的技能
        if (ps.heroType == HeroType.Magician) {
            if (info.applicableRole == ApplicableRole.Swordman) {
                return;
            }
        }
        if (ps.heroType == HeroType.Swordman) {
            if (info.applicableRole == ApplicableRole.Magician) {
                return;
            }
        }

        switch (info.applyType) {
            case ApplyType.Passive:
                StartCoroutine( OnPassiveSkillUse(info));
                break;
            case ApplyType.Buff:
                StartCoroutine(OnBuffSkillUse(info));
                break;
            case ApplyType.SingleTarget:
                OnSingleTargetSkillUse(info) ;
                break;
            case ApplyType.MultiTarget:
                OnMultiTargetSkillUse(info);
                break;
        }

    }

    //处理增益技能
    IEnumerator OnPassiveSkillUse(SkillInfo info ) {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;
        int hp = 0, mp = 0;
        if (info.applyProperty == ApplyProperty.HP) {
            hp = info.applyValue;
        } else if (info.applyProperty == ApplyProperty.MP) {
            mp = info.applyValue;
        }

        ps.GetDrug(hp,mp);
        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
    }

    //处理增强技能
    IEnumerator OnBuffSkillUse(SkillInfo info) {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;

        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        GameObject.Instantiate(prefab, transform.position, Quaternion.identity);

        switch (info.applyProperty) {
            case ApplyProperty.Attack:
                ps.attack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normalattack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed *= (info.applyValue / 100f);
                break;
        }
        yield return new WaitForSeconds(info.applyTime);
        switch (info.applyProperty) {
            case ApplyProperty.Attack:
                ps.attack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normalattack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed /= (info.applyValue / 100f);
                break;
        }
    }

    //选择目标敌人
    void OnSingleTargetSkillUse(SkillInfo info) {
        state = PlayerState.SkillAttack;
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }

    //选择好目标，开始技能的释放
    void OnLockTarget() {
        isLockingTarget = false;
        switch (info.applyType) {
            case ApplyType.SingleTarget:
                StartCoroutine( OnLockSingleTarget());
                break;
            case ApplyType.MultiTarget:
                StartCoroutine(OnLockMultiTarget());
                break;
        }
    }

    //单个敌人
    IEnumerator OnLockSingleTarget() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo);
        if (isCollider && hitInfo.collider.tag == Tags.enemy) {//选择了一个敌人
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;
            //实例化特效
            GameObject prefab = null;
            efxDict.TryGetValue(info.efx_name, out prefab);
            GameObject.Instantiate(prefab, hitInfo.collider.transform.position, Quaternion.identity);

            hitInfo.collider.GetComponent<WolfBaby>().TakeDamage((int)(GetAttack() * (info.applyValue / 100f)));
        } else {
            state = PlayerState.NormalAttack;
        }
        CursorManager._instance.SetNormal();
    }

    //范围攻击（多目标）
    void OnMultiTargetSkillUse(SkillInfo info) {
        state = PlayerState.SkillAttack;
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }

    IEnumerator OnLockMultiTarget() {
        CursorManager._instance.SetNormal();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo,11);
        if (isCollider) {
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;

            //实例化特效
            GameObject prefab = null;
            efxDict.TryGetValue(info.efx_name, out prefab);
            GameObject go = GameObject.Instantiate(prefab, hitInfo.point + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            go.GetComponent<MagicSphere>().attack = GetAttack() * (info.applyValue / 100f);
        } else {
            state = PlayerState.ControlWalk;
        }
    }
}
