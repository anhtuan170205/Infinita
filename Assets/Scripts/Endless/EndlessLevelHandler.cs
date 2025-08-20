using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] sectionsPrefab;
    private GameObject[] sectionsPool = new GameObject[20];
    private GameObject[] activeSections = new GameObject[10];
    private Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);
    const float sectionLength = 26;

    private void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
        int prefabIndex = 0;
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionsPrefab[prefabIndex]);
            sectionsPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex >= sectionsPrefab.Length)
            {
                prefabIndex = 0;
            }
        }

        for (int i = 0; i < activeSections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, -10, i * sectionLength);
            randomSection.SetActive(true);

            activeSections[i] = randomSection;
        }
        StartCoroutine(UpdateLessOftenCO());
    }

    private IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    private void UpdateSectionPositions()
    {
        for (int i = 0; i < activeSections.Length; i++)
        {
            if (activeSections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = activeSections[i].transform.position;
                activeSections[i].SetActive(false);

                activeSections[i] = GetRandomSectionFromPool();

                activeSections[i].transform.position = new Vector3(lastSectionPosition.x, -10, lastSectionPosition.z + sectionLength * activeSections.Length);
                activeSections[i].SetActive(true);
            }
        }
    }

    private GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, sectionsPool.Length);
        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!sectionsPool[randomIndex].activeInHierarchy)
            {
                isNewSectionFound = true;
            }
            else
            {
                randomIndex++;
                if (randomIndex >= sectionsPool.Length)
                {
                    randomIndex = 0;
                }
            }
        }
        return sectionsPool[randomIndex];
    }
}
