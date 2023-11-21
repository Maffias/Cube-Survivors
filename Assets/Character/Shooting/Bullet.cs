using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float baseDamage; // Podstawowe obra�enia

    // Wsp�czynnik z jakim skaluj� si� obra�enia wraz ze wzrostem numeru fali
    [SerializeField]
    public float damageScaleFactor = 1.3f;

    // Obliczane obra�enia
    public int damage
    {
        get
        {
            // Oblicz obra�enia na podstawie numeru fali
            return Mathf.RoundToInt(baseDamage * Mathf.Pow(damageScaleFactor, SpawnerScript.waveNumber));
        }
    }
}