﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class HitEvent : UnityEvent<int> { }

public class DigitalDamageReceiver : MonoBehaviour
{
    public int HP;

    public HitEvent OnShutDown = new HitEvent();

    public int index = 0;

    public bool damagable = true;
    [SerializeField] private GameObject effect;
    [SerializeField] private Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (damagable && collider.tag == "PlayerAttack")
        {
            TakeDamage(collider.gameObject.GetComponent<ProjectileBehavior>().damage);
        }
    }
    public void TakeDamage(int dmg)
    {
        AudioManager.Instance.MonsterHurtAudio();
        GetComponentInParent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);
        HP -= dmg;
        if (HP <= 0)
        {
            damagable = false;
            OnShutDown.Invoke(index);
        }
    }
    private void ResetColor()
    {
        GetComponentInParent<SpriteRenderer>().color = Color.white;
    }
}
