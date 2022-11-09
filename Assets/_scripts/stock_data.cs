using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class stock_data : MonoBehaviour
{
   //handles data for all the stock also updates ui according to data

    [SerializeField] string _name;
    [SerializeField] string _price;
    [SerializeField] string _multiplier;
    public int _holdings;
    [SerializeField] string _details;
    Text[] _child_text;
    [SerializeField] GameObject _details_text; //holds object text for details tab
    [SerializeField] GameObject _selection_disp;

    [SerializeField] AddRemove_stocks _addremove_stocks;

    private void Start()
    {
        _child_text = GetComponentsInChildren<Text>();
        assign();
    }

    public void on_stock_select()
    {
        _details_text.transform.GetChild(0).GetComponent<Text>().text = _details;
        _details_text.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _name;
        _addremove_stocks._current_stock = this;

        _selection_disp.transform.SetParent(null);
        _selection_disp.transform.SetParent(transform);
        _selection_disp.transform.localPosition = new Vector2(-6.449f, 0.82307f);
    }

    void assign() //fill up the repective fields with given data and selects the first one 
    {
        _child_text[0].text = _name;
        _child_text[1].text = _price;
        _child_text[2].text = _multiplier;
        _child_text[3].text = _holdings.ToString();
    }
}
