using UnityEngine;

public class RainCubes : MonoBehaviour
{
    public int amount = 10;

    public GameObject[] gameObjects;
    
    void Start()
    {
        int nextGameObject = 0;

        while (amount > 0)
        {
            GameObject clone = Object.Instantiate<GameObject>(
                gameObjects[nextGameObject],
                new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(-30, 30)),
                Random.rotation);

            nextGameObject = (nextGameObject + 1) % gameObjects.Length;

            --amount;
        }
    }
}
