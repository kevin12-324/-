using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 레벨업 시 스킬 선택 UI를 관리합니다
/// </summary>
public class SkillSelectUI : MonoBehaviour
{
    [Header("UI 요소")]
    public GameObject skillSelectionPanel; // 스킬 선택 패널
    public Button[] skillButtons = new Button[3]; // 3개의 스킬 버튼
    public TextMeshProUGUI[] skillNameTexts = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] skillDescTexts = new TextMeshProUGUI[3];

    private ExperienceSystem experienceSystem;
    private List<PassiveSkill> currentSkills;

    private void Start()
    {
        if (skillSelectionPanel != null)
        {
            skillSelectionPanel.SetActive(false);
        }

        // 버튼 이벤트 연결
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int index = i;
            if (skillButtons[i] != null)
            {
                skillButtons[i].onClick.AddListener(() => SelectSkill(index));
            }
        }
    }

    /// <summary>
    /// 스킬 선택 UI를 표시합니다
    /// </summary>
    public void ShowSkillSelection(ExperienceSystem expSystem)
    {
        experienceSystem = expSystem;
        currentSkills = expSystem.GetRandomSkills();

        // UI 업데이트
        for (int i = 0; i < 3; i++)
        {
            if (i < currentSkills.Count)
            {
                skillNameTexts[i].text = currentSkills[i].skillName;
                skillDescTexts[i].text = currentSkills[i].skillDescription;
                skillButtons[i].interactable = true;
            }
            else
            {
                skillButtons[i].interactable = false;
            }
        }

        // 패널 표시
        if (skillSelectionPanel != null)
        {
            skillSelectionPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 스킬을 선택합니다
    /// </summary>
    private void SelectSkill(int skillIndex)
    {
        if (skillIndex >= 0 && skillIndex < currentSkills.Count)
        {
            experienceSystem.ApplySkill(currentSkills[skillIndex]);
        }

        // UI 숨기기
        if (skillSelectionPanel != null)
        {
            skillSelectionPanel.SetActive(false);
        }
    }
}
