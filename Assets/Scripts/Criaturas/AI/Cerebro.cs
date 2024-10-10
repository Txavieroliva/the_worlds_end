using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerebro : MonoBehaviour
{
    public List<Accion> Lista_Acciones = new List<Accion>();  // Cambiado a una lista
    public Accion Sig_Accion;
    public Accion Accion_Actual;
    private bool Accionando = false;

    public void Start()
    {
        // Huir accionHuir = gameObject.AddComponent<Huir>();  // Añade el componente Huir al GameObject
        // Lista_Acciones.Add(accionHuir);

    }

    private void Calcular_Acciones(List<Accion> Lista_Compara)
    {
        float Mayor_Puntaje = 0f;
        int Num_Accion = 0;

    // Validación para evitar errores si la lista está vacía
        if (Lista_Compara == null || Lista_Compara.Count == 0)
        {
            Debug.Log("La lista de acciones está vacía");
            return;
        }

            for (int i = 0; i < Lista_Compara.Count; i++)
            {
                if (Lista_Compara[i].Calculo_Puntaje() > Mayor_Puntaje)
                {
                    Num_Accion = i;
                    Mayor_Puntaje = Lista_Compara[i].Puntos;
                }
            }

        Sig_Accion = Lista_Acciones[Num_Accion];
    }
    private void Ejecutar_Accion(Accion Acc)
        {
            if (Acc != null)
            {
                Acc.Ejecutar();
            }
        }

    void LateUpdate()
    {
        if (!Accionando)
        {
            Calcular_Acciones(Lista_Acciones);
            Ejecutar_Accion(Sig_Accion);
            Accion_Actual = Sig_Accion;
            Accionando = true;
        }
        else
        {
            if (Accion_Actual != null)
            {
                if(Accion_Actual.Revisar() == true)
                {
                    Accionando = false;
                }
            }
        }
    }
}
