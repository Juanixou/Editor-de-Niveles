using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    private GameObject[] instancias;
    private GameObject player;
    private GameObject saver;
    GameObject[] enemies;
    List<Vector3> enemiesPosition;
    public string transformacion;
    public GameObject play;
    public GameObject pause;
    Transform objects;

    private Vector3 playerPosition;
    private Vector3 cameraPosition;
    private string curState;

    // Use this for initialization
    void Start () {
        transformacion = "Move";
        objects = GameObject.Find("Objects").transform;
        saver = GameObject.Find("DataController");
        enemiesPosition = new List<Vector3>();
        curState = "Editar";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Selection()
    {

        foreach (Transform instancias in objects)
        {
            if (instancias.GetComponent<MoveObject>() != null)
            {
                instancias.GetComponent<MoveObject>().SelectTransform(EventSystem.current.currentSelectedGameObject.name);
            } 
        }
        transformacion = EventSystem.current.currentSelectedGameObject.name;
    }

    public void Comenzar()
    {
        if (saver.GetComponent<SaveGround>().IsSaveNecessary())
        {
            if (CheckSave())
            {
                saver.GetComponent<SaveGround>().SaveData();
            }
            else
            {
                return;
            }
            
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        player.GetComponentInChildren<Animator>().enabled = true;
        playerPosition = player.transform.position;
        cameraPosition = Camera.main.transform.position;

        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 3;
        player.GetComponent<MoveObject>().enabled = false;
        //player.GetComponent<PlayerStats>().FillHealth();
        player.GetComponent<PlayerStats>().enabled = true;

        enemiesPosition.Clear();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            enemiesPosition.Add(enemy.transform.position);
            enemy.GetComponent<MoveObject>().enabled = false;
            if (enemy.GetComponentInChildren<Animator>() != null) enemy.GetComponentInChildren<Animator>().enabled = true;
            if (enemy.GetComponent<BasicEnemyMovement>() != null) enemy.GetComponent<BasicEnemyMovement>().enabled = true;
            if (enemy.GetComponent<MyRayCast>() != null)
            {
                enemy.GetComponent<MyRayCast>().enabled = true;
                enemy.GetComponent<MyRayCast>().SetInitialPosition(enemy.transform.position);
            }
            if (enemy.GetComponent<MyDistanceAttack>() != null) enemy.GetComponent<MyDistanceAttack>().enabled = true;
            if (enemy.GetComponent<Damage>() != null) enemy.GetComponent<Damage>().enabled = true;
            if (enemy.GetComponent<BoxCollider2D>()) enemy.GetComponent<BoxCollider2D>().enabled = true;
            enemy.GetComponent<EnemiesHealth>().FillHealth();
            enemy.GetComponent<EnemiesHealth>().enabled = true;
            if (enemy.name.Contains("Enemy 1")|| enemy.name.Contains("Enemy 3"))
            {
                enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }


        Camera.main.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().enabled = true;
        Camera.main.GetComponent<MoveCamera>().enabled = false;
        play.SetActive(false);
        pause.SetActive(true);
        curState = "Play";
    }

    public void Editar()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<PlayerStats>().FillHealth();
        player.GetComponent<PlayerStats>().enabled = false;
        player.GetComponentInChildren<Animator>().SetBool("Correr",false);
        player.GetComponentInChildren<Animator>().SetBool("Caminar",false);
        Camera.main.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().enabled = false;
        Camera.main.GetComponent<MoveCamera>().enabled = true;
        player.transform.position = playerPosition;
        Camera.main.transform.position = cameraPosition;
        
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.GetComponent<MoveObject>().enabled = true;
        }

        int count = 0;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.position = enemiesPosition[count];
            enemy.gameObject.SetActive(true);
            enemy.GetComponent<MoveObject>().enabled = true;
            if (enemy.GetComponentInChildren<Animator>() != null) enemy.GetComponentInChildren<Animator>().enabled = false;
            if (enemy.GetComponent<BasicEnemyMovement>() != null) enemy.GetComponent<BasicEnemyMovement>().enabled = false;
            if (enemy.GetComponent<MyRayCast>() != null) enemy.GetComponent<MyRayCast>().enabled = false;
            if (enemy.GetComponent<MyDistanceAttack>() != null) enemy.GetComponent<MyDistanceAttack>().enabled = false;
            if (enemy.GetComponent<Damage>() != null) enemy.GetComponent<Damage>().enabled = false;
            enemy.GetComponent<EnemiesHealth>().FillHealth();
            enemy.GetComponent<EnemiesHealth>().enabled = false;

            if (enemy.name.Contains("Enemy 1") || enemy.name.Contains("Enemy 3"))
            {
                enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            count++;
        }
        
        play.SetActive(true);
        pause.SetActive(false);
        curState = "Editar";

        if (!saver.GetComponent<SaveGround>().tempPath.Equals(saver.GetComponent<SaveGround>().dataPath))
        {
            saver.GetComponent<SaveGround>().tempPath = saver.GetComponent<SaveGround>().dataPath;
            saver.GetComponent<SaveGround>().LoadData();
        }

    }

    public bool CheckSave()
    {

        DialogResult result = MessageBox.Show("Se guardará el nivel. Continuar?", "Cuidado!",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Exclamation);
        if (result == DialogResult.OK)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

}
