using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackTest : MonoBehaviour
{
    [Header("Attack Player")]
    public float attackRange;
    public int attackDamage;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    [Header("Life Player")]
    public float maxLife;
    private float currentLife;
    public Image lifeBar;

    private bool isAttack = false;

    private Animator anim;
    private PlayerMovimentTest playerMoviment;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMoviment = GetComponent<PlayerMovimentTest>();

        currentLife = maxLife;
        UpdateLifeBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            isAttack = true;
            anim.SetTrigger("attack");
            playerMoviment.canMove = false;
            PerformAttack();

            Invoke(nameof(EnableMove), 0.8f);
        }
    }

    private void PerformAttack()
    {
        Collider[] hitsEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider enemy in hitsEnemies)
        {
            if(enemy.TryGetComponent<NPCIATest>(out NPCIATest enemyScript))
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    private void EnableMove()
    {
        isAttack = false;
        playerMoviment.canMove = true;
    }

    public void TakeDamage(int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);

        UpdateLifeBar();

        Debug.Log("Player recebeu dano");

        if(currentLife <= 0)
        {
            Death();
        }
    }

    private void UpdateLifeBar()
    {
        if(lifeBar != null)
        {
            lifeBar.fillAmount = currentLife / maxLife;
        }
    }

    private void Death()
    {
        Debug.Log("O player morreu");
        anim.SetTrigger("death");
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
