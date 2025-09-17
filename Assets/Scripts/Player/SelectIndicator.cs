using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicator : MonoBehaviour
{
    private PlayerController player;

    [SerializeField] private float speed=15;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.selectedOne.transform.position;

        transform.position = Vector2.Lerp(transform.position, targetPosition,
            Time.deltaTime*speed);
    }
}
