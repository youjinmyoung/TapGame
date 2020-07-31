using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : UnitySingleton<SkillUI>
{
    public Button[] skillButton = new Button[5];
    public bool[] isAvailableSkill = new bool[5];
    public Image[] skillImage = new Image[5];

    private Transform _skills;

    private void Start()
    {
        _skills = GameObject.Find("Skills").GetComponent<Transform>();

        for (int i = 0; i < 5; ++i)
        {
            skillButton[i] = _skills.GetChild(i).GetComponent<Button>();
            skillButton[i].transform.GetChild(0).GetComponent<Text>().enabled = false;
            skillImage[i] = GameObject.Find("Skill" + i.ToString()).GetComponent<Image>();
            if (isAvailableSkill[i] == false)
            {
                skillButton[i].GetComponent<Button>().interactable = false;
                skillImage[i].sprite = Resources.Load<Sprite>("UI/skill");
            }
        }

        if(GameInstance.Instance.coolDownTime > 0)
        {
            StartCoroutine(SkillCoolDownTime(4, GameInstance.Instance.coolDownTime));
        }
    }

    public IEnumerator SkillCoolDownTime(int skillIndex, float coolTime)
    {
        Button skillButton = _skills.GetChild(skillIndex).GetComponent<Button>();
        skillButton.interactable = false;

        Text skillCoolTimeText = skillButton.GetComponentInChildren<Text>();
        skillCoolTimeText.text = coolTime.ToString();
        skillCoolTimeText.enabled = true;

        float coolDownTime = coolTime;

        while (coolDownTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            coolDownTime -= 1.0f;
            GameInstance.Instance.coolDownTime = coolDownTime;
            skillCoolTimeText.text = coolDownTime.ToString();
        }

        skillButton.interactable = true;
        skillCoolTimeText.enabled = false;
    }

    public IEnumerator AddCriticalPercent(int activeTime)
    {
        Text activeTimeText = GameObject.Find("ActiveTime").GetComponent<Text>();
        activeTimeText.text = activeTime.ToString();
        activeTimeText.enabled = true;
        PlayerController.Instance.isCritical = true;
        while (activeTime > 0)
        {
            activeTimeText.text = activeTime.ToString();
            yield return new WaitForSeconds(1.0f);
            --activeTime;
        }
        PlayerController.Instance.isCritical = false;
        activeTimeText.enabled = false;
    }

    public IEnumerator AddHiredSolderSpeedUp(int activeTime)
    {
        Text activeTimeText = GameObject.Find("ActiveSpeedTime").GetComponent<Text>();
        activeTimeText.text = activeTime.ToString();
        activeTimeText.enabled = true;

        for (int i = 0; i < 4; ++i)
        {
            GameController.Instance.CurrentHiredSoldiers[i].attackSpeed -= (GameController.Instance.CurrentHiredSoldiers[i].attackSpeed * UpGradePopUp.Instance.hiredSoldierAttackSpeedIncrease * 0.5f);
            GameController.Instance.CurrentHiredSoldiers[i].attackSpeed -= PassivePopUp.Instance.passiveValue[17];
        }

        while (activeTime > 0)
        {
            activeTimeText.text = activeTime.ToString();
            yield return new WaitForSeconds(1.0f);
            --activeTime;
        }

        for (int i = 0; i < 4; ++i)
        {
            GameController.Instance.CurrentHiredSoldiers[i].attackSpeed = 3.0f;
        }

        activeTimeText.enabled = false;
    }

    public IEnumerator AddGold(int activeTime)
    {
        Text activeTimeText = GameObject.Find("ActiveGoldTime").GetComponent<Text>();
        activeTimeText.text = activeTime.ToString();
        activeTimeText.enabled = true;
        PlayerController.Instance.isAddGold = true;
        while (activeTime > 0)
        {
            activeTimeText.text = activeTime.ToString();
            yield return new WaitForSeconds(1.0f);
            --activeTime;
        }
        PlayerController.Instance.isAddGold = false;
        activeTimeText.enabled = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        StopAllCoroutines();
    }
}
