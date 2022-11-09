using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class sorting_stocks : MonoBehaviour
{
    //handles sorting of element according to their holdings
    [SerializeField] GameObject _place_holder;
    GameObject target;
    int _current_index, _index_to_check;

    private void Start()
    {
        sort_stocks_all();
    }

    void sort_stocks_all() //does initial sorting of all the stocks in ascending order
    {
        int len = transform.childCount;
        for (int i = 0; i < len; i++)
        {
            int largest_num_index = i;
            for (int j = i + 1; j < len; j++)
            {
                if (transform.GetChild(largest_num_index).GetComponent<stock_data>()._holdings < transform.GetChild(j).GetComponent<stock_data>()._holdings)
                {
                    largest_num_index = j;
                }
            }
            transform.GetChild(largest_num_index).SetSiblingIndex(i);
        }
        transform.GetChild(0).GetComponent<stock_data>().on_stock_select(); // selects the first element
    }

    public void onAddRemove_holdings(GameObject _target, int _AddOrRemove)//_target holds the current selected stock,, _AddOrRemove can only be 1 or -1 tells if stock has been added or removed
    {
        target = _target;


        _current_index = _target.transform.GetSiblingIndex();
        _index_to_check = _current_index - _AddOrRemove;


        while (_index_to_check > 0 && _index_to_check < transform.childCount && transform.GetChild(_index_to_check).GetComponent<stock_data>()._holdings * _AddOrRemove < _target.GetComponent<stock_data>()._holdings * _AddOrRemove)
            _index_to_check -= _AddOrRemove;


        if (_current_index - _AddOrRemove != _index_to_check)
        {
            _place_holder.SetActive(true);
            _place_holder.transform.SetParent(transform);
            _place_holder.transform.SetSiblingIndex((_AddOrRemove == 1) ? (_index_to_check + 1) : (_index_to_check));
            _target.transform.SetParent(transform.parent);

            StartCoroutine(moveTowards());
        }

    }

    //public void OnAdd_holdings(GameObject changed_target)
    //{

    //    if (_place_holder.transform.parent != transform)
    //    {
    //        _place_holder.transform.SetParent(transform);
    //        _place_holder.transform.SetSiblingIndex(changed_target.transform.GetSiblingIndex());
    //    }

    //    int above_values = _place_holder.transform.GetSiblingIndex() - 1;
    //    while (above_values >= 0 && changed_target.GetComponent<stock_data>()._holdings > transform.GetChild(above_values).GetComponent<stock_data>()._holdings)
    //    {
    //        above_values -= 1;
    //    }

    //    if (above_values + 1 != _place_holder.transform.GetSiblingIndex())
    //    {
    //        print("add");
    //        _place_holder.transform.SetSiblingIndex(above_values + 1);
    //        _place_holder.SetActive(true);

    //        changed_target.transform.SetParent(transform.parent.parent);
    //        StartCoroutine(moveTowards(changed_target));
    //    }
    //}

    //public void OnRemove_holdings(GameObject changed_target)
    //{
    //    if (_place_holder.transform.parent != transform)
    //    {
    //        _place_holder.transform.SetParent(transform);
    //        _place_holder.transform.SetSiblingIndex(changed_target.transform.GetSiblingIndex() + 1);
    //    }
    //    int above_values = _place_holder.transform.GetSiblingIndex() + 1;
    //    while (above_values < transform.childCount && changed_target.GetComponent<stock_data>()._holdings < transform.GetChild(above_values).GetComponent<stock_data>()._holdings)
    //    {
    //        above_values += 1;
    //    }
    //    if (above_values != _place_holder.transform.GetSiblingIndex())
    //    {
    //        print("in");
    //        _place_holder.transform.SetSiblingIndex(above_values);
    //        _place_holder.SetActive(true);

    //        changed_target.transform.SetParent(transform.parent.parent);
    //        StartCoroutine(moveTowards(changed_target));
    //    }


    //}

    IEnumerator moveTowards()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 new_scroll_post, old_scroll_post = transform.position; //this helps compensate for if stock list is scrolled while lerping

        while ((int)target.transform.position.y / 10 != (int)_place_holder.transform.position.y / 10)
        {
            new_scroll_post = transform.position;
            target.transform.position = Vector3.Lerp(target.transform.position, _place_holder.transform.position, 0.1f) + new_scroll_post - old_scroll_post;
            old_scroll_post = transform.position;

            yield return new WaitForSeconds(0.02f);
        }
        _place_holder.SetActive(false);
        target.transform.SetParent(transform);
        target.transform.SetSiblingIndex(_place_holder.transform.GetSiblingIndex());
        _place_holder.transform.SetParent(this.transform.parent.parent);

        int upordown; //check if the is above below the mask
        if (395 > target.transform.position.y) upordown = 1;
        else upordown = -1;

        while (!RectTransformUtility.RectangleContainsScreenPoint(transform.parent.GetComponent<RectTransform>(), target.transform.position))
        {
            transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition -= 0.1f * upordown;
        }

        StopCoroutine("moveTowards");
        yield return null;
    }


}
