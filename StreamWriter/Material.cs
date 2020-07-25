using System;

namespace Construction
{
    public class Material
    {
        public string Date { get; set; }
        public string Materials { get; set; }
        public decimal Cost { get; set; }

        public void SetData(string mtrl, decimal cs)
        {
            DateTime dt = DateTime.Now;
            Date = dt.ToShortDateString();
            Materials = mtrl;
            Cost = cs;
        }

        public Material()
        {

        }
    }
}
