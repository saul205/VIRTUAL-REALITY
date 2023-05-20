using UnityEngine;

public class ZombieCharacterControl : MonoBehaviour
{

    [SerializeField] private Animator m_animator = null;

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
    }
}
