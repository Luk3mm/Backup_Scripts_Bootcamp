using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float playerLife = 100f;

    private bool canSave = false;
    private float lastLifeSave;
    private bool hasDateSave;

    private Vector3 lastPositionSave;


    // Start is called before the first frame update
    void Start()
    {
        LoadDateSave();
    }

    // Update is called once per frame
    void Update()
    {
        if(canSave && Input.GetKeyDown(KeyCode.B))
        {
            SaveDatePlayer();
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

        PlayerPrefs.SetFloat("PosX", lastPositionSave.x);
        PlayerPrefs.SetFloat("PosY", lastPositionSave.y);
        PlayerPrefs.SetFloat("PosZ", lastPositionSave.z);
        PlayerPrefs.SetFloat("Life", lastLifeSave);
        PlayerPrefs.SetInt("HasDateSave", 1);

        Debug.Log($"Jogo salvo! Posicao: {lastPositionSave}, Vida: {lastLifeSave}");

        canSave = false;
    }

    private void CleanDateSave()
    {
        PlayerPrefs.DeleteKey("PosX");
        PlayerPrefs.DeleteKey("PosY");
        PlayerPrefs.DeleteKey("PosZ");
        PlayerPrefs.DeleteKey("Life");
        PlayerPrefs.DeleteKey("HasDateSave");

        lastPositionSave = Vector3.zero;
        lastLifeSave = 0;
        hasDateSave = false;

        Debug.Log("Dados salvos foram limpos!");
    }

    private void LoadDateSave()
    {
        if (PlayerPrefs.GetInt("HasDateSave") == 1)
        {
            float posX = PlayerPrefs.GetFloat("PosX");
            float posY = PlayerPrefs.GetFloat("PosY");
            float posZ = PlayerPrefs.GetFloat("PosZ");
            lastPositionSave = new Vector3(posX, posY, posZ);
            lastLifeSave = PlayerPrefs.GetFloat("Life");

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
