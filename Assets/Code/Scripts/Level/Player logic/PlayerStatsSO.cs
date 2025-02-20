using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Bayat.SaveSystem;

[CreateAssetMenu(menuName = "PlayerStatsSO", fileName = "New Player Stats SO")]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField] private string saveSystemIdentifier = "playerStats.json";
    [SerializeField] private PlayerStatsSO initialPlayerStats;
    
    [Title("Player Base Stats")]
    public double MaxHealth = 100f;
    public double CurrentHealth = 100f;
    public bool PlayerIsAlive()
    {
        return CurrentHealth > 0;
    }
    public double Attack = 10f;
    public double MinAttack = 3f;
    public double MaxAttack = 7f;

    public double Defense = 0f;
    public float CriticalAttackChance = 20f;
    public float DodgeChance = 0f;
    public int ComboPower = 1;
    public int comboCount = 0;
    
    [Title("Player Bricks")]
    [SerializeField] List<BrickProbability> brickProbabilityList;
    public List<BrickProbability> BrickProbabilityList => brickProbabilityList;
    
    [Title("Spawn Time Player Bricks")]
    [SerializeField] private float minTimeToSpawnPlayerBrick = 0.5f;
    public float MinTimeToSpawnPlayerBrick => minTimeToSpawnPlayerBrick;
    [SerializeField] private float maxTimeToSpawnPlayerBrick = 1.5f;
    public float MaxTimeToSpawnPlayerBrick => maxTimeToSpawnPlayerBrick;
    [SerializeField] private int maxSimultaneousPlayerBricks = 6;
    public int MaxSimultaneousPlayerBricks => maxSimultaneousPlayerBricks;

    [Title("Player constant Upgrade")]
    [Title("Health", titleAlignment: TitleAlignments. Centered, horizontalLine: false, bold: false)]
    public float maxHealthLevel = 0;
    [SerializeField] float maxHealthIncreasePerLevel = 10f;
    public float MaxHealthIncreasePerLevel => maxHealthIncreasePerLevel;

    [Title("Min Attack", titleAlignment: TitleAlignments. Centered, horizontalLine: false, bold: false)]
    public int minAttackLevel = 0;
    public float MinAttackLittleIncrement = 3f;
    [SerializeField] private float constToIncreaseMinAttackLittleIncrement = 1f;
    public float ConstToIncreaseMinAttackLittleIncrement => constToIncreaseMinAttackLittleIncrement;
    
    public float MinAttackBigIncrement = 6f;
    [SerializeField] private float constToIncreaseMinAttackBigIncrement = 4f;
    public float ConstToIncreaseMinAttackBigIncrement => constToIncreaseMinAttackBigIncrement;
    
    [Title("Max Attack", titleAlignment: TitleAlignments. Centered, horizontalLine: false, bold: false)]
    public int maxAttackLevel = 0;
    public float MaxAttackLittleIncrement = 8f;
    [SerializeField] private float constToIncreaseMaxAttackLittleIncrement = 2f;
    public float ConstToIncreaseMaxAttackLittleIncrement => constToIncreaseMaxAttackLittleIncrement;
    
    public float MaxAttackBigIncrement = 15f;
    [SerializeField] private float constToIncreaseMaxAttackBigIncrement = 5f;
    public float ConstToIncreaseMaxAttackBigIncrement => constToIncreaseMaxAttackBigIncrement;
    
    [Title("ComboPower", titleAlignment: TitleAlignments. Centered, horizontalLine: false, bold: false)]
    public int comboPowerLevel = 0;
    public int comboPowerLittleIncrement = 1;
    [SerializeField] private int constToIncreaseComboPowerLittleIncrement = 1;
    public int ConstToIncreaseComboPowerLittleIncrement => constToIncreaseComboPowerLittleIncrement;
    public int comboPowerBigIncrement = 4;
    [SerializeField] private int constToIncreaseComboPowerBigIncrement = 4;
    public int ConstToIncreaseComboPowerBigIncrement => constToIncreaseComboPowerBigIncrement;
    


    [Title("Player Percent Stats Increments")]
    [SerializeField] double healPercentage = 25f;
    public double HealPercentage { get { return healPercentage; } }

    [SerializeField] float littleAttackIncreasePercentage = 15f;
    public float LittleAttackIncreasePercentage { get {  return littleAttackIncreasePercentage; } }

    [SerializeField] float mediumAttackIncreasePercentage = 30f;
    public float MediumAttackIncreasePercentage { get { return mediumAttackIncreasePercentage; } }

    [SerializeField] float mediumDefenseIncreasePercentage = 10f;
    public float MediumDefenseIncreasePercentage { get { return mediumDefenseIncreasePercentage; } }

    [SerializeField] float maxHealthIncreasePercentage = 10f;
    public float MaxHealthIncreasePercentage { get { return maxHealthIncreasePercentage; } }

    [SerializeField] float criticalChanceIncrease = 5f;
    public float CriticalChanceIncrease { get {  return criticalChanceIncrease; } }

    [Title("Special Effects Upgrades")]
    [SerializeField] float adrenalineDogdeChance = 30f;
    public float AdrenalineDodgeChance { get { return adrenalineDogdeChance; } }

    [SerializeField] float revengePercentageIncrease = 100f;
    public float RevengePercentageIncrease {  get { return revengePercentageIncrease; } }

    [SerializeField] float healthPercentageToActivateRage = 30f;
    public float HealthPercentageToActivateRage { get { return healthPercentageToActivateRage; } }

    [SerializeField] float extraRageAttack = 20f;
    public float ExtraRageAttack { get { return extraRageAttack; } }

    [SerializeField] float spinesPercentageDamage = 25f;
    public float SpinesPercentageDamage { get {  return spinesPercentageDamage; } }


    [Title("Heavy Attack Upgrade Stats")]
    public int HeavyAttackLevel = 0;
    public float HeavyAttackPercentageDamage = 250f;


    [Title("Fire Upgrade Stats")]
    [SerializeField] int fireLevel = 0;

    [SerializeField] double firePercentageDamage = 0f;
    public double FirePercentageDamage { get { return firePercentageDamage; } }

    [SerializeField] float firePercentageDamageIncrement = 10f;
    public float FirePercentageDamageIncrement { get { return firePercentageDamageIncrement; } }

    [SerializeField] float fireChanceIncrement = 10f;

    [SerializeField] float timeToDoDamageFire = 2f;
    public float TimeToDoDamageFire { get { return timeToDoDamageFire; } }

    [SerializeField] float timeToTurnOffFire = 10f;
    public float TimeToTurnOffFire { get { return timeToTurnOffFire; } }

    public BrickTypeEnum GetRandomPlayerBrick()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        float rangeNumberToSpawn = 0f;
        foreach (BrickProbability brickProbability in brickProbabilityList)
        {
            if (rangeNumberToSpawn < randomNumber && (rangeNumberToSpawn + brickProbability.Probability) > randomNumber)
            {
                return brickProbability.BrickType;
            }
            rangeNumberToSpawn += brickProbability.Probability;
        }

        Debug.LogError("LevelSo: No random Enemy brick selected");
        return BrickTypeEnum.YellowBrick;
    }

    public void CopyPlayerStatsFrom(PlayerStatsSO statsToCopy)
    {
        this.MaxHealth = statsToCopy.MaxHealth;
        this.CurrentHealth = statsToCopy.CurrentHealth;
        this.Attack = statsToCopy.Attack;
        this.MinAttack = statsToCopy.MinAttack;
        this.MaxAttack = statsToCopy.MaxAttack;
        this.ComboPower = statsToCopy.ComboPower;
        this.comboCount = statsToCopy.comboCount;

        this.minTimeToSpawnPlayerBrick = statsToCopy.minTimeToSpawnPlayerBrick;
        this.maxTimeToSpawnPlayerBrick = statsToCopy.maxTimeToSpawnPlayerBrick;
        this.maxSimultaneousPlayerBricks = statsToCopy.maxSimultaneousPlayerBricks;

        this.maxHealthLevel = statsToCopy.maxHealthLevel;
        this.minAttackLevel = statsToCopy.minAttackLevel;
        this.MinAttackLittleIncrement = statsToCopy.MinAttackLittleIncrement;
        this.constToIncreaseMinAttackLittleIncrement = statsToCopy.ConstToIncreaseMinAttackLittleIncrement;
        this.MinAttackBigIncrement = statsToCopy.MinAttackBigIncrement;
        this.constToIncreaseMinAttackBigIncrement = statsToCopy.ConstToIncreaseMinAttackBigIncrement;
        this.maxAttackLevel = statsToCopy.maxAttackLevel;
        this.MaxAttackLittleIncrement = statsToCopy.MaxAttackLittleIncrement;
        this.constToIncreaseMaxAttackLittleIncrement = statsToCopy.ConstToIncreaseMaxAttackLittleIncrement;
        this.MaxAttackBigIncrement = statsToCopy.MaxAttackBigIncrement;
        this.constToIncreaseMaxAttackBigIncrement = statsToCopy.ConstToIncreaseMaxAttackBigIncrement;

        this.Defense = statsToCopy.Defense;
        this.CriticalAttackChance = statsToCopy.CriticalAttackChance;
        this.DodgeChance = statsToCopy.DodgeChance;

        this.healPercentage = statsToCopy.HealPercentage;
        this.littleAttackIncreasePercentage = statsToCopy.littleAttackIncreasePercentage;
        this.mediumAttackIncreasePercentage = statsToCopy.mediumAttackIncreasePercentage;
        this.maxHealthIncreasePerLevel = statsToCopy.maxHealthIncreasePerLevel;
        this.criticalChanceIncrease = statsToCopy.criticalChanceIncrease;
        this.adrenalineDogdeChance = statsToCopy.adrenalineDogdeChance;
        this.revengePercentageIncrease = statsToCopy.revengePercentageIncrease;
        this.healthPercentageToActivateRage = statsToCopy.healthPercentageToActivateRage;
        this.extraRageAttack = statsToCopy.extraRageAttack;
        this.spinesPercentageDamage = statsToCopy.spinesPercentageDamage;

        this.fireLevel = statsToCopy.fireLevel;
        this.firePercentageDamage = statsToCopy.firePercentageDamage;
        this.firePercentageDamageIncrement = statsToCopy.firePercentageDamageIncrement;
        this.fireChanceIncrement = statsToCopy.fireChanceIncrement;
        this.timeToDoDamageFire = statsToCopy.timeToDoDamageFire;
        this.timeToTurnOffFire = statsToCopy.TimeToTurnOffFire;

        this.brickProbabilityList = Clone(statsToCopy.brickProbabilityList);
        
    }

    private List<BrickProbability> Clone(List<BrickProbability> listToClone)
    {
        List<BrickProbability> probabilityList = new List<BrickProbability>();

        foreach(BrickProbability brickProbabilityToClone in listToClone)
        {
            BrickProbability newBrickProbability = new BrickProbability(brickProbabilityToClone.BrickType)
            {
                Probability = brickProbabilityToClone.Probability
            };
            probabilityList.Add(newBrickProbability);
        }

        return probabilityList;
    }

    public void LevelUpFireAttack()
    {
        fireLevel += 1;
        firePercentageDamage += firePercentageDamageIncrement;
        
        foreach(BrickProbability brickProbability in brickProbabilityList)
        {
            if(brickProbability.BrickType == BrickTypeEnum.FireBrick)
            {
                brickProbability.Probability += fireChanceIncrement;
                return;
            }
        }
    }

    public async void LoadPlayerStats()
    {
        if (await SaveSystemAPI.ExistsAsync(saveSystemIdentifier) == false)
        {
            CopyPlayerStatsFrom(initialPlayerStats);
            SavePlayerStats();
        }
        else
        {
            PlayerStatsSO loadedStatsData = await SaveSystemAPI.LoadAsync<PlayerStatsSO>(saveSystemIdentifier);
            CopyPlayerStatsFrom(loadedStatsData);
        }
    }

    public async void SavePlayerStats()
    {
        await SaveSystemAPI.SaveAsync(saveSystemIdentifier, this);
    }
}
