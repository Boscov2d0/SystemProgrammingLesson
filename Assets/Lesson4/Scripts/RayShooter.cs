using System.Collections;
using UnityEngine;

public class RayShooter : FireAction
{
    private Camera _camera;
    private int _damage = 25;
    protected override void Start()
    {
        base.Start();
        _camera = GetComponentInChildren<Camera>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }
        if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    protected override void Shooting()
    {
        base.Shooting();
        if (bullets.Count > 0)
        {
            StartCoroutine(Shoot());
        }
    }
    private IEnumerator Shoot()
    {
        if (reloading)
        {
            yield break;
        }
        
        Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray ray = _camera.ScreenPointToRay(point);
        /*if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.TryGetComponent(out PlayerCharacter player))
            {
                player.CheckHealth(_damage);
                Debug.Log("Shoot");
            }
        }
        else
        {
            yield break;
        }*/
        if (!Physics.Raycast(ray, out var hit))
        {
            yield break;
        }

        var shoot = bullets.Dequeue();
        string bulletCount = bullets.Count.ToString();
        ammunition.Enqueue(shoot);
        shoot.SetActive(true);
        shoot.transform.position = hit.point;
        shoot.transform.parent = hit.transform;
        yield return new WaitForSeconds(2.0f);
        shoot.SetActive(false);
    }
}