using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TalentTreeUI : MonoBehaviour
{
    public GameObject talentPanel;
    public Transform talentContainer;
    public GameObject talentNodePrefab;
    public TalentManager talentManager;

    private Dictionary<TalentData, GameObject> nodeMap = new Dictionary<TalentData, GameObject>();
    private bool isOpen = false;

    void Start()
    {
        talentPanel.SetActive(false);
        BuildTree();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isOpen = !isOpen;
            talentPanel.SetActive(isOpen);
            if (isOpen) RefreshUI();
        }
    }

    void BuildTree()
    {
        foreach (var talent in talentManager.allTalents)
        {
            GameObject node = Instantiate(talentNodePrefab, talentContainer);

            node.GetComponentInChildren<Text>().text = talent.displayName;
            node.transform.Find("Icon").GetComponent<Image>().sprite = talent.icon;

            Button btn = node.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                talentManager.LearnTalent(talent);
                RefreshUI();
            });

            nodeMap[talent] = node;
        }
    }

    void RefreshUI()
    {
        foreach (var kvp in nodeMap)
        {
            TalentData talent = kvp.Key;
            GameObject node = kvp.Value;

            bool learned = talentManager.IsLearned(talent);
            bool canLearn = talentManager.CanLearn(talent);

            Button btn = node.GetComponent<Button>();
            btn.interactable = !learned && canLearn;

            Image bg = node.GetComponent<Image>();
            if (learned)
                bg.color = Color.green;
            else if (!canLearn)
                bg.color = Color.gray;
            else
                bg.color = Color.white;
        }
    }
}