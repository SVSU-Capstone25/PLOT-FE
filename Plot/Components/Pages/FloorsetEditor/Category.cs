//Tristan Calay 4/7/25
//Rework the allocations sidebar category struct into a reusable class
//Stores progress bar category data
public class Category
{
    public bool isHeader { get; set; } = false;
    public String headerCategory = "";
    public String text { get; set; } = "";
    public String color { get; set; } = "red";
    public int maxValue { get; set; } = 100;
    public int currentValue { get; set; } = 0;

    // Tristan Calay 4/2/25
    // Added selection tracking to this struct, for state persistence.
    public bool isSelected { get; set; } = false;

    //Returns "Transparent" or the green selected color for the bar,
    // depending on isSelected.
    public String getStyle()
    {
        if (isSelected)
        {
            Console.WriteLine("A selected style is being applied...");
            return "background-color: rgba(0,255,0,.3);";
        }
        return "background-color: transparent;";
    }

    public Category(String _headerCategory, String _text, String _color, int _maxValue, int _currentValue, bool _header = false)
    {
        headerCategory = _headerCategory;
        text = _text;
        color = _color;
        maxValue = _maxValue;
        currentValue = _currentValue;
        isHeader = _header;

        Console.WriteLine("Header is " + isHeader.ToString());
    }

    public String getLinearFeet()
    {
        return currentValue + "/" + maxValue + " LF";
    }

    public bool isInSearch(String searchText)
    {
        if (searchText == "")
        {
            return true;
        }
        return (headerCategory + " " + text).ToLower().Contains(searchText.ToLower());
    }

    public Double getPercent()
    {
        return Convert.ToDouble(currentValue) / Convert.ToDouble(maxValue);
    }

    override public String ToString()
    {
        return String.Format("{0} {1} ({2}): {3} of {4}. Header : {5}. Selected : {6}", headerCategory, text, color, currentValue, maxValue, isHeader, isSelected);
    }


    public enum SORTTYPE { ASCENDING, DESCENDING };


    //Sorting function for category data.
    public static int compareCategories(Category a, Category b, SORTTYPE sort)
    {
        //Both are null, return equivalent
        if (a.Equals(null) && b.Equals(null))
        {
            //Console.WriteLine("Both are null!");
            return 0;
        }

        //Neither null, return header first, else sort by fullfillment percentage.
        if (!a!.Equals(null) && !b.Equals(null))
        {
            if (a.isHeader)
            {
                //Console.WriteLine("A is header! Keep A first.");
                return -1;
            }
            if (b.isHeader)
            {
                //Console.WriteLine("B is header! Should swap.");
                return 1;
            }
            //Console.WriteLine("Standard Sort!");
            //Sort here by percent complete. To do different sort methods, add them here.
            switch (sort)
            {
                case SORTTYPE.ASCENDING:
                    return (a.getPercent()).CompareTo(b.getPercent());
                case SORTTYPE.DESCENDING:
                    return (b.getPercent()).CompareTo(a.getPercent());
            }

        }

        //First not null, return first
        if (!a!.Equals(null))
        {
            //Console.WriteLine("First is only non null item!");
            return -1;
        }

        //Return second.
        //Console.WriteLine("Default to second : only non null item!");
        return 1;

    }
}