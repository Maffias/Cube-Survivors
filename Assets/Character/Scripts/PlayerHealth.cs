using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; // Maksymalne zdrowie gracza
    public int currentHealth { get; set; } // Aktualne zdrowie gracza
    private Animator animator; // Animator moba
    public bool isAlive = true;

    void Start()
    {
        currentHealth = maxHealth; // Ustaw aktualne zdrowie na maksymalne zdrowie na pocz�tku gry
        animator = GetComponent<Animator>(); // Pobierz komponent Animator
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Zredukuj aktualne zdrowie o zadane obra�enia

        // Sprawd�, czy zdrowie gracza spad�o do 0 lub poni�ej
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        if (animator != null)
        {
            animator.Play("Death"); // Uruchom animacj� �mierci
        }
        Invoke("EndGame", 2f); // Zako�cz gr� po 2 sekundach
    }

    private void EndGame()
    {
        SpawnerScript spawnerScript = FindObjectOfType<SpawnerScript>(); // Znajd� obiekt z komponentem PlayerHealth
        if (spawnerScript != null)
        {
            spawnerScript.initialEnemyCount = 2;
            spawnerScript.waveNumber = 1;
        }

        SceneManager.LoadScene("MenuScene"); // Za�aduj scen� "GameOver"
    }
}