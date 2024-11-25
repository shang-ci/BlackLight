using UnityEngine;
using System.Collections;

public class CharacterCreation : MonoBehaviour {

    public GameObject[] characterPrefabs;
    public UIInput nameInput;//输入的文本
    private GameObject[] characterGameObjects;//存放实例化的角色

    private int selectedIndex = 0;
    private int length;//可供选择角色个数

	// Use this for initialization
	void Start () {
	    length = characterPrefabs.Length;
        characterGameObjects = new GameObject[length];
        for (int i = 0; i < length; i++) {
            characterGameObjects[i] = GameObject.Instantiate(characterPrefabs[i], transform.position, transform.rotation) as GameObject;
        }
        UpdateCharacterShow();
	}

    //更新角色的显示，选中谁，展示谁
    void UpdateCharacterShow() {
        characterGameObjects[selectedIndex].SetActive(true);
        for (int i = 0; i < length; i++) {
            if (i != selectedIndex) {
                characterGameObjects[i].SetActive(false);
            }
        }
    }

    //两个切换按钮
    public void OnNextButtonClick() {
        selectedIndex++;
        selectedIndex %= length;
        UpdateCharacterShow();
    }

    public void OnPrevButtonClick() {
        selectedIndex--;
        if (selectedIndex == -1) {
            selectedIndex = length - 1;
        }
        UpdateCharacterShow();
    }

    public void OnOkButtonClick() {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedIndex);//存储角色
        PlayerPrefs.SetString("name", nameInput.value);//存储名字

        //加载下一个场景
        Application.LoadLevel(2);
    }

}
