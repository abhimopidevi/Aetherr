using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject solarFlarePrefab;
    public GameObject magmaRockPrefab;
    public GameObject blizzardPrefab;
    public Transform castPoint;


    private int elementA = -1;
    private int elementB = -1;

    private enum SpellType { None, SolarFlare, MagmaRock, Blizzard }
    private SpellType currentSpell = SpellType.None;

    void Update()
    {
        HandleElementInput();
        HandleShooting();
    }


    void HandleElementInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectElement(1); // Fire
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectElement(2); // Ice
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectElement(3); // Wind
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectElement(4); // Earth
    }

    void SelectElement(int element)
    {
        if (elementA == -1)
        {
            elementA = element;
        }
        else
        {
            elementB = element;
            DetermineSpell();
        }
    }

    void DetermineSpell()
    {
        currentSpell = SpellType.None;

        // Solar Flare
        if (elementA == 1 && elementB == 1)
            currentSpell = SpellType.SolarFlare;

        // MagmaRock
        else if ((elementA == 1 && elementB == 4) || (elementA == 4 && elementB == 1))
            currentSpell = SpellType.MagmaRock;

        // Blizzard
        else if ((elementA == 2 && elementB == 3) || (elementA == 3 && elementB == 2))
            currentSpell = SpellType.Blizzard;

        elementA = -1;
        elementB = -1;

        Debug.Log("Current Spell: " + currentSpell);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && currentSpell != SpellType.None)
        {
            ShootSpell();
        }
    }

    void ShootSpell()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - castPoint.position).normalized;

        GameObject spell = null;

        switch (currentSpell)
        {
            case SpellType.SolarFlare:
                spell = Instantiate(solarFlarePrefab, castPoint.position, Quaternion.identity);
                break;

            case SpellType.MagmaRock:
                spell = Instantiate(magmaRockPrefab, castPoint.position, Quaternion.identity);
                break;

            case SpellType.Blizzard:
                spell = Instantiate(blizzardPrefab, castPoint.position, Quaternion.identity);
                break;
        }

        spell.GetComponent<Projectile>().Launch(direction);
    }
}
