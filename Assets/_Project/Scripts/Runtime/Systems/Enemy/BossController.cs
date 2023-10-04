using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private SpawnManager _spawn;
    private SceneryManager _sceneryManager;

    public Boss _boss;
    public GameObject[] _lackeys; // lacaios
    public List<GameObject> _lackeysInScene; // em cena
    public float _delaySpawnLackeys;
    public bool _canInstantiate;
    public int _quantityOfLackeys;

    public float minTime, maxTime;
    private float tempTime, routineTime;
    private bool isActive;


    private void Start()
    {
        _sceneryManager = GetComponent<SceneryManager>();
        _spawn = GetComponent<SpawnManager>();

        _sceneryManager.OnBattleBoss += StartControl;
       
    }

    private void OnDestroy()
    {
        _sceneryManager.OnBattleBoss -= StartControl;
    }

    private void Update()
    {
        if (_spawn._currentGame != GameState.Battleboss)
        {
            return;
        }

        if (_lackeysInScene.Count < _quantityOfLackeys && !_canInstantiate)
        {
            _canInstantiate = true;
            StartCoroutine(CallLackeys());
        }

        ControlEnemies();
    }

    private void StartControl()
    {
        print(" BOSS BATTLE");

        StartCoroutine(ControlBoss());
    }

    private IEnumerator ControlBoss()
    {
        yield return new WaitForSeconds(2);

        _boss.ChangeState(EnemyState.Chase);
        yield return new WaitForSeconds(2);

        _boss.ChangeState(EnemyState.Escape);
        yield return new WaitForSeconds(2);

        _boss.ChangeState(EnemyState.Idle);
        yield return new WaitForSeconds(2);

        _boss.JumpAttack();
        yield return new WaitForSeconds(0.3f);
        _boss.isJump = true;

        StartCoroutine(ControlBoss());
    }

    private void ControlEnemies()
    {
        if (_lackeysInScene.Count > 0)
        {
            if (!isActive)
            {
                routineTime = Random.Range(minTime, maxTime);
                isActive = true;
            }

            tempTime += Time.deltaTime;

            if (tempTime >= routineTime)
            {
                foreach (GameObject enemy in _lackeysInScene)
                {
                    int percMov = Random.Range(0, 99);

                    if (percMov > 80)
                    {
                        enemy.SendMessage("ChangeState", EnemyState.Positioning, SendMessageOptions.DontRequireReceiver);
                    }
                    else if (percMov > 45)
                    {
                        enemy.SendMessage("ChangeState", EnemyState.Patrol, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        enemy.SendMessage("ChangeState", EnemyState.Chase, SendMessageOptions.DontRequireReceiver);
                    }
                }

                tempTime = 0;
                isActive = false;
            }
        }
    }


    private IEnumerator CallLackeys()
    {
        yield return new WaitForSeconds(_delaySpawnLackeys);

        SpawnEnemy();

        _canInstantiate = false;
    }

    private void SpawnEnemy()
    {
        int rand = Random.Range(0, 99);
        bool isLeft = false;
        Vector3 posIns = Vector3.zero;

        if (rand < 50)
        {
            posIns = new Vector3(_spawn._camera.transform.position.x + _spawn._outCam * -1, 0, _spawn._playerController.transform.position.z);
            isLeft = true;
        }
        else
        {
            posIns = new Vector3(_spawn._camera.transform.position.x + _spawn._outCam, 0, _spawn._playerController.transform.position.z);
            isLeft = false;
        }

        GameObject temp = Instantiate(_lackeys[Random.Range(0,_lackeys.Length)], posIns, transform.localRotation);
        temp.SendMessage("SetIsPosition", isLeft, SendMessageOptions.DontRequireReceiver);
        temp.SendMessage("SetIsBoss", true, SendMessageOptions.DontRequireReceiver);
        temp.SendMessage("SetBoss", gameObject, SendMessageOptions.DontRequireReceiver);
        temp.SendMessage("ChangeState", EnemyState.Chase, SendMessageOptions.DontRequireReceiver);
        _lackeysInScene.Add(temp);
    }

    public void RemoveLackeys(GameObject lackey)
    {
        _lackeysInScene.Remove(lackey);
    }
}
