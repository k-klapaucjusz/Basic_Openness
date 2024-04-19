using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    public class TreeNode
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public ObservableCollection<TreeNode> Children { get; set; }

        public TreeNode(string name, string number = "", string type = "")
        {
            Name = name;
            Number = number;
            Type = type;
            Children = new ObservableCollection<TreeNode>();
            
        }
    }
}
