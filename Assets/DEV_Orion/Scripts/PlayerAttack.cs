    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class PlayerAttack : MonoBehaviour
    {
        public GameObject player;
        public WeaponEquip weaponScript;
        private InputHandler inputHandler;
        public CapsuleCollider attackCollider;
        public float swingSpeed = .5f;
        public float damage = 10f;
        public float attackDelay = 1f;
        public float attackTimer = 1f;
        private AudioSource audioSource;
        public AudioClip[] hammerAttackSounds;
        public AudioClip[] swordAttackSounds;

        public PlayerLocomotion pl;
        private Shop shopScript;

        private StateFlipping stateScript;
        private bool isTrippy;

        // Start is called before the first frame update
        void Start()
        {
            shopScript = FindObjectOfType<Shop>();
            inputHandler = player.GetComponent<InputHandler>();
            attackCollider = GetComponent<CapsuleCollider>();
            audioSource = GetComponent<AudioSource>();
            stateScript = player.GetComponent<StateFlipping>();
            weaponScript = player.GetComponent<WeaponEquip>();
        }

        void Update()
        {
            if (attackTimer < attackDelay)
            {
                attackTimer += Time.deltaTime;
            }
            isTrippy = stateScript.isTrippy;
        }

        private void OnTriggerStay(Collider other)
        {
            if (inputHandler.punchFlag && attackTimer >= attackDelay)
            {
                HostileBehavior otherHostile = other.gameObject.GetComponent<HostileBehavior>();
                HostileBehavior parentHostile = other.gameObject.GetComponentInParent<HostileBehavior>();

                if (otherHostile != null)
                {
                    attackTimer = 0f;
                    if (!isTrippy && weaponScript.activeWeapon != weaponScript.EVMM)
                    {
                        otherHostile.TakeDamage(damage);
                        stateScript.timer += damage;
                        Debug.Log($"damage done: {damage}");
                    }
                }
                else if (parentHostile != null)
                {
                    attackTimer = 0f;
                    if (!isTrippy && weaponScript.activeWeapon != weaponScript.EVMM)
                    {
                        parentHostile.TakeDamage(damage);
                        stateScript.timer += damage;
                        Debug.Log($"damage done: {damage}");
                    }
                }
            }

            if (inputHandler.punchFlag && attackTimer >= attackDelay && other.gameObject.tag == "merchant")
            {
                shopScript.dead();
            }
        }
    }
}
