using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasivo : Reaccion_Base
{
    protected override void Reaccion_Agresivo(Base Z)
    {}
        
    //reacción contra pasivo
    protected override void Reaccion_Neutral(Base Z)
    {}

    
    //reacción contra agresivo
    protected override void Reaccion_Pasivo(Base Z)
    {}

}
