using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CreateAndSortFields2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var code = textBox1.Text;
            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            var fieldInfos = ConvertToFieldInfos(code);
            fieldInfos.Sort(new FieldInfo());

            var result = string.Join(";\n", fieldInfos.ToArray());
            result += ";\n\n";

            var constructor = string.Join(",\n\t", fieldInfos.Select(x => x.ToConstructorString()));
            var assignments = string.Join(";\n\t", fieldInfos.Select(x => x.ToAssignmentString()));

            var className = textBox2.Text;

            result += $"public {className}(\n\t{constructor})\n{{\n\t{assignments};\n}}";

            Clipboard.SetText(result);
            MessageBox.Show("Copied contents to clipboard");
        }

        List<FieldInfo> ConvertToFieldInfos(string code)
        {
            var stringReader = new StringReader(code);

            var list = new List<FieldInfo>();
            while (true)
            {
                var line = stringReader.ReadLine();
                if (line == null)
                {
                    break;
                }

                var tokenIgnored = new string[] { "readonly", "static" };
                var tokens = line.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var filteredTokens = tokens.Where(token => !tokenIgnored.Contains(token)).ToList();

                if (filteredTokens.Count != 2)
                {
                    continue;
                }

                list.Add(new FieldInfo()
                {
                    Class = filteredTokens[0],
                    Name = filteredTokens[1]
                });
            }

            return list;
        }

        struct FieldInfo : IComparer<FieldInfo>
        {
            public string Class;
            public string Name;

            public int Compare(FieldInfo x, FieldInfo y)
            {
                return x.ToString().Length - y.ToString().Length;
            }

            public override string ToString()
            {
                return $"readonly {Class} {Name}";
            }

            public string ToConstructorString()
            {
                return $"{Class} {(Name.StartsWith("_") ? Name.Substring(1) : Name)}";
            }

            public string ToAssignmentString()
            {
                return $"{Name} = {(Name.StartsWith("_") ? Name.Substring(1) : Name)}";
            }
        }
    }
}
