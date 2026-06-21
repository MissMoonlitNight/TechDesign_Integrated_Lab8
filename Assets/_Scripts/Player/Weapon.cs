using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Data")]
    public WeaponData data;
    public Transform firePoint;

    [Header("UI")]
    public Text ammoText;

    [Header("Отдача")]
    public float recoilRecoverySpeed = 5f;

    private int currentAmmo;
    private int currentReserve;
    private float nextFireTime;
    private bool isReloading;
    private Camera playerCamera;

    // Переменные для контроля отдачи
    private Vector3 cameraBaseRotation;
    private float currentRecoil = 0f;

    void Start()
    {
        playerCamera = Camera.main;
        cameraBaseRotation = playerCamera.transform.localEulerAngles;
        currentAmmo = data.magazineSize;
        currentReserve = data.totalReserve;
        UpdateUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (IsAnyUIOpen()) return;

        // Стрельба (ЛКМ)
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + data.fireRate;
        }

        // Перезарядка (R)
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < data.magazineSize && currentReserve > 0)
        {
            StartCoroutine(Reload());
        }

        if (currentRecoil > 0.01f)
        {
            currentRecoil = Mathf.Lerp(currentRecoil, 0f, Time.deltaTime * recoilRecoverySpeed);
            Vector3 targetRot = cameraBaseRotation;
            targetRot.x -= currentRecoil;
            playerCamera.transform.localEulerAngles = targetRot;
        }
        else if (currentRecoil > 0f)
        {
            currentRecoil = 0f;
        }
    }

    bool IsAnyUIOpen()
    {
        if (PauseMenu.Instance != null && (PauseMenu.Instance.IsInMainMenu() || PauseMenu.Instance.IsPaused()))
            return true;

        if (DialogueManager.Instance != null && DialogueManager.Instance.dialoguePanel.activeSelf)
            return true;

        if (UIManager.Instance != null && UIManager.Instance.IsAnyUIOpen())
            return true;

        var questPanel = GameObject.Find("QuestPanel");
        if (questPanel != null && questPanel.activeSelf) return true;

        var talentPanel = GameObject.Find("TalentPanel");
        if (talentPanel != null && talentPanel.activeSelf) return true;

        var craftPanel = GameObject.Find("CraftingPanel");
        if (craftPanel != null && craftPanel.activeSelf) return true;

        return false;
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateUI();

        // Звук выстрела
        if (data.shootSound != null)
            AudioSource.PlayClipAtPoint(data.shootSound, transform.position);

        // Вспышка
        if (data.muzzleFlashPrefab != null)
            Instantiate(data.muzzleFlashPrefab, firePoint != null ? firePoint.position : transform.position, Quaternion.identity);

        float finalDamage = data.damage;
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            finalDamage += playerStats.damage;
        }

        int pellets = (data.weaponName == "Shotgun") ? 8 : 1;

        for (int i = 0; i < pellets; i++)
        {

            Vector3 direction = playerCamera.transform.forward;
            direction += Random.insideUnitSphere * data.spread;
            direction.Normalize();

            if (data.projectilePrefab != null && firePoint != null)
            {
                Quaternion rot = Quaternion.LookRotation(direction);
                GameObject bullet = Instantiate(data.projectilePrefab, firePoint.position, rot);

                // Передаем урон в скрипт пули
                BulletProjectile bp = bullet.GetComponent<BulletProjectile>();
                if (bp != null)
                {
                    bp.damage = finalDamage;
                }
            }
            else
            {

                if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit, 100f))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(finalDamage);
                    }
                }
            }
        }

        // Отдача камеры
        currentRecoil += data.recoilForce;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (data.reloadSound != null)
            AudioSource.PlayClipAtPoint(data.reloadSound, transform.position);

        yield return new WaitForSeconds(2f);

        int needed = data.magazineSize - currentAmmo;
        int take = Mathf.Min(needed, currentReserve);
        currentAmmo += take;
        currentReserve -= take;

        isReloading = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo}/{currentReserve}";
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isReloading = false;
    }

    private void OnEnable()
    {
        UpdateUI();
    }
}