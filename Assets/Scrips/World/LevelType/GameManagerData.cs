using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="New GameData", menuName = "Game Data")]
public class GameManagerData : ScriptableObject
{
    public void Level(int nivel)
    {
        if (nivel == 1)
        {
            GameManager.Instance.AddComponent<Level_1>();
        }
        if (nivel == 2)
        {
            Debug.Log("Crear nivel 2");
        }
        if (nivel == 3)
        {
            Debug.Log("Crear nivel 3");
        }
        if (nivel == 4)
        {
            Debug.Log("Crear nivel 4");
        }

    }
}
