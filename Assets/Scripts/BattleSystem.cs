using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{

    [SerializeField] private enum BattleState { Start, Selection, Battle, Won, Lost, Run }

    [Header("Battle States")]
    [SerializeField] private BattleState state;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject battleMessages;
    [SerializeField] private TextMeshProUGUI battleStatusText;
    [SerializeField] private GameObject partyMemberList;

    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;

    private const string OVERWORLD_SCENE = "OverworldScene";
    private const string WIN_MESSAGE = "Enemies defeated!";
    private const string LOSS_MESSAGE = "Party defeated!";
    private const string SUCCESSFULLY_RAN_MESSAGE = "Party ran away!";
    private const string UNSUCCESSFULLY_RAN_MESSAGE = "Party could not run away!";
    private const int TURN_DURATION = 2;
    private const int RUN_CHANCE = 50;

    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
        DetermineBattleOrder();

    }

    private IEnumerator BattleRoutine()
    {
        enemySelectionMenu.SetActive(false);
        state = BattleState.Battle;
        battleMessages.SetActive(true);

        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (state == BattleState.Battle && allBattlers[i].CurrentHealth > 0)
            {
                //Debug.Break();
                switch (allBattlers[i].BattleAction)
                {
                    case BattleEntities.Action.MeleeAttack:
                        yield return StartCoroutine(MeleeAttackRoutine(i));
                        break;
                    case BattleEntities.Action.RangedAttack:
                        yield return StartCoroutine(RangedAttackRoutine(i));
                        break;
                    case BattleEntities.Action.Run:
                        yield return StartCoroutine(RunRoutine());
                        break;
                    default:
                        Debug.Log("Error - incorrect battle action");
                        break;
                }
            }
        }

       // RemoveDeadBattlers();

        if (state == BattleState.Battle)
        {
            battleMessages.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();
        }

        yield return null;
    }

    private IEnumerator MeleeAttackRoutine(int i)
    {
        if (allBattlers[i].IsPlayer == true)
        {
            BattleEntities currentAttacker = allBattlers[i];
            if (allBattlers[currentAttacker.Target].CurrentHealth <= 0)
            {
                currentAttacker.SetTarget(GetRandomEnemy());
            }
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];
            MeleeAttackAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION);
            // partyMemberList.SetActive(true);

            if (currentTarget.CurrentHealth <= 0)
            {
                yield return new WaitForSeconds(TURN_DURATION);
                //enemyBattlers.Remove(currentTarget);

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    currentAttacker.BattleVisuals.PlayVictoriousAnimation();
                    battleStatusText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    SceneManager.LoadScene(OVERWORLD_SCENE);
                }
            }

        }

        if (i < allBattlers.Count && allBattlers[i].IsPlayer == false)
        {
            BattleEntities currentAttacker = allBattlers[i];
            currentAttacker.SetTarget(GetRandomPartyMember());
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];

            MeleeAttackAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION);

            if (currentTarget.CurrentHealth <= 0)
            {
                yield return new WaitForSeconds(TURN_DURATION);
                //playerBattlers.Remove(currentTarget);

                if (playerBattlers.Count <= 0)
                {
                    state = BattleState.Lost;
                    battleStatusText.text = LOSS_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    Debug.Log("Game Over");
                }
            }
        }
    }
    private IEnumerator RangedAttackRoutine(int i)
    {
        if (allBattlers[i].IsPlayer == true)
        {
            BattleEntities currentAttacker = allBattlers[i];
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];
            RangedAttackAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION);
            // partyMemberList.SetActive(true);

            if (currentTarget.CurrentHealth <= 0)
            {
                enemyBattlers.Remove(currentTarget);

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    partyMemberList.SetActive(false);
                    battleStatusText.text = WIN_MESSAGE;
                }
            }
        }
    }

    private IEnumerator RunRoutine()
    {
        if (state == BattleState.Battle)
        {
            if (Random.Range(1, 101) >= RUN_CHANCE)
            {
                battleStatusText.text = SUCCESSFULLY_RAN_MESSAGE;
                state = BattleState.Run;
                allBattlers.Clear();
                yield return new WaitForSeconds(TURN_DURATION);
                SceneManager.LoadScene(OVERWORLD_SCENE);
                yield break;
            }
            else
            {
                battleStatusText.text = UNSUCCESSFULLY_RAN_MESSAGE;
                yield return new WaitForSeconds(TURN_DURATION);
            }
        }
    }

   /* private void RemoveDeadBattlers()
    {
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].CurrentHealth <= 0)
            {
                allBattlers.RemoveAt(i);
            }
        }
    }*/

    private void CreatePartyEntities()
    {
        List<PartyMember> currentParty = new List<PartyMember>();
        currentParty = partyManager.GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues(currentParty[i].MemberName, currentParty[i].CurrentHealth, currentParty[i].MaxHealth,
                currentParty[i].Strength, currentParty[i].Speed, currentParty[i].Level, true);

            BattleVisuals tempBattleVisuals = Instantiate(currentParty[i].MemberBattleVisualPrefab,
                partySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(currentParty[i].CurrentHealth, currentParty[i].MaxHealth, currentParty[i].Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }
    }

    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = new List<Enemy>();
        currentEnemies = enemyManager.GetCurrentEnemies();

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues(currentEnemies[i].EnemyName, currentEnemies[i].CurrentHealth, currentEnemies[i].MaxHealth,
                currentEnemies[i].Strength, currentEnemies[i].Speed, currentEnemies[i].Level, false);

            BattleVisuals tempBattleVisuals = Instantiate(currentEnemies[i].EnemyVisualPrefab,
                            enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(currentEnemies[i].MaxHealth, currentEnemies[i].MaxHealth, currentEnemies[i].Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }

    public void ShowBattleMenu()
    {
        actionText.text = playerBattlers[currentPlayer].Name;
        battleMenu.SetActive(true);
    }

    public void ShowEnemySelectionMenu()
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);
    }

    private void SetEnemySelectionButtons()
    {
        for (int i = 0; i < enemySelectionButtons.Length; i++)
        {
            enemySelectionButtons[i].SetActive(false);
        }

        for (int j = 0; j < enemyBattlers.Count; j++)
        {
            enemySelectionButtons[j].SetActive(true);
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name;
        }
    }

    public void SelectEnemy(int currentEnemy)
    {
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentEnemy]));

        currentPlayerEntity.BattleAction = BattleEntities.Action.MeleeAttack;
        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();
        }
    }

    private void MeleeAttackAction(BattleEntities currentAttacker, BattleEntities currentTarget)
    {
        partyMemberList.SetActive(false);
        int damage = currentAttacker.Strength;
        currentAttacker.BattleVisuals.PlayMeleeAttackAnimation();
        currentTarget.CurrentHealth -= damage;
        currentTarget.BattleVisuals.PlayDamagedAnimation();
        currentTarget.UpdateUI();
        battleStatusText.text = string.Format("{0} damages {1} for {2} damage.", currentAttacker.Name, currentTarget.Name, damage); // change to show damage above character
        SaveHealth();
    }

    private void RangedAttackAction(BattleEntities currentAttacker, BattleEntities currentTarget)
    {
        int damage = currentAttacker.Strength;
        currentAttacker.BattleVisuals.PlayRangedAttackAnimation();
        currentTarget.CurrentHealth -= damage;
        currentTarget.BattleVisuals.PlayDamagedAnimation();
        currentTarget.UpdateUI();
        battleStatusText.text = string.Format("{0} damages {1} for {2} damage.", currentAttacker.Name, currentTarget.Name, damage); // change to show damage above character
    }

    private int GetRandomPartyMember()
    {
        List<int> partyMembers = new List<int>();

        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer == true)
            {
                partyMembers.Add(i);
            }
        }

        return partyMembers[Random.Range(0, partyMembers.Count)];
    }

    private int GetRandomEnemy()
    {
        List<int> enemies = new List<int>();

        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer == false)
            {
                enemies.Add(i);
            }
        }

        return enemies[Random.Range(0, enemies.Count)];
    }

    private void SaveHealth()
    {
        for (int i = 0; i < playerBattlers.Count; i++)
        {
            partyManager.SaveHealth(i, playerBattlers[i].CurrentHealth);
        }
    }

    private void DetermineBattleOrder()
    {
        allBattlers.Sort((bi1, bi2) => -bi1.Speed.CompareTo(bi2.Speed)); //Sorts list by Speed in ascending order
    }

    public void SelectRunAction()
    {
        state = BattleState.Selection;
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];

        currentPlayerEntity.BattleAction = BattleEntities.Action.Run; // Tell battle system to run
        battleMenu.SetActive(false);
        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();
        }
    }
}

[System.Serializable]
public class BattleEntities
{
    public enum Action { MeleeAttack, RangedAttack, Run }
    public Action BattleAction;

    public string Name;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int Strength;
    public int Speed;
    public bool IsPlayer;
    public BattleVisuals BattleVisuals;
    public int Target;

    public void SetEntityValues(string name, int currHealth, int maxHealth, int strength, int speed, int level, bool isPlayer)
    {
        Name = name;
        CurrentHealth = currHealth;
        MaxHealth = maxHealth;
        Strength = strength;
        Speed = speed;
        Level = level;
        IsPlayer = isPlayer;
    }

    public void SetTarget(int target)
    {
        Target = target;
    }

    public void UpdateUI()
    {
        BattleVisuals.ChangeHealth(CurrentHealth);
    }

}
