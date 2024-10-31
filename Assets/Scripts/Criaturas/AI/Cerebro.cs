using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerebro : MonoBehaviour
{
    public List<Accion> Lista_Acciones = new List<Accion>();  // Cambiado a una lista
    public Accion Sig_Accion;
    public Accion Accion_Actual;
    private bool Accionando = false;

    //esta función agarra la lista de acciones y las revisa una por una para sacar a la que tenga mayor puntaje.
    private void Calcular_Acciones(List<Accion> Lista_Compara)
    {
        //un float para saber cual tiene mayor puntaje
        float Mayor_Puntaje = 0f;

        //un int que se utiliza para ver la posición de la acción en la lista.
        int Num_Accion = 0;

    // Validación para evitar errores si la lista está vacía
        if (Lista_Compara == null || Lista_Compara.Count == 0)
        {
            Debug.Log("La lista de acciones está vacía");
            return;
        }
            //un for que revisa cada posición en la Lista
            for (int i = 0; i < Lista_Compara.Count; i++)
            {
                // si una acción en alguna posición tiene un puntaje mayor al actualmente mayor, entonces lo reemplaza.
                if (Lista_Compara[i].Calculo_Puntaje() > Mayor_Puntaje)
                {
                    Num_Accion = i;
                    Mayor_Puntaje = Lista_Compara[i].Calculo_Puntaje();
                }
            }

        Sig_Accion = Lista_Acciones[Num_Accion];
    }


    //agarra una acción y la ejecuta.
    private void Ejecutar_Accion(Accion Acc)
        {
            if (Acc != null)
            {
                Acc.Ejecutar();
            }
        }


    void LateUpdate()
    {
        //revisa si no está accionando, en cuyo caso:
        if (!Accionando)
        {
            //calcula la acción con mayor puntos y le otorga el valor sig accion
            Calcular_Acciones(Lista_Acciones);
            if (Sig_Accion != Accion_Actual && Sig_Accion != null && Accion_Actual != null){Accion_Actual.Terminar();}
                // ejecuta la acción asignada a sig_accion
                Ejecutar_Accion(Sig_Accion);
                    //Aplica acción actual
                    Accion_Actual = Sig_Accion;
                        //Accionando se vuelve verdadero
                        Accionando = true;
        }
        else
        {
            //si la acción actual existe
            if (Accion_Actual != null)
            {
                //entonces revisa si ya terminó de ejecutarse
                if(Accion_Actual.Revisar() == true)
                {
                    //La settea en falso.
                    Accionando = false;
                }
            }
        }
    }
}
