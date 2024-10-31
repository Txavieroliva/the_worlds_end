using System.Collections.Generic;
using UnityEngine;

public class Cerebro : MonoBehaviour
{
    public List<Accion> Lista_Acciones = new List<Accion>();  // Lista de acciones
    public Accion Sig_Accion;
    public Accion Accion_Actual;
    private bool Accionando = false;

    // Esta función encuentra la acción con el mayor puntaje en la lista.
    private void Calcular_Acciones(List<Accion> Lista_Compara)
    {
        // Validación para evitar errores si la lista está vacía
        if (Lista_Compara == null || Lista_Compara.Count == 0)
        {
            Debug.Log("La lista de acciones está vacía");
            return;
        }

        float Mayor_Puntaje = 0f;
        Accion accionConMayorPuntaje = null;

        // Recorre cada acción y actualiza la que tiene el mayor puntaje
        foreach (var accion in Lista_Compara)
        {
            float puntajeActual = accion.Calculo_Puntaje();
            if (puntajeActual > Mayor_Puntaje)
            {
                Mayor_Puntaje = puntajeActual;
                accionConMayorPuntaje = accion;
            }
        }

        Sig_Accion = accionConMayorPuntaje;
    }

    // Ejecuta una acción dada.
    private void Ejecutar_Accion(Accion Acc)
    {
        Acc?.Ejecutar(); // Usa operador condicional nulo para evitar cheques adicionales
    }

    void LateUpdate()
    {
        if (!Accionando) // Si no está accionando
        {
            Calcular_Acciones(Lista_Acciones); // Calcula la acción con mayor puntaje

            if (Sig_Accion != Accion_Actual)
            {
                Accion_Actual?.Terminar(); // Termina la acción actual si es diferente
                Ejecutar_Accion(Sig_Accion); // Ejecuta la nueva acción
                Accion_Actual = Sig_Accion; // Actualiza la acción actual
                Accionando = true; // Marca como accionando
            }
        }
        else if (Accion_Actual?.Revisar() == true) // Si la acción actual ha terminado
        {
            Accionando = false; // Deja de accionar
        }
    }
}
