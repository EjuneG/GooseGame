using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseSpawner : MonoBehaviour
{
    public static GooseSpawner Instance;
    public Transform p1Position;
    public Transform p2Position;
    private Goose p1Goose;
    private Goose p2Goose;
    [SerializeField] Goose[] gooses;
    private Dictionary<string, Goose> gooseDictionary = new Dictionary<string, Goose>();
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }

        gooseDictionary.Add("BigGoose", gooses[0]);
        gooseDictionary.Add("QuickGoose", gooses[1]);
        gooseDictionary.Add("MageGoose", gooses[2]);

        p1Goose = gooseDictionary[CharacterSelection.Instance.p1GooseName];
        p2Goose = gooseDictionary[CharacterSelection.Instance.p2GooseName];

        p1Goose.gameObject.SetActive(true);
        p2Goose.gameObject.SetActive(true);
        Vector3 goose1Position = p1Position.position;
        Vector3 goose2Position = p2Position.position;

        p1Goose.transform.position = goose1Position;
        p2Goose.transform.position = goose2Position;

        CharacterSelection.Instance.setPlayerGooses(p1Goose, p2Goose);

    }
}
