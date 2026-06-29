using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("ตั้งค่าการแพนกล้อง")]
    public float lookUpDistance = 3f;   // ระยะที่กล้องจะเลื่อนขึ้น (บวก)
    public float lookDownDistance = -3f; // ระยะที่กล้องจะเลื่อนลง (ติดลบ)
    public float panSpeed = 5f;         // ความเร็วความนุ่มนวลตอนแพน

    private Vector3 defaultLocalPosition;
    private float targetY;

    void Start()
    {
        // จำตำแหน่งเริ่มต้นของตัวเองไว้ (ซึ่งก็คือจุดกึ่งกลางของ Player)
        defaultLocalPosition = transform.localPosition;
        targetY = defaultLocalPosition.y;
    }

    void Update()
    {
        // เช็คว่ามีคีย์บอร์ดเชื่อมต่ออยู่ไหม
        if (Keyboard.current == null) return;

        // ถ้ากดปุ่ม W (มองขึ้น)
        if (Keyboard.current.wKey.isPressed)
        {
            targetY = defaultLocalPosition.y + lookUpDistance;
        }
        // ถ้ากดปุ่ม S (มองลง)
        else if (Keyboard.current.sKey.isPressed)
        {
            targetY = defaultLocalPosition.y + lookDownDistance;
        }
        // ถ้าปล่อยปุ่ม (กลับมามองตรงกลาง)
        else
        {
            targetY = defaultLocalPosition.y;
        }

        // ค่อยๆ เลื่อนตำแหน่งแกน Y ไปหาเป้าหมายอย่างนุ่มนวล (Lerp)
        float newY = Mathf.Lerp(transform.localPosition.y, targetY, Time.deltaTime * panSpeed);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}