using UnityEngine;
using System.Collections;

//start场景的两个按钮
public class ButtonContainer : MonoBehaviour {

    //游戏数据的保存，和场景之间游戏数据的传递用 PlayerPrefs;有三个场景选择人物，start场景，play场景

    //开始新游戏
    public void OnNewGame() {
        PlayerPrefs.SetInt("DataFromSave", 0); 

        // 加载场景2，根据builled里存放的顺序
        Application.LoadLevel(1);
    }

    //加载已经保存的游戏，还没做
    public void OnLoadGame() {
        PlayerPrefs.SetInt("DataFromSave", 1); //DataFromSave表示数据来自保存

        //加载play3
    }
	
}
