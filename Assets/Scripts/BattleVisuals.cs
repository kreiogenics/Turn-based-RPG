using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private int currHealth;
    private int maxHealth;
    private int level;

    private Animator anim;

    private const string LEVEL_ABBREVIATION = "Lvl: ";

//|-------------------------------------------|
//| Constant strings for animation triggers   |
//|-------------------------------------------|
    private const string IS_MELE_ATTACK_PARAM = "IsMeleeAttack";
    private const string IS_RANGED_ATTACK_PARAM = "IsRangedAttack";
    private const string IS_DAMAGED_PARAM = "IsDamaged";
    private const string IS_DEAD_PARAM = "IsDead";
    private const string IS_VICTORIOUS_PARAM = "IsVictorious";

    // Start is called before the first frame update
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void SetStartingValues(int currHealth, int maxHealth, int level)
    {
        this.currHealth = currHealth;
        this.maxHealth = maxHealth;
        this.level = level;
        levelText.text = LEVEL_ABBREVIATION + this.level.ToString();
        UpdateHealthBar();
    }

    public void ChangeHealth(int currHealth) 
    {
        this.currHealth = currHealth;

        if (currHealth <= 0)
        {
            PlayDeathAnimation();
            Destroy(gameObject, 1f);
        }
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;
    }

//|-------------------------------------------|
//| Animation triggers                        |
//|-------------------------------------------|

    public void PlayMeleeAttackAnimation() => anim.SetTrigger(IS_MELE_ATTACK_PARAM);

    public void PlayRangedAttackAnimation() => anim.SetTrigger(IS_RANGED_ATTACK_PARAM);
    
    public void PlayDamagedAnimation() => anim.SetTrigger(IS_DAMAGED_PARAM);

    public void PlayDeathAnimation() => anim.SetTrigger(IS_DEAD_PARAM);
    
    public void PlayVictoriousAnimation() => anim.SetTrigger(IS_VICTORIOUS_PARAM);

}
