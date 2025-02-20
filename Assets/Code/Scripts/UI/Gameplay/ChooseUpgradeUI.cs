using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ChooseUpgradeUI : UIComponent
{
    [Header("Upgrades Lists SO")]
    [SerializeField] private UpgradeInLevelSO upgradeListSO;
    [SerializeField] private UpgradeInLevelSO upgradesPlayerHasSO;

    [Header("Events")]
    [SerializeField] private GameEvent onContinueWalkingEvent;
    [SerializeField] private GameEvent onShowUpgradesToChooseEvent;
    [SerializeField] private GameEvent onUpgradeChosenEvent;

    private VisualElement holderToScale;
    private List<VisualElement> UpgradeContainerList = new List<VisualElement>();

    [SerializeField] private List<Upgrade> upgradesThatCanBeSelected = new List<Upgrade>();
    private List<Upgrade> upgradesRandomlySelected = new List<Upgrade>();

    private const string upgradeContainerClass = "upgradeContainer";
    private const string upgradeNameClass = "nameUpgrade";
    private const string iconImageName = "IconImage";
    private const string upgradeDescriptionClass = "descriptionUpgrade";
    private const string scaleHolderElement = "HolderToScale";

    public void ActivateChooseUpgradeUI()
    {
        SelectRandomUpgrades();
        StartCoroutine(ActivateChooseUpgradeUICoroutine());
    }

    private IEnumerator ActivateChooseUpgradeUICoroutine()
    {
        yield return new WaitForSeconds(2f);
        onShowUpgradesToChooseEvent?.Raise(new Empty(), this);
        SetDisplayElementFlex();
        ScaleUpUI();
    }

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();

        holderToScale = m_UIElement.Query<VisualElement>(name: scaleHolderElement);
        holderToScale.RegisterCallback<TransitionEndEvent>(OnChangeScaleEndEvent);
        ScaleDownElement(holderToScale);

        UpgradeContainerList = m_UIElement.Query<VisualElement>(className: upgradeContainerClass).ToList();

        upgradesThatCanBeSelected = new List<Upgrade>(upgradeListSO.UpgradeList);
    }

    public void ResetUpgradesCanChooseList()
    {
        upgradesThatCanBeSelected = new List<Upgrade>(upgradeListSO.UpgradeList);
    }

    public void UpgradeSelected()
    {
        onContinueWalkingEvent.Raise(new Empty(), this);
        ScaleDownUI();
        onUpgradeChosenEvent.Raise(new Empty(), this);
    }

    private void SelectRandomUpgrades()
    {
        upgradesRandomlySelected.Clear();

        int upgradesToDisplay = 3;
        if(upgradesThatCanBeSelected.Count < 3) { upgradesToDisplay = upgradesThatCanBeSelected.Count; }

        while (upgradesRandomlySelected.Count < upgradesToDisplay)
        {
            int i = Random.Range(0, upgradesThatCanBeSelected.Count);
            Upgrade upgrade = upgradesThatCanBeSelected[i];

            if (upgradesRandomlySelected.Count == 0)
            {
                upgradesRandomlySelected.Add(upgrade);
            }
            else
            {
                foreach (Upgrade upgradeToCheck in upgradesRandomlySelected)
                {
                    if (upgradeToCheck.UpgradeName == upgrade.UpgradeName)
                    {
                        goto WhileLoop;
                    }
                }

                upgradesRandomlySelected.Add(upgrade);
            }

            WhileLoop:
            continue;
        }

        SetUpgradesUI();
    }

    private void SetUpgradesUI()
    {
        if(upgradesRandomlySelected.Count == 0)
        {
            SelectRandomUpgrades();
        }

        Label upgradeName;
        VisualElement iconImage;
        Label upgradeDescription;

        for(int i = 0; i < upgradesRandomlySelected.Count; i++)
        {
            upgradeName = UpgradeContainerList[i].Query<Label>(className: upgradeNameClass);
            iconImage = UpgradeContainerList[i].Query<VisualElement>(name: iconImageName);
            upgradeDescription = UpgradeContainerList[i].Query<Label> (className: upgradeDescriptionClass);

            upgradeName.text = upgradesRandomlySelected[i].UpgradeName;
            iconImage.style.backgroundImage = new StyleBackground(upgradesRandomlySelected[i].Image);
            upgradeDescription.text = upgradesRandomlySelected[i].UpgradeDescription;
        }
    }

    private void RegisterAllEvents()
    {
        UpgradeContainerList[0].RegisterCallback<ClickEvent>(OnFirstUpgradeSelectes);
        UpgradeContainerList[1].RegisterCallback<ClickEvent>(OnSecondUpgradeSelected);
        UpgradeContainerList[2].RegisterCallback<ClickEvent>(OnThirdUpgradeSelected);
    }

    private void UnregisterAllEvents()
    {
        UpgradeContainerList[0].UnregisterCallback<ClickEvent>(OnFirstUpgradeSelectes);
        UpgradeContainerList[1].UnregisterCallback<ClickEvent>(OnSecondUpgradeSelected);
        UpgradeContainerList[2].UnregisterCallback<ClickEvent>(OnThirdUpgradeSelected);
    }

    private void OnFirstUpgradeSelectes(ClickEvent evt)
    {
        upgradesRandomlySelected[0].UpgradeEvent.Raise(new Empty(), this);
        upgradesPlayerHasSO.UpgradeList.Add(upgradesRandomlySelected[0]);
        UpgradeSelected();
        RemoveUpgradeSelectedFromList(upgradesRandomlySelected[0]);
        UnregisterAllEvents();
    }

    private void OnSecondUpgradeSelected(ClickEvent evt)
    {
        upgradesRandomlySelected[1].UpgradeEvent.Raise(new Empty(), this);
        upgradesPlayerHasSO.UpgradeList.Add(upgradesRandomlySelected[1]);
        UpgradeSelected();
        RemoveUpgradeSelectedFromList(upgradesRandomlySelected[1]);
        UnregisterAllEvents();
    }

    private void OnThirdUpgradeSelected(ClickEvent evt)
    {
        upgradesRandomlySelected[2].UpgradeEvent.Raise(new Empty(), this);
        upgradesPlayerHasSO.UpgradeList.Add(upgradesRandomlySelected[2]);
        UpgradeSelected();
        RemoveUpgradeSelectedFromList(upgradesRandomlySelected[2]);
        UnregisterAllEvents();
    }

    private void RemoveUpgradeSelectedFromList(Upgrade upgrade)
    {
        if(upgrade.MaxLevel > 0)
        {
            if(upgrade.MaxLevel <= upgradesPlayerHasSO.GetCurrentUpgradeLevel(upgrade)) 
            {
                foreach(Upgrade upgradeToCheck in upgradesThatCanBeSelected)
                {
                    if (upgradeToCheck.UpgradeId == upgrade.UpgradeId)
                    {
                        upgradesThatCanBeSelected.Remove(upgradeToCheck);
                        return;
                    }
                }
            }
        }
    }


    protected override void OnScaledUp()
    {
        base.OnScaledUp();
        RegisterAllEvents();
    }

    protected override void OnScaledDown()
    {
        base.OnScaledDown();
        SetDisplayElementNone();
    }
}
