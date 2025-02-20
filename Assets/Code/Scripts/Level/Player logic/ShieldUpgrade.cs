using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : UpgradeBehaviour
{
    [SerializeField] GameEvent ActivateShieldVFX;
    [SerializeField] GameEvent DeactivateShieldVFX;

    public void ActivateShield()
    {
        if (HasUpgrade())
        {
            ActivateShieldVFX.Raise(new Empty(), this);
        }
    }

    public void DeactivateShield()
    {
        if(HasUpgrade())
        {
            DeactivateShieldVFX.Raise(new Empty(), this);
        }
    }
}
