using UnityEngine;
public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown; // Czas, po kt�rym mo�e nast�pi� kolejny atak
    [SerializeField]
    private int attackDamage;
    private Animator animator;
    private bool isCoolingDown;
    private float nextAttackAllowed; // Czas, kiedy nast�pny atak jest dozwolony

    void Start()
    {
        // Pobierz komponent Animator z obiektu 
        animator = GetComponent<Animator>();
        isCoolingDown = false;
        nextAttackAllowed = 0f; // Ustaw warto�� pocz�tkow� na 0f, aby umo�liwi� natychmiastowy atak
    }

    public void CheckAttack()
    {
        if (Time.time > nextAttackAllowed) // Je�li czas jest wi�kszy ni� czas nast�pnego ataku
        {
            if (!isCoolingDown) // Je�li nie jest w trakcie odnowienia
            {
                animator.Play("Attack"); 
                isCoolingDown = true; // Ustaw flag� odnowienia
                Invoke("ResetAttack", attackCooldown); // Wywo�aj metod� ResetAttack po okre�lonym czasie

                // Zadaj obra�enia graczowi
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(attackDamage);
                    }
                }
            }
        }
    }

    void ResetAttack()
    {
        animator.SetBool("isAttacking", false); // Zresetuj animacj� ataku
        isCoolingDown = false; // Zresetuj flag� odnowienia
        nextAttackAllowed = Time.time + attackCooldown; // Ustaw czas nast�pnego ataku na bie��cy czas plus czas odnowienia
    }
}