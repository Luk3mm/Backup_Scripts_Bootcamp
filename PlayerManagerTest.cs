using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerTest : MonoBehaviour
{
    public float playerLife;

    private bool canSave = false;
    private float lastLifeSave;
    private bool hasDateSave;
    private string sceneName;

    public GameObject bowFireSave;
    public GameObject bowFireSave2;

    private Vector3 lastPositionSave;


    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        LoadDateSave();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSave && Input.GetKeyDown(KeyCode.B))
        {
            SaveDatePlayer();
            bowFireSave.SetActive(true);
            bowFireSave2.SetActive(true);

            Invoke("DisableFireSave", 10f);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CleanDateSave();
        }
    }

    private void SaveDatePlayer()
    {
        lastPositionSave = transform.position;
        lastLifeSave = playerLife;

        PlayerPrefs.SetFloat(sceneName + "PosX", lastPositionSave.x);
        PlayerPrefs.SetFloat(sceneName + "PosY", lastPositionSave.y);
        PlayerPrefs.SetFloat(sceneName + "PosZ", lastPositionSave.z);
        PlayerPrefs.SetFloat(sceneName + "Life", lastLifeSave);
        PlayerPrefs.SetInt(sceneName + "HasDateSave", 1);

        Debug.Log($"Jogo salvo! Posicao: {lastPositionSave}, Vida: {lastLifeSave}");

        canSave = false;
    }

    private void CleanDateSave()
    {
        PlayerPrefs.DeleteKey(sceneName + "PosX");
        PlayerPrefs.DeleteKey(sceneName + "PosY");
        PlayerPrefs.DeleteKey(sceneName + "PosZ");
        PlayerPrefs.DeleteKey(sceneName + "Life");
        PlayerPrefs.DeleteKey(sceneName + "HasDateSave");

        lastPositionSave = Vector3.zero;
        lastLifeSave = 0;
        hasDateSave = false;

        Debug.Log("Dados salvos foram limpos!");
    }

    private void LoadDateSave()
    {
        if (PlayerPrefs.GetInt("HasDateSave") == 1)
        {
            float posX = PlayerPrefs.GetFloat(sceneName + "PosX");
            float posY = PlayerPrefs.GetFloat(sceneName + "PosY");
            float posZ = PlayerPrefs.GetFloat(sceneName + "PosZ");
            lastPositionSave = new Vector3(posX, posY, posZ);
            lastLifeSave = PlayerPrefs.GetFloat(sceneName + "Life");

            transform.position = lastPositionSave;
            playerLife = lastLifeSave;
            hasDateSave = true;

            Debug.Log($"Jogo carregado! Posicao: {lastPositionSave}, Vida: {lastLifeSave}");
        }
        else
        {
            Debug.Log("Nenhum dado encontrado!");
        }
    }

    private void DisableFireSave()
    {
        bowFireSave.SetActive(false);
        bowFireSave2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SavePoint"))
        {
            Debug.Log("Colidiu com o ponto de salvamento");
            canSave = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SavePoint"))
        {
            Debug.Log("Saiu do o ponto de salvamento");
            canSave = false;
        }
    }
}
