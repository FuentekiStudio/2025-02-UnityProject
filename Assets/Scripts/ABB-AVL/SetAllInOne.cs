using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SetTDA
{
    void Inicialize();
    bool IsEmpty();
    void Add(int x);
    int Choose();
    void Remove(int x);
    bool Exists(int x);
}

public class Node
{
    public int info;
    public Node next;

    public Node(int info, Node next)
    {
        this.info = info;
        this.next = next;
    }
}

// IMPLEMENTACIÓN DINÁMICA //
public class DynSet : SetTDA
{
    Node setRoot;
    public void Inicialize()
    {
        setRoot = null;
    }

    public bool IsEmpty()
    {
        return (setRoot == null);
    }

    public void Add(int x)
    {
        /* Verifica que x no este en el conjunto */
        if (!Exists(x))
        {
            Node aux = new Node(x, setRoot);
            setRoot = aux;
        }
    }

    public int Choose()
    {
        return setRoot.info;
    }

    public void Remove(int x)
    {
        if (setRoot != null)
        {
            // si es el primer elemento de la lista
            if (setRoot.info == x)
            {
                setRoot = setRoot.next;
            }
            else
            {
                Node aux = setRoot;
                while (aux.next != null && aux.next.info != x)
                    aux = aux.next;
                if (aux.next != null)
                    aux.next = aux.next.next;
            }
        }
    }

    public bool Exists(int x)
    {
        Node aux = setRoot;
        while ((aux != null) && (aux.info != x))
        {
            aux = aux.next;
        }
        return (aux != null);
    }
}

// IMPLEMENTACIÓN ESTÁTICA //
public class StaticSet : SetTDA
{
    int[] set;
    int amount;

    public StaticSet(int setLength)
    {
        set = new int[setLength];
    }

    public void Add(int x)
    {
        if (!this.Exists(x))
        {
            set[amount] = x;
            amount++;
        }
    }

    public bool IsEmpty()
    {
        return amount == 0;
    }

    public int Choose()
    {
        return set[amount - 1];
    }

    public void Inicialize()
    {
        amount = 0;
    }

    public bool Exists(int x)
    {
        int i = 0;
        while (i < amount && set[i] != x)
        {
            i++;
        }
        return (i < amount);
    }

    public void Remove(int x)
    {
        int i = 0;
        while (i < amount && set[i] != x)
        {
            i++;
        }
        if (i < amount)
        {
            set[i] = set[amount - 1];
            amount--;
        }
    }
}