using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private int amount;
    [SerializeField] private Vector2 maxRange;
    [SerializeField] private Vector2 minRange;

    [SerializeField] private GameObject cube;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++) {
            float x = Random.Range(minRange.x, maxRange.x);
            float z = Random.Range(minRange.y, maxRange.y);
            GameObject spawnedCube = Instantiate(cube, new Vector3(x, 0, z), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
