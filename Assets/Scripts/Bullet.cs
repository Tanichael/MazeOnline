using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Mirror;
using Mirror.Examples.AdditiveScenes;
using UniRx;
using UniRx.Triggers;

public class Bullet : NetworkBehaviour
{
    [SyncVar]
    private int _shooter;

    private readonly float _speed = 10f;

    public int Shooter
    {
        get => _shooter;
        set => _shooter = value;
    }

    void Update()
    {
        gameObject.transform.position += new Vector3(1, 1, 1) * _speed * Time.deltaTime;
    }

    public void Shoot(Vector3 shootDirection)
    {
        // gameObject.transform.SetParent(null);
        //
        // this.UpdateAsObservable()
        //     .Subscribe(_ =>
        //     {
        //         gameObject.transform.position += shootDirection * _speed * Time.deltaTime;
        //     });
    }
    
}
