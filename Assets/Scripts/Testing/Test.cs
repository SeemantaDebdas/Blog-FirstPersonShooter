using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] GameObject arrowPointer;
    [SerializeField] Transform originPos;
    [SerializeField] LayerMask hittableLayer;
    [SerializeField] float weaponRange;

    [SerializeField] float movementAmt = 20;

    float movement;

    Color color;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            movement -= movementAmt * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            movement += movementAmt * Time.deltaTime;

        transform.position = new Vector3(movement,transform.position.y,transform.position.z);

        if (Physics.Raycast(originPos.position, originPos.transform.forward, out RaycastHit hit, weaponRange, hittableLayer))
        {
            arrowPointer.SetActive(true);
            arrowPointer.transform.position = hit.point;
            arrowPointer.transform.rotation = Quaternion.LookRotation(hit.normal);
            color = Color.green;

        }
        else
        {
            arrowPointer.SetActive(false);
            color = Color.red;
        }

        Debug.DrawRay(originPos.position, originPos.forward * weaponRange,color);
    }
}
