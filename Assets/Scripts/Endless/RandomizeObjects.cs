using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomizeObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsArray;
    [SerializeField] private int maxRandomObjectCount = 150;
    [SerializeField] private int maxZPosition = 13;
    [SerializeField] private int maxXPosition = 10;
    [SerializeField] private float localScaleMultiplierMin = 0.2f;
    [SerializeField] private float localScaleMultiplierMax = 0.5f;
    [SerializeField] private Transform parentTransform;
    private List<Vector3> usedPositionList = new List<Vector3>();

    private bool initialized = false;

    private void Awake()
    {
        if (initialized) return;
        initialized = true;
        GenerateRandomObjects();
    }

    private void GenerateRandomObjects()
    {
        if (objectsArray == null || objectsArray.Length == 0) return;

        for (int i = 0; i < maxRandomObjectCount; i++)
        {
            GameObject prefab = objectsArray[Random.Range(0, objectsArray.Length)];

            int randomX;
            if (Random.value < 0.5f)
                randomX = Random.Range(-maxXPosition, -1);     
            else
                randomX = Random.Range(2, maxXPosition + 1);  

            int randomZ = Random.Range(-maxZPosition, maxZPosition);

            Vector3 randomPos = new Vector3(randomX, 0, randomZ);
            if (usedPositionList.Contains(randomPos))
            {
                i--;
                continue;
            }
            usedPositionList.Add(randomPos);

            GameObject obj = Instantiate(prefab, transform);
            
            obj.transform.SetParent(parentTransform, false);
            obj.transform.localPosition = randomPos;
            obj.transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            obj.transform.localScale = Vector3.one * Random.Range(localScaleMultiplierMin, localScaleMultiplierMax);
        }
    }
}
