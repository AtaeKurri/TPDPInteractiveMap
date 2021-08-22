using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace TPDPInteractiveMap
{
    public class GensokyoMap
    {
        public int columnNbr { get => 28; }
        public int rowNbr { get => 19; }
        public int minCase { get => 0; }
        public int maxCase { get => 16; }

        public List<ColumnDefinition> returnColumns()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            for (int i = 0; i < columnNbr; i++)
            {
                columns.Add(new ColumnDefinition());
            }
            return columns;
        }

        public List<RowDefinition> returnRows()
        {
            List<RowDefinition> rows = new List<RowDefinition>();
            for (int i = 0; i < rowNbr; i++)
            {
                rows.Add(new RowDefinition());
            }
            return rows;
        }
    }

    public class Cases
    {
        public string name;
        public int minX;
        public int maxX;
        public int minY;
        public int maxY;
        public int positionX;
        public int positionY;
    }
}
