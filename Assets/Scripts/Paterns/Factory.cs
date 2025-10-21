using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private Arrow[] _arrowsArr;
    private Dictionary<string, Arrow> _idArrow;

    [SerializeField] private Transform _container;

    private void Awake()
    {
        _idArrow = new Dictionary<string, Arrow>();

        foreach (Arrow arrow in _arrowsArr)
        {
            _idArrow.Add(arrow.GetArrowData().idName, arrow);
        }
    }

    public Arrow CreateArrow(string id)
    {
        if (!_idArrow.TryGetValue(id, out Arrow arrow))
            return null;

        return Instantiate(arrow, _container);
    }
}