using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public ParticleSystem fire;
    public ParticleSystem rings;
    public ParticleSystem embersSmall;
    public ParticleSystem embersFlickering;
    public ParticleSystem fireMains;

    private ParticleSystem.MainModule fireMain;
    private ParticleSystem.EmissionModule fireEmission;

    private ParticleSystem.MainModule ringsMain;
    private ParticleSystem.EmissionModule ringsEmission;

    private ParticleSystem.MainModule embersSmallMain;
    private ParticleSystem.EmissionModule embersSmallEmission;

    private ParticleSystem.MainModule embersFlickeringMain;
    private ParticleSystem.EmissionModule embersFlickeringEmission;

    private ParticleSystem.MainModule fireMainsMain;
    private ParticleSystem.EmissionModule fireMainsEmission;

    private Vector3 fireMainsOriginalScale;

    private float fireRateOverTime = 5f;
    private float ringsRateOverTime = 5f;
    private float embersSmallRateOverTime = 20f;
    private float embersFlickeringRateOverTime = 3f;
    private float fireMainsRateOverTime = 5f;

    private void Awake()
    {
        fireMain = fire.main;
        fireEmission = fire.emission;

        ringsMain = rings.main;
        ringsEmission = rings.emission;

        embersSmallMain = embersSmall.main;
        embersSmallEmission = embersSmall.emission;

        embersFlickeringMain = embersFlickering.main;
        embersFlickeringEmission = embersFlickering.emission;

        fireMainsMain = fireMains.main;
        fireMainsEmission = fireMains.emission;

        fireMainsOriginalScale = fireMains.transform.localScale;
    }

    private void Update()
    {
        // Decrease rate over time every 20 seconds
        fireRateOverTime = Mathf.Max(5f, fireRateOverTime - (0.3f * Time.deltaTime));
        ringsRateOverTime = Mathf.Max(5f, ringsRateOverTime - (0.75f * Time.deltaTime));
        embersSmallRateOverTime = Mathf.Max(20f, embersSmallRateOverTime - (1.5f * Time.deltaTime));
        embersFlickeringRateOverTime = Mathf.Max(5f, embersFlickeringRateOverTime - (1.5f * Time.deltaTime));
        fireMainsRateOverTime = Mathf.Max(5f, fireMainsRateOverTime - (1.5f * Time.deltaTime));

        // Decrease Fire Mains scale every 20 seconds
        Vector3 decreaseScale = new Vector3(0.01f, 0.01f, 0.01f) * Time.deltaTime;
        fireMains.transform.localScale = Vector3.Max(fireMainsOriginalScale * 0.3f, fireMains.transform.localScale - decreaseScale);

        // Update the emissions
        fireEmission.rateOverTime = fireRateOverTime;
        ringsEmission.rateOverTime = ringsRateOverTime;
        embersSmallEmission.rateOverTime = embersSmallRateOverTime;
        embersFlickeringEmission.rateOverTime = embersFlickeringRateOverTime;
        fireMainsEmission.rateOverTime = fireMainsRateOverTime;
    }



    public void OnWoodAdded()
    {
        // Increase rate over time
        fireRateOverTime = 50f;
        ringsRateOverTime += 20f;
        embersSmallRateOverTime = 100f;
        embersFlickeringRateOverTime = Mathf.Max(20f, embersFlickeringRateOverTime);
        fireMainsRateOverTime = Mathf.Max(15f, fireMainsRateOverTime);

        // Increase Fire Mains scale
        fireMains.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);

        // Apply changes instantly
        fireEmission.rateOverTime = fireRateOverTime;
        ringsEmission.rateOverTime = ringsRateOverTime;
        embersSmallEmission.rateOverTime = embersSmallRateOverTime;
        embersFlickeringEmission.rateOverTime = embersFlickeringRateOverTime;
        fireMainsEmission.rateOverTime = fireMainsRateOverTime;

        // Start the co-routines to decrease values after 2 seconds
        StartCoroutine(DecreaseAfterSeconds(fireEmission, 200f, 80f, 1f));
        StartCoroutine(DecreaseAfterSeconds(ringsEmission, ringsRateOverTime, 80f, 1f));
        StartCoroutine(DecreaseAfterSeconds(embersSmallEmission, 100f, 30f, 1f));
        StartCoroutine(DecreaseAfterSeconds(embersFlickeringEmission, embersFlickeringRateOverTime, 15f, 1f));
        StartCoroutine(DecreaseAfterSeconds(fireMainsEmission, fireMainsRateOverTime, 10f, 1f));
    }

    private IEnumerator DecreaseAfterSeconds(ParticleSystem.EmissionModule emission, float startingValue, float decreaseValue, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        emission.rateOverTime = startingValue - decreaseValue;
    }
}