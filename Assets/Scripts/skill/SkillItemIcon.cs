using UnityEngine;
using System.Collections;


public class SkillItemIcon : UIDragDropItem {

    private int skillId;

    //在克隆的icon上调用的
    protected override void OnDragDropStart() {
        base.OnDragDropStart();

        skillId = transform.parent.GetComponent<SkillItem>().id;
        transform.parent = transform.root;
        this.GetComponent<UISprite>().depth = 40;//显示层
    }

    //从skill表里拖到快捷栏
    protected override void OnDragDropRelease(GameObject surface) {
        base.OnDragDropRelease(surface);
        if (surface != null && surface.tag == Tags.shortcut) {
            surface.GetComponent<ShortCutGrid>().SetSkill(skillId);
        }
    }
}
