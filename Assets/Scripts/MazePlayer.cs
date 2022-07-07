using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UniRx;
using UniRx.Triggers;

public class MazePlayer : NetworkBehaviour
{
    [SerializeField] private HitManager _hitManager; //余裕あればZenject
    [SerializeField] private GameObject _launchPosition;
    [SerializeField] private Bullet _bullet;
    
    private IInputProvider _inputProvider;
    
    //Weaponクラスを用意してもいい
    private bool _isShooting;
    private float _lapseTime = 0f;
    private readonly float _coolTime = 1f;
    private Vector3 _shootDirection;
    
    public override void OnStartLocalPlayer()
    {
        //InputProviderの設定
        _inputProvider = UnityInputProvider.Instance;
        
        //Cameraの設定
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        transform.LookAt(new Vector3(0f, 1f, 0f));
        _shootDirection = Vector3.Normalize(_launchPosition.transform.position - gameObject.transform.position);

    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        transform.Rotate(_inputProvider.GetRotate());
        
        if (!_hitManager.IsHit)
        {
            transform.Translate(_inputProvider.GetMove());
        }

        if (_inputProvider.GetShoot())
        {
            if (!_isShooting)
            {
                _isShooting = true;
                _lapseTime = 0f;
                _shootDirection = Vector3.Normalize(_launchPosition.transform.position - gameObject.transform.position);
                CmdShoot();
            }
        }

        if (_isShooting)
        {
            _lapseTime += Time.deltaTime;
            if (_lapseTime >= _coolTime)
            {
                _isShooting = false;
                _lapseTime = 0f;
            }
        }
    }
    
    [Command]
    void CmdShoot()
    {
        Bullet bullet = Instantiate(_bullet, _launchPosition.transform);
        bullet.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        NetworkServer.Spawn(bullet.gameObject);
        bullet.Shoot(_shootDirection);
    }
    
    
}
