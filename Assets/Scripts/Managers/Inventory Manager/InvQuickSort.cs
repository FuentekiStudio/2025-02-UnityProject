using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvQuickSort
{

    //private void Start()
    //{

    //    for (int i = 0; i < 10; i++)
    //    {
    //        arrayPlayer[i] = new Player();
    //        arrayPlayer[i].name = "Player_" + i.ToString();
    //        arrayPlayer[i].score = Random.Range(1, 100);
    //    }

    //    Debug.Log("Inicio Programa: Quick Sort");

    //    // muestro vector desordenado
    //    Debug.Log("\nLista Desordenada: ");
    //    ImprimirVector(arrayPlayer);

    //    // algoritmo de ordenamiento
    //    // inicialmente los parametros left y right son los extremos del vector
    //    QSort(arrayPlayer, 0, arrayPlayer.Length - 1);

    //    // muestro vector ordenado
    //    Debug.Log("\nLista Ordenada: ");
    //    ImprimirVector(arrayPlayer);
    //}

    public int Partition(List<InventoryItem> items, int left, int right)
    {
        int pivot;
        int aux = (left + right) / 2;   //tomo el valor central del vector
        pivot = items[aux].stackSize;

        // en este ciclo debo dejar todos los valores menores al pivot
        // a la izquierda y los mayores a la derecha
        while (true)
        {
            Debug.Log($"hola {pivot}");

            while (items[left].stackSize < pivot)
            {
                left++;
                Debug.Log(left);
            }
            while (items[right].stackSize > pivot)
            {
                right--;
                Debug.Log(right);
            }

            if (items[left].stackSize == items[right].stackSize)
            {
                Debug.Log($"sayonara :) - {left} {right}");
                return right;
            }
            if (left < right)
            {
                Debug.Log("A");
                InventoryItem temp = items[right];
                items[right] = items[left];
                items[left] = temp;
            }
            else
            {
                // este es el valor que devuelvo como proxima posicion de
                // la particion en el siguiente paso del algoritmo
                Debug.Log("B");
                return right;
            }
        }
    }
    public void QSort(List<InventoryItem> items, int left, int right)
    {
        int pivot;
        if (left < right)
        {
            pivot = Partition(items, left, right);
            if (pivot > 1)
            {
                // mitad del lado izquierdo del vector
                QSort(items, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                // mitad del lado derecho del vector
                QSort(items, pivot + 1, right);
            }
        }
    }

    public void ImprimirVector(List<InventoryItem> items)
    {
        string str = "";
        for (int i = 0; i < items.Count; i++)
        {
            str += $"{items[i].data.itemName} {items[i].stackSize}, ";
        }

        Debug.Log(str);
    }

}

